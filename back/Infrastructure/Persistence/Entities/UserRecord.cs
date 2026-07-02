namespace Back.Infrastructure.Persistence.Entities;

public record UserRecord(int Id, string Login, string PasswordHash, DateTimeOffset CreatedAt);
