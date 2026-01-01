using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Application.Interfaces;
using UI.Views;

namespace UI.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Giriş Yap";
    }
    
    [ObservableProperty] private string? _email;
    [ObservableProperty] private string? _password;

    [RelayCommand]
    private async Task Login()
    {
        // işlem devam ediyorsa yeni istek başlatma
        if (IsBusy) return;
        
        // giriş bilgilerinin eksik olup olmadığını kontrol et
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlertAsync("Uyarı", "Email ve şifre gereklidir.", "Tamam");
            return;
        }

        try
        {
            // işlem sürecini başlat
            IsBusy = true;
            
            // giriş yapmayı dene ve başarılıysa jeton al
            var token = await _authService.LoginAsync(Email, Password);

            if (!string.IsNullOrEmpty(token))
            {
                // ana sayfaya yönlendir
                await Shell.Current.GoToAsync("//HomePage"); 
            }
        }
        catch (Exception ex)
        {
            // hata durumunda kullanıcıyı bilgilendir
            await Shell.Current.DisplayAlertAsync("Giriş Hatası", "Email veya şifre hatalı.\nDetay: " + ex.Message, "Tamam");
        }
        finally
        {
            // işlem durumunu sıfırla
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToRegister()
    {
        // kayıt ol sayfasına yönlendir
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}