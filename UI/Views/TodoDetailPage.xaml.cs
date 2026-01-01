namespace UI.Views;

public partial class TodoDetailPage : ContentPage
{
    public TodoDetailPage(UI.ViewModels.TodoDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}