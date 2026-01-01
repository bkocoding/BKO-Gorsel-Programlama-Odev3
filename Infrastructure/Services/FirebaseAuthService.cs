using Application.Interfaces;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Infrastructure.Config;

namespace Infrastructure.Services;

public class FirebaseAuthService : IAuthService
{
    private readonly FirebaseAuthClient _authClient;
    
    private string? _currentToken;
    private string? _currentUserId;

    public FirebaseAuthService()
    {
        // uygulama yapılandırmasından ayarları çekip firebase istemcisini başlatır
        var configData = AppConfigService.GetConfig();
        var config = new FirebaseAuthConfig
        {
            ApiKey = configData.ApiKey,
            AuthDomain = configData.AuthDomain,
            Providers =
            [
                new EmailProvider()
            ]
        };

        _authClient = new FirebaseAuthClient(config);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        // e-posta ve şifre ile giriş yapıp oturum bilgilerini günceller
        var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
        var user = userCredential.User;
        
        _currentToken = await user.GetIdTokenAsync();
        _currentUserId = user.Uid; 
        
        return _currentToken;
    }

    public async Task<bool> RegisterAsync(string email, string password, string displayName)
    {
        // yeni bir kullanıcı kaydı oluşturur
        var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);

        return userCredential != null;
    }

    public string? GetCurrentUserId()
    {
        // aktif olan kullanıcının benzersiz kimliğini döndürür
        return _currentUserId;
    }
    
    public Task<string> GetUserTokenAsync()
    {
        // mevcut oturum anahtarını döndürür, yoksa boş metin döner
        return Task.FromResult(_currentToken ?? string.Empty);
    }
    
    public void SignOut()
    {
        // oturumu kapatır ve yerelde tutulan kullanıcı verilerini temizler
        _authClient.SignOut();
        _currentToken = null;
        _currentUserId = null;
    }
}