using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Application.Interfaces;
using Domain.Entities;
using UI.Views;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace UI.ViewModels;

public partial class TodoViewModel : BaseViewModel
{
    private readonly ITodoService _todoService;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public TodoViewModel(ITodoService todoService)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        _todoService = todoService;
        Title = "Yapılacaklar";
    }

    [ObservableProperty]
    private ObservableCollection<TodoTask> _tasks = [];
    
    [ObservableProperty]
    private bool _isRefreshing;
    
    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage;
    
    public async Task OnAppearing()
    {
        await LoadTasks();
    }

    [RelayCommand]
    private async Task LoadTasks()
    {
        // Eğer işlem zaten sürüyorsa VE bu bir yenileme işlemi değilse çık
        // (Yenileme işlemi ise devam etmesine izin veriyoruz, kilitlenmeyi önler)
        if (IsBusy && !IsRefreshing) return;

        try
        {
            HasError = false;
            ErrorMessage = string.Empty;
            
            if (!IsRefreshing)
            {
                IsBusy = true;
            }
            
            // internet bağlantısını kontrol eder ve yoksa hata fırlatır
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("İnternet bağlantısı yok. Lütfen bağlantınızı kontrol edin.");
            }
            
            var taskList = await _todoService.GetAllTasksAsync();
            
            // listeyi temizler ve güncel verileri ekler
            Tasks.Clear();
            foreach (var task in taskList)
            {
                Tasks.Add(task);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
    
    // yeni görev ekleme sayfasına yönlendirir
    [RelayCommand]
    private async Task GoToAddPage()
    {
        await Shell.Current.GoToAsync(nameof(TodoDetailPage));
    }
    
    [RelayCommand]
    private async Task GoToEditPage(TodoTask task)
    {
        if (task == null) return;

        // seçilen görev bilgisini parametre olarak detay sayfasına taşır
        var navParam = new Dictionary<string, object>
        {
            { "TaskObject", task }
        };

        await Shell.Current.GoToAsync(nameof(TodoDetailPage), navParam);
    }
    
    [RelayCommand]
    private async Task DeleteTask(TodoTask task)
    {
        if (task == null) return;

        var answer = await Shell.Current.DisplayAlertAsync("Silinsin mi?", 
            $"'{task.Title}' görevini silmeyi onaylıyor musunuz?", 
            "Evet", "Hayır");

        if (answer)
        {
            try 
            {
                await _todoService.DeleteTaskAsync(task.Id);
                Tasks.Remove(task); // Listeden de düşür
            }
            catch(Exception ex)
            {
                // kullanıcıdan silme işlemi için onay alır
                await Shell.Current.DisplayAlertAsync("Hata", "Silinemedi: " + ex.Message, "Tamam");
            }
        }
    }
    
    [RelayCommand]
    private async Task ToggleComplete(TodoTask task)
    {
        if (task == null) return;
        
        try
        {
            await _todoService.UpdateTaskAsync(task);
        }
        catch(Exception)
        {
            //TODO: güncelleyemezsek alert mi açalım yoksa xamldaki hata kısmımızı mı açalım, karar veremedim. Bunun hakkında bir şeyler yap.
        }
    }
}