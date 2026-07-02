namespace Back.Features.Auth.Models;

public record TokenResponse(string Token, DateTime ExpiresAt);
