namespace LosPollos.Backend.Api.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool IsActive { get; set; } = true;

    public static User Create(string username, string password, bool isActive = true)
    {
        return new User
        {
            Username = username,
            Password = password,
            IsActive = isActive
        };
    }
}