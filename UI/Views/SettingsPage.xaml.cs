using UI.ViewModels;

namespace UI.Views;

public partial class SettingsPage
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}