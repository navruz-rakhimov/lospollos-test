using LosPollos.Backend.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LosPollos.Backend.Api.Context;

public class SeedData
{
    public static async Task AddUsers(ApplicationContext dbContext)
    {
        if (!await dbContext.Users.AnyAsync())
        {
            await dbContext.Users.AddRangeAsync(GetUsers());
            await dbContext.SaveChangesAsync();
        }
    }
    
    private static User[] GetUsers() => 
    [
        User.Create("Navruz", "password"),
        User.Create("Carl", "password"),
        User.Create("Sabina", "password"),
        User.Create("Marcus", "password", false),
        User.Create("David", "password", false)
    ];
}