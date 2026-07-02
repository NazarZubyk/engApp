using Back.Features.Auth.Models;

namespace Back.Features.Auth;

public interface IAuthService
{
    Task<TokenResponse?> AdminLoginAsync(AdminLoginRequest request, CancellationToken cancellationToken = default);
    Task<TokenResponse?> UserLoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default);
}
