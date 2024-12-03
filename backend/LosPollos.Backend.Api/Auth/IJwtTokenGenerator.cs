using LosPollos.Backend.Api.Entities;

namespace LosPollos.Backend.Api.Auth;

public interface IJwtTokenGenerator
{
    public string GenerateToken(User user);
}