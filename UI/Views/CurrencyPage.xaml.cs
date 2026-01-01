using UI.ViewModels;

namespace UI.Views;

public partial class CurrencyPage
{
    public CurrencyPage(CurrencyViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}