using UI.ViewModels;

namespace UI.Views;

public partial class HomePage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}