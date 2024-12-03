using LosPollos.Backend.Api.Context;
using LosPollos.Backend.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LosPollos.Backend.Api.Services;

public class UserService : IUserService
{
    private readonly ApplicationContext _context;

    public UserService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        User? user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user is null || user.Password != password)
        {
            return null;
        }
        
        return user;
    }
}