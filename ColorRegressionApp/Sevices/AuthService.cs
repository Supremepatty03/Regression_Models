using ColorRegressionApp.DTO;
using ColorRegressionApp.Models;
namespace ColorRegressionApp.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<string?> LoginAsync(LoginRequest request);
    }
}

public class AuthService : IAuthService
{
    private readonly List<User> _users = new List<User>();

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (_users.Any(u => u.Username == request.Username))
            return false;

        var user = new User
        {
            Id = _users.Count + 1,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Email = request.Email
        };

        _users.Add(user);
        return true;
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        // In a real application, generate a JWT or similar token here
        return $"fake-jwt-token-for-{user.Username}";
    }
}