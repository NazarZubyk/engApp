using Back.Infrastructure.Persistence.Entities;

namespace Back.Infrastructure.Persistence;

public interface IUserRepository
{
    Task<IReadOnlyList<UserRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserRecord?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<UserRecord> CreateAsync(string login, string passwordHash, CancellationToken cancellationToken = default);
    Task<UserRecord?> UpdateAsync(int id, string? login, string? passwordHash, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
