namespace Back.Infrastructure.Persistence.Entities;

public record GrammarTopicRecord(
    int Id,
    int? ParentId,
    string Slug,
    string Title,
    string ExplanationMd,
    int SortOrder,
    DateTimeOffset CreatedAt,
    int ExerciseCount);
