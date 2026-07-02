namespace Back.Infrastructure.Auth;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) CreateAdminToken(int adminId, string username);
    (string Token, DateTime ExpiresAt) CreateUserToken(int userId, string login);
}
