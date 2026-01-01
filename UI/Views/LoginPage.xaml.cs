using UI.ViewModels;

namespace UI.Views;

public partial class LoginPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}