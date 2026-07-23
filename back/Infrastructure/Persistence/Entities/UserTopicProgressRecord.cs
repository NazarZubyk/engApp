namespace Back.Infrastructure.Persistence.Entities;

public record UserTopicProgressRecord(
    int TotalAttempts,
    int TotalCorrect,
    DateTimeOffset? LastStudiedAt);
