using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Application.Interfaces;
using Domain.Entities;

namespace UI.ViewModels;

[QueryProperty(nameof(TaskItem), "TaskObject")]
public partial class TodoDetailViewModel : BaseViewModel
{
    private readonly ITodoService _todoService;
    private TodoTask _existingTask; // düzenleme modunda mevcut görevi tutması için var

    public TodoDetailViewModel(ITodoService todoService)
    {
        _todoService = todoService;
        Title = "Görev Ekle";
        
        // varsayılan olarak bugünün tarihini ve saatini atar
        TaskDate = DateTime.Now;
        TaskTime = DateTime.Now.TimeOfDay;
    }
    
    public TodoTask TaskItem
    {
        set
        {
            _existingTask = value;
            if (_existingTask != null)
            {
                // gelen görev doluysa alanları düzenleme moduna göre doldurur
                Title = "Görevi Düzenle";
                TaskTitle = _existingTask.Title;
                TaskDetail = _existingTask.Detail;
                TaskDate = _existingTask.Date;
                TaskTime = _existingTask.Time;
                IsCompleted = _existingTask.IsCompleted;
            }
        }
    }

    [ObservableProperty]
    private string _taskTitle;

    [ObservableProperty]
    private string _taskDetail;

    [ObservableProperty]
    private DateTime _taskDate;

    [ObservableProperty]
    private TimeSpan _taskTime;

    [ObservableProperty]
    private bool _isCompleted;

    [RelayCommand]
    private async Task SaveTask()
    {
        if (string.IsNullOrWhiteSpace(TaskTitle))
        {
            await Shell.Current.DisplayAlertAsync("Uyarı", "Başlık boş olamaz.", "Tamam");
            return;
        }

        IsBusy = true; // işlem başladığında meşgul durumunu aktif eder

        try
        {
            if (_existingTask == null)
            {
                // yeni bir görev nesnesi oluşturur ve servise ekler
                var newTask = new TodoTask
                {
                    Title = TaskTitle,
                    Detail = TaskDetail ?? "",
                    Date = TaskDate,
                    Time = TaskTime,
                    IsCompleted = false
                };
                await _todoService.AddTaskAsync(newTask);
            }
            else
            {
                // mevcut görevi güncellenen bilgilerle yeniler
                _existingTask.Title = TaskTitle;
                _existingTask.Detail = TaskDetail ?? "";
                _existingTask.Date = TaskDate;
                _existingTask.Time = TaskTime;
                
                await _todoService.UpdateTaskAsync(_existingTask);
            }
            
            // işlem başarılıysa bir önceki sayfaya döner
            await Shell.Current.GoToAsync(".."); 
        }
        catch (Exception ex)
        {
            // hata durumunda kullanıcıya bilgilendirme mesajı gösterir
            await Shell.Current.DisplayAlertAsync("Hata", "Kaydedilemedi: " + ex.Message, "Tamam");
        }
        finally
        {
            // işlem bittiğinde meşgul durumunu kapatır
            IsBusy = false;
        }
    }
}