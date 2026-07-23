using Back.Infrastructure.Persistence.Entities;

namespace Back.Infrastructure.Persistence;

public interface IGrammarContentRepository
{
    Task<IReadOnlyList<GrammarTopicRecord>> GetAllTopicsWithCountsAsync(CancellationToken cancellationToken = default);

    Task<GrammarTopicRecord?> GetTopicBySlugWithCountAsync(string slug, CancellationToken cancellationToken = default);

    Task<bool> TopicExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<int?> GetTopicIdBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<UserTopicProgressRecord?> GetUserTopicProgressAsync(
        int userId,
        int topicId,
        CancellationToken cancellationToken = default);

    Task<GrammarImportResult> ImportAsync(GrammarImportPayload payload, CancellationToken cancellationToken = default);
}

public record GrammarImportPayload(
    string ImportMode,
    GrammarImportTopicPayload Topic,
    IReadOnlyList<GrammarImportExercisePayload> Exercises);

public record GrammarImportTopicPayload(
    string Slug,
    string? ParentSlug,
    string Title,
    string ExplanationMd,
    int SortOrder);

public record GrammarImportExercisePayload(
    string Prompt,
    string? Notes,
    IReadOnlyList<GrammarImportSlotPayload> Slots);

public record GrammarImportSlotPayload(
    string? Label,
    IReadOnlyList<string> Accepted,
    IReadOnlyList<string> Distractors);

public record GrammarImportResult(
    int TopicId,
    int ExercisesCreated,
    int ExercisesTotal,
    int SlotsCreated,
    IReadOnlyList<string> Warnings);
