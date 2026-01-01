using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    public SettingsViewModel()
    {
        Title = "Ayarlar";
        
        var app = Microsoft.Maui.Controls.Application.Current;
        if (app == null) return;

        // uygulama temasını kontrol edip sistem ayarına veya kullanıcı seçimine göre aktif temayı buluyoruz
        var effectiveTheme = app.UserAppTheme == AppTheme.Unspecified 
            ? app.RequestedTheme 
            : app.UserAppTheme;
            
        // eğer aktif tema koyu mod ise anahtarımızı açık konuma getiriyoruz
        _isDarkMode = effectiveTheme == AppTheme.Dark;
    }

    [ObservableProperty]
    private bool _isDarkMode;

    partial void OnIsDarkModeChanged(bool value)
    {
        var app = Microsoft.Maui.Controls.Application.Current;
        if (app == null) return;

        // kullanıcı anahtarı değiştirdiğinde uygulama temasını anlık olarak güncelliyoruz
        app.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
            
        // seçilen tema tercihini kalıcı olarak cihaza kaydediyoruz
        Preferences.Set("AppTheme", value ? "Dark" : "Light");
    }
}