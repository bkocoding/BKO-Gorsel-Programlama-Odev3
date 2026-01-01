using UI.ViewModels;

namespace UI.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        
        BindingContext = vm; 
    }
}