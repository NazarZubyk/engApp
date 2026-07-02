using Back.Features.Auth.Models;
using Back.Infrastructure.Auth;
using Back.Infrastructure.Persistence;
using Back.Infrastructure.Persistence.Entities;

namespace Back.Features.Auth;

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UsersService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(ToDto).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        return user is null ? null : ToDto(user);
    }

    public async Task<(UserDto? User, string? Error)> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
        {
            return (null, "Login and password are required.");
        }

        var existing = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
        if (existing is not null)
        {
            return (null, "Login already exists.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = await _userRepository.CreateAsync(request.Login, passwordHash, cancellationToken);
        return (ToDto(user), null);
    }

    public async Task<(UserDto? User, string? Error)> UpdateAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Login is not null)
        {
            var existing = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
            if (existing is not null && existing.Id != id)
            {
                return (null, "Login already exists.");
            }
        }

        var passwordHash = request.Password is null ? null : _passwordHasher.Hash(request.Password);
        var user = await _userRepository.UpdateAsync(id, request.Login, passwordHash, cancellationToken);
        return user is null ? (null, null) : (ToDto(user), null);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        _userRepository.DeleteAsync(id, cancellationToken);

    private static UserDto ToDto(UserRecord user) =>
        new(user.Id, user.Login, user.CreatedAt);
}
