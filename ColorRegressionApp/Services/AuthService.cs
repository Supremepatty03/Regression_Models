using ColorRegressionApp.DTO;
using ColorRegressionApp.Models;
using ColorRegressionApp.Data;
using Microsoft.EntityFrameworkCore;


namespace ColorRegressionApp.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<string?> LoginAsync(LoginRequest request);
}


public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return false;
        }

        var exists = await _db.Users.AnyAsync(u =>
            u.Username == request.Username || u.Email == request.Email);

        if (exists)
            return false;

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
            return null;

        var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!ok)
            return null;

        return $"fake-jwt-token-for-{user.Username}";
    }
}
