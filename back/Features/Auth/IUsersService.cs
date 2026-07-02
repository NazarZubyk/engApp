using Back.Features.Auth.Models;

namespace Back.Features.Auth;

public interface IUsersService
{
    Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(UserDto? User, string? Error)> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<(UserDto? User, string? Error)> UpdateAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
