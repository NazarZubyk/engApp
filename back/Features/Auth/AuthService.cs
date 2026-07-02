using Back.Features.Auth.Models;
using Back.Infrastructure.Auth;
using Back.Infrastructure.Persistence;

namespace Back.Features.Auth;

public class AuthService : IAuthService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IAdminRepository adminRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _adminRepository = adminRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<TokenResponse?> AdminLoginAsync(AdminLoginRequest request, CancellationToken cancellationToken = default)
    {
        var admin = await _adminRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (admin is null || !_passwordHasher.Verify(request.Password, admin.PasswordHash))
        {
            return null;
        }

        var (token, expiresAt) = _jwtTokenService.CreateAdminToken(admin.Id, admin.Username);
        return new TokenResponse(token, expiresAt);
    }

    public async Task<TokenResponse?> UserLoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var (token, expiresAt) = _jwtTokenService.CreateUserToken(user.Id, user.Login);
        return new TokenResponse(token, expiresAt);
    }
}
