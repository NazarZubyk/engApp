using System.Text.RegularExpressions;
using Back.Infrastructure.Persistence.Entities;
using Npgsql;

namespace Back.Infrastructure.Persistence;

public partial class GrammarContentRepository : IGrammarContentRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public GrammarContentRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<IReadOnlyList<GrammarTopicRecord>> GetAllTopicsWithCountsAsync(
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            SELECT
                t.id,
                t.parent_id,
                t.slug,
                t.title,
                t.explanation_md,
                t.sort_order,
                t.created_at,
                COUNT(e.id)::int AS exercise_count
            FROM grammar_topics t
            LEFT JOIN grammar_exercises e ON e.topic_id = t.id
            GROUP BY t.id
            ORDER BY t.sort_order, t.title
            """,
            connection);

        var topics = new List<GrammarTopicRecord>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            topics.Add(ReadTopic(reader));
        }

        return topics;
    }

    public async Task<GrammarTopicRecord?> GetTopicBySlugWithCountAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            SELECT
                t.id,
                t.parent_id,
                t.slug,
                t.title,
                t.explanation_md,
                t.sort_order,
                t.created_at,
                COUNT(e.id)::int AS exercise_count
            FROM grammar_topics t
            LEFT JOIN grammar_exercises e ON e.topic_id = t.id
            WHERE t.slug = @slug
            GROUP BY t.id
            """,
            connection);

        command.Parameters.AddWithValue("slug", slug);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadTopic(reader) : null;
    }

    public async Task<bool> TopicExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            "SELECT 1 FROM grammar_topics WHERE slug = @slug",
            connection);

        command.Parameters.AddWithValue("slug", slug);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is not null;
    }

    public async Task<int?> GetTopicIdBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            "SELECT id FROM grammar_topics WHERE slug = @slug",
            connection);

        command.Parameters.AddWithValue("slug", slug);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is int id ? id : null;
    }

    public async Task<UserTopicProgressRecord?> GetUserTopicProgressAsync(
        int userId,
        int topicId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            SELECT total_attempts, total_correct, last_studied_at
            FROM user_topic_progress
            WHERE user_id = @user_id AND topic_id = @topic_id
            """,
            connection);

        command.Parameters.AddWithValue("user_id", userId);
        command.Parameters.AddWithValue("topic_id", topicId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new UserTopicProgressRecord(
            reader.GetInt32(0),
            reader.GetInt32(1),
            reader.IsDBNull(2) ? null : reader.GetFieldValue<DateTimeOffset>(2));
    }

    public async Task<GrammarImportResult> ImportAsync(
        GrammarImportPayload payload,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var warnings = new List<string>();
            var importMode = payload.ImportMode.ToLowerInvariant();
            var topicExists = await TopicExistsBySlugAsync(connection, transaction, payload.Topic.Slug, cancellationToken);

            int topicId;
            switch (importMode)
            {
                case "create":
                    if (topicExists)
                    {
                        throw new GrammarImportException("Topic slug already exists.");
                    }

                    topicId = await InsertTopicAsync(connection, transaction, payload.Topic, cancellationToken);
                    break;

                case "append":
                    if (!topicExists)
                    {
                        throw new GrammarImportException("Topic not found for append.");
                    }

                    topicId = (await GetTopicIdBySlugAsync(connection, transaction, payload.Topic.Slug, cancellationToken))!.Value;
                    await UpdateTopicMetadataAsync(connection, transaction, topicId, payload.Topic, cancellationToken);
                    break;

                case "replace":
                    if (topicExists)
                    {
                        topicId = (await GetTopicIdBySlugAsync(connection, transaction, payload.Topic.Slug, cancellationToken))!.Value;
                        await UpdateTopicMetadataAsync(connection, transaction, topicId, payload.Topic, cancellationToken);
                        await DeleteExercisesByTopicIdAsync(connection, transaction, topicId, cancellationToken);
                    }
                    else
                    {
                        topicId = await InsertTopicAsync(connection, transaction, payload.Topic, cancellationToken);
                    }

                    break;

                default:
                    throw new GrammarImportException("Invalid importMode. Use create, append, or replace.");
            }

            var startSortOrder = importMode == "append"
                ? await GetMaxExerciseSortOrderAsync(connection, transaction, topicId, cancellationToken) + 1
                : 0;

            var exercisesCreated = 0;
            var slotsCreated = 0;

            for (var i = 0; i < payload.Exercises.Count; i++)
            {
                var exercise = payload.Exercises[i];
                var sortOrder = startSortOrder + i;
                var exerciseId = await InsertExerciseAsync(
                    connection,
                    transaction,
                    topicId,
                    exercise.Prompt,
                    exercise.Notes,
                    sortOrder,
                    cancellationToken);

                exercisesCreated++;

                for (var slotIndex = 0; slotIndex < exercise.Slots.Count; slotIndex++)
                {
                    var slot = exercise.Slots[slotIndex];
                    var slotId = await InsertSlotAsync(
                        connection,
                        transaction,
                        exerciseId,
                        slotIndex,
                        slot.Label,
                        cancellationToken);

                    slotsCreated++;

                    foreach (var accepted in slot.Accepted)
                    {
                        await InsertAcceptedAnswerAsync(
                            connection,
                            transaction,
                            slotId,
                            accepted,
                            NormalizeAnswer(accepted),
                            cancellationToken);
                    }

                    foreach (var distractor in slot.Distractors)
                    {
                        await InsertDistractorAsync(connection, transaction, slotId, distractor, cancellationToken);
                    }
                }
            }

            var exercisesTotal = await CountExercisesByTopicIdAsync(connection, transaction, topicId, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new GrammarImportResult(topicId, exercisesCreated, exercisesTotal, slotsCreated, warnings);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static GrammarTopicRecord ReadTopic(NpgsqlDataReader reader) =>
        new(
            reader.GetInt32(0),
            reader.IsDBNull(1) ? null : reader.GetInt32(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetInt32(5),
            reader.GetFieldValue<DateTimeOffset>(6),
            reader.GetInt32(7));

    private static async Task<bool> TopicExistsBySlugAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string slug,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "SELECT 1 FROM grammar_topics WHERE slug = @slug",
            connection,
            transaction);

        command.Parameters.AddWithValue("slug", slug);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is not null;
    }

    private static async Task<int?> GetTopicIdBySlugAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string slug,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "SELECT id FROM grammar_topics WHERE slug = @slug",
            connection,
            transaction);

        command.Parameters.AddWithValue("slug", slug);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is int id ? id : null;
    }

    private static async Task<int?> ResolveParentIdAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string? parentSlug,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(parentSlug))
        {
            return null;
        }

        var parentId = await GetTopicIdBySlugAsync(connection, transaction, parentSlug, cancellationToken);
        if (parentId is null)
        {
            throw new GrammarImportException($"Parent topic '{parentSlug}' not found.");
        }

        return parentId;
    }

    private static async Task<int> InsertTopicAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        GrammarImportTopicPayload topic,
        CancellationToken cancellationToken)
    {
        var parentId = await ResolveParentIdAsync(connection, transaction, topic.ParentSlug, cancellationToken);

        await using var command = new NpgsqlCommand(
            """
            INSERT INTO grammar_topics (parent_id, slug, title, explanation_md, sort_order)
            VALUES (@parent_id, @slug, @title, @explanation_md, @sort_order)
            RETURNING id
            """,
            connection,
            transaction);

        command.Parameters.AddWithValue("parent_id", (object?)parentId ?? DBNull.Value);
        command.Parameters.AddWithValue("slug", topic.Slug);
        command.Parameters.AddWithValue("title", topic.Title);
        command.Parameters.AddWithValue("explanation_md", topic.ExplanationMd);
        command.Parameters.AddWithValue("sort_order", topic.SortOrder);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return (int)result!;
    }

    private static async Task UpdateTopicMetadataAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int topicId,
        GrammarImportTopicPayload topic,
        CancellationToken cancellationToken)
    {
        var parentId = await ResolveParentIdAsync(connection, transaction, topic.ParentSlug, cancellationToken);

        await using var command = new NpgsqlCommand(
            """
            UPDATE grammar_topics
            SET
                parent_id = @parent_id,
                title = @title,
                explanation_md = @explanation_md,
                sort_order = @sort_order
            WHERE id = @id
            """,
            connection,
            transaction);

        command.Parameters.AddWithValue("id", topicId);
        command.Parameters.AddWithValue("parent_id", (object?)parentId ?? DBNull.Value);
        command.Parameters.AddWithValue("title", topic.Title);
        command.Parameters.AddWithValue("explanation_md", topic.ExplanationMd);
        command.Parameters.AddWithValue("sort_order", topic.SortOrder);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task DeleteExercisesByTopicIdAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int topicId,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "DELETE FROM grammar_exercises WHERE topic_id = @topic_id",
            connection,
            transaction);

        command.Parameters.AddWithValue("topic_id", topicId);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<int> GetMaxExerciseSortOrderAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int topicId,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "SELECT COALESCE(MAX(sort_order), -1) FROM grammar_exercises WHERE topic_id = @topic_id",
            connection,
            transaction);

        command.Parameters.AddWithValue("topic_id", topicId);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    private static async Task<int> InsertExerciseAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int topicId,
        string prompt,
        string? notes,
        int sortOrder,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            """
            INSERT INTO grammar_exercises (topic_id, prompt, sort_order, notes)
            VALUES (@topic_id, @prompt, @sort_order, @notes)
            RETURNING id
            """,
            connection,
            transaction);

        command.Parameters.AddWithValue("topic_id", topicId);
        command.Parameters.AddWithValue("prompt", prompt);
        command.Parameters.AddWithValue("sort_order", sortOrder);
        command.Parameters.AddWithValue("notes", (object?)notes ?? DBNull.Value);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return (int)result!;
    }

    private static async Task<int> InsertSlotAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int exerciseId,
        int slotIndex,
        string? label,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            """
            INSERT INTO exercise_answer_slots (exercise_id, slot_index, label)
            VALUES (@exercise_id, @slot_index, @label)
            RETURNING id
            """,
            connection,
            transaction);

        command.Parameters.AddWithValue("exercise_id", exerciseId);
        command.Parameters.AddWithValue("slot_index", slotIndex);
        command.Parameters.AddWithValue("label", (object?)label ?? DBNull.Value);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return (int)result!;
    }

    private static async Task InsertAcceptedAnswerAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int slotId,
        string text,
        string normalizedText,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            """
            INSERT INTO accepted_answers (slot_id, text, normalized_text)
            VALUES (@slot_id, @text, @normalized_text)
            """,
            connection,
            transaction);

        command.Parameters.AddWithValue("slot_id", slotId);
        command.Parameters.AddWithValue("text", text);
        command.Parameters.AddWithValue("normalized_text", normalizedText);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task InsertDistractorAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int slotId,
        string text,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            """
            INSERT INTO mcq_distractors (slot_id, text)
            VALUES (@slot_id, @text)
            """,
            connection,
            transaction);

        command.Parameters.AddWithValue("slot_id", slotId);
        command.Parameters.AddWithValue("text", text);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<int> CountExercisesByTopicIdAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        int topicId,
        CancellationToken cancellationToken)
    {
        await using var command = new NpgsqlCommand(
            "SELECT COUNT(*)::int FROM grammar_exercises WHERE topic_id = @topic_id",
            connection,
            transaction);

        command.Parameters.AddWithValue("topic_id", topicId);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return (int)result!;
    }

    internal static string NormalizeAnswer(string text)
    {
        var trimmed = text.Trim().ToLowerInvariant();
        return WhitespaceRegex().Replace(trimmed, " ");
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}

public class GrammarImportException : Exception
{
    public GrammarImportException(string message) : base(message)
    {
    }
}
