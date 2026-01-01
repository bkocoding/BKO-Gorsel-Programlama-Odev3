using Domain.Entities;
using UI.ViewModels;

namespace UI.Views;

public partial class TodoPage : ContentPage
{
    private readonly TodoViewModel _viewModel;

    public TodoPage(TodoViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }

    // YENİ EKLENEN KISIM: Checkbox değişince burası çalışır
    private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Olayı tetikleyen Checkbox'ı ve içindeki Görevi (TodoTask) buluyoruz
        if (sender is CheckBox checkBox && checkBox.BindingContext is TodoTask task)
        {
            // ViewModel içindeki ToggleCompleteCommand komutunu çalıştırıyoruz
            if (_viewModel.ToggleCompleteCommand.CanExecute(task))
            {
                await _viewModel.ToggleCompleteCommand.ExecuteAsync(task);
            }
        }
    }
}