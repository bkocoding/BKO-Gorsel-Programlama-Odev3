using UI.Views;

namespace UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(UI.Views.TodoDetailPage), typeof(UI.Views.TodoDetailPage));
        Routing.RegisterRoute(nameof(UI.Views.NewsDetailPage), typeof(UI.Views.NewsDetailPage));
    }
}