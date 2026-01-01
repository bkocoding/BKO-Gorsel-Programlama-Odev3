namespace UI;

public partial class App
{
    public App()
    {
        InitializeComponent();
        
        var savedTheme = Preferences.Get("AppTheme", "System");

        UserAppTheme = savedTheme switch
        {
            "Dark" => AppTheme.Dark,
            "Light" => AppTheme.Light,
            _ => AppTheme.Unspecified
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}