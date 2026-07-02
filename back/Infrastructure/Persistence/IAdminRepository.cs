using Back.Infrastructure.Persistence.Entities;

namespace Back.Infrastructure.Persistence;

public interface IAdminRepository
{
    Task<AdminRecord?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
