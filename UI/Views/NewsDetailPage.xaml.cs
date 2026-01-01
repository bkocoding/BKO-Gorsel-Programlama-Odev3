using UI.ViewModels;

namespace UI.Views;

public partial class NewsDetailPage : ContentPage
{
    public NewsDetailPage(NewsDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}