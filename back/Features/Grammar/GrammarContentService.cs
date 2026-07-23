using Back.Features.Grammar.Models;
using Back.Infrastructure.Persistence;
using Back.Infrastructure.Persistence.Entities;

namespace Back.Features.Grammar;

public class GrammarContentService : IGrammarContentService
{
    private readonly IGrammarContentRepository _repository;

    public GrammarContentService(IGrammarContentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TopicTreeNodeDto>> GetTopicTreeAsync(
        int? userId,
        CancellationToken cancellationToken = default)
    {
        var topics = await _repository.GetAllTopicsWithCountsAsync(cancellationToken);
        var progressByTopicId = await LoadProgressMapAsync(userId, topics, cancellationToken);
        return BuildTree(topics, progressByTopicId, parentId: null);
    }

    public async Task<TopicDetailDto?> GetTopicDetailAsync(
        string slug,
        int? userId,
        CancellationToken cancellationToken = default)
    {
        var topic = await _repository.GetTopicBySlugWithCountAsync(slug, cancellationToken);
        if (topic is null)
        {
            return null;
        }

        var progress = userId is null
            ? null
            : await _repository.GetUserTopicProgressAsync(userId.Value, topic.Id, cancellationToken);

        var stats = ComputeTopicStats(progress);
        return new TopicDetailDto(
            topic.Slug,
            topic.Title,
            topic.ExplanationMd,
            topic.ExerciseCount,
            stats.TotalAttempts,
            stats.TotalCorrect,
            stats.AccuracyPct,
            stats.MasteryLevel,
            WeakExerciseCount: 0);
    }

    public async Task<(ImportResponse? Result, string? Error)> ImportAsync(
        ImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationError = ValidateImportRequest(request);
        if (validationError is not null)
        {
            return (null, validationError);
        }

        try
        {
            var payload = new GrammarImportPayload(
                request.ImportMode,
                new GrammarImportTopicPayload(
                    request.Topic.Slug.Trim(),
                    request.Topic.ParentSlug?.Trim(),
                    request.Topic.Title.Trim(),
                    request.Topic.ExplanationMd,
                    request.Topic.SortOrder),
                request.Exercises.Select(exercise => new GrammarImportExercisePayload(
                    exercise.Prompt.Trim(),
                    exercise.Notes?.Trim(),
                    exercise.Slots.Select(slot => new GrammarImportSlotPayload(
                        slot.Label?.Trim(),
                        slot.Accepted.Select(a => a.Trim()).Where(a => a.Length > 0).ToList(),
                        slot.Distractors.Select(d => d.Trim()).Where(d => d.Length > 0).ToList())).ToList())).ToList());

            var result = await _repository.ImportAsync(payload, cancellationToken);
            return (new ImportResponse(
                result.TopicId,
                result.ExercisesCreated,
                result.ExercisesTotal,
                result.SlotsCreated,
                result.Warnings), null);
        }
        catch (GrammarImportException ex)
        {
            return (null, ex.Message);
        }
    }

    private async Task<Dictionary<int, UserTopicProgressRecord>> LoadProgressMapAsync(
        int? userId,
        IReadOnlyList<GrammarTopicRecord> topics,
        CancellationToken cancellationToken)
    {
        var map = new Dictionary<int, UserTopicProgressRecord>();
        if (userId is null)
        {
            return map;
        }

        foreach (var topic in topics)
        {
            var progress = await _repository.GetUserTopicProgressAsync(userId.Value, topic.Id, cancellationToken);
            if (progress is not null)
            {
                map[topic.Id] = progress;
            }
        }

        return map;
    }

    private static IReadOnlyList<TopicTreeNodeDto> BuildTree(
        IReadOnlyList<GrammarTopicRecord> topics,
        IReadOnlyDictionary<int, UserTopicProgressRecord> progressByTopicId,
        int? parentId)
    {
        return topics
            .Where(topic => topic.ParentId == parentId)
            .OrderBy(topic => topic.SortOrder)
            .ThenBy(topic => topic.Title)
            .Select(topic =>
            {
                progressByTopicId.TryGetValue(topic.Id, out var progress);
                var stats = ComputeTopicStats(progress);
                return new TopicTreeNodeDto(
                    topic.Slug,
                    topic.Title,
                    topic.SortOrder,
                    topic.ExerciseCount,
                    stats.MasteryLevel,
                    stats.AccuracyPct,
                    BuildTree(topics, progressByTopicId, topic.Id));
            })
            .ToList();
    }

    private static TopicStats ComputeTopicStats(UserTopicProgressRecord? progress)
    {
        if (progress is null || progress.TotalAttempts == 0)
        {
            return new TopicStats(0, 0, 0, "not_started");
        }

        var accuracyPct = Math.Round(progress.TotalCorrect * 100.0 / progress.TotalAttempts, 1);
        var masteryLevel = ComputeMasteryLevel(progress.TotalAttempts, accuracyPct);
        return new TopicStats(progress.TotalAttempts, progress.TotalCorrect, accuracyPct, masteryLevel);
    }

    internal static string ComputeMasteryLevel(int totalAttempts, double accuracyPct)
    {
        if (totalAttempts == 0)
        {
            return "not_started";
        }

        if (totalAttempts < 10 || accuracyPct < 70)
        {
            return "learning";
        }

        if (accuracyPct < 90)
        {
            return "practicing";
        }

        return totalAttempts >= 20 ? "strong" : "practicing";
    }

    private static string? ValidateImportRequest(ImportRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ImportMode))
        {
            return "importMode is required.";
        }

        if (string.IsNullOrWhiteSpace(request.Topic.Slug))
        {
            return "topic.slug is required.";
        }

        if (string.IsNullOrWhiteSpace(request.Topic.Title))
        {
            return "topic.title is required.";
        }

        if (request.Exercises.Count == 0)
        {
            return "At least one exercise is required.";
        }

        for (var i = 0; i < request.Exercises.Count; i++)
        {
            var exercise = request.Exercises[i];
            if (string.IsNullOrWhiteSpace(exercise.Prompt))
            {
                return $"Exercise {i + 1}: prompt is required.";
            }

            if (exercise.Slots.Count == 0)
            {
                return $"Exercise {i + 1}: at least one slot is required.";
            }

            for (var j = 0; j < exercise.Slots.Count; j++)
            {
                var slot = exercise.Slots[j];
                if (slot.Accepted.Count == 0 || slot.Accepted.All(string.IsNullOrWhiteSpace))
                {
                    return $"Exercise {i + 1}, slot {j + 1}: at least one accepted answer is required.";
                }
            }
        }

        return null;
    }

    private record TopicStats(int TotalAttempts, int TotalCorrect, double AccuracyPct, string MasteryLevel);
}
