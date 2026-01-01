namespace Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(string email, string password, string displayName);
    
    Task<string> LoginAsync(string email, string password);
    
    string? GetCurrentUserId();
    
    Task<string> GetUserTokenAsync();
    
    void SignOut();
}