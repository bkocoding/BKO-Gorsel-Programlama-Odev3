using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Application.Interfaces;

namespace UI.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Kaydol";
    }

    [ObservableProperty] private string? _displayName;
    [ObservableProperty] private string? _email;
    [ObservableProperty] private string? _password;

    [RelayCommand]
    private async Task Register()
    {
        // işlem devam ediyorsa tekrar tetiklenmesini engelle
        if (IsBusy) return;
        
        // alanların boş olup olmadığını kontrol et
        if (string.IsNullOrWhiteSpace(DisplayName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlertAsync("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
            return;
        }

        try
        {
            IsBusy = true;
            
            // kayıt servisini çağır
            var result = await _authService.RegisterAsync(Email, Password, DisplayName);
            
            if (result)
            {
                // kayıt başarılıysa önceki sayfaya dön
                await Shell.Current.DisplayAlertAsync("Başarılı", "Kayıt oluşturuldu! Giriş yapabilirsiniz.", "Tamam");
                await Shell.Current.Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            // hata durumunda kullanıcıyı bilgilendir
            await Shell.Current.DisplayAlertAsync("Hata", $"Kayıt başarısız: {ex.Message}", "Tamam");
        }
        finally
        {
            // işlem bittiğinde meşgul durumunu kapat
            IsBusy = false;
        }
    }

    // giriş sayfasına geri yönlendiren komut
    [RelayCommand]
    async Task GoToLogin()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}