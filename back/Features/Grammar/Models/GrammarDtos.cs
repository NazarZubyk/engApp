namespace Back.Features.Grammar.Models;

public record TopicTreeNodeDto(
    string Slug,
    string Title,
    int SortOrder,
    int ExerciseCount,
    string MasteryLevel,
    double AccuracyPct,
    IReadOnlyList<TopicTreeNodeDto> Children);

public record TopicDetailDto(
    string Slug,
    string Title,
    string ExplanationMd,
    int ExerciseCount,
    int TotalAttempts,
    int TotalCorrect,
    double AccuracyPct,
    string MasteryLevel,
    int WeakExerciseCount);

public record ImportTopicDto(
    string Slug,
    string? ParentSlug,
    string Title,
    string ExplanationMd,
    int SortOrder);

public record ImportSlotDto(
    string? Label,
    IReadOnlyList<string> Accepted,
    IReadOnlyList<string> Distractors);

public record ImportExerciseDto(
    string Prompt,
    string? Notes,
    IReadOnlyList<ImportSlotDto> Slots);

public record ImportRequest(
    string ImportMode,
    ImportTopicDto Topic,
    IReadOnlyList<ImportExerciseDto> Exercises);

public record ImportResponse(
    int TopicId,
    int ExercisesCreated,
    int ExercisesTotal,
    int SlotsCreated,
    IReadOnlyList<string> Warnings);
