using LosPollos.Backend.Api.Entities;

namespace LosPollos.Backend.Api.Services;

public interface IUserService
{
    Task<User?> AuthenticateAsync(string username, string password);
}


