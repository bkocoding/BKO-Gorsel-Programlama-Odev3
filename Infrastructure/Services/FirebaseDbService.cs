using Firebase.Database;
using Firebase.Database.Query;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Config;

namespace Infrastructure.Services;

public class FirebaseDbService : ITodoService
{
    private readonly FirebaseClient _firebaseClient;
    private readonly IAuthService _authService;

    public FirebaseDbService(IAuthService authService)
    {
        _authService = authService;
        
        var configData = AppConfigService.GetConfig();
        // veritabanı url'sinin sonuna gerekirse bölü işareti ekleyerek düzeltir
        var url = configData.DatabaseUrl.EndsWith('/') ? configData.DatabaseUrl : configData.DatabaseUrl + "/";
        
        // firebase istemcisi için kimlik doğrulama ayarlarını yapılandırır
        var options = new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => _authService.GetUserTokenAsync()
        };
        
        _firebaseClient = new FirebaseClient(url, options);
    }
    
    // o an giriş yapmış olan kullanıcının benzersiz kimliğini döndürür
    private string CurrentUserId => _authService.GetCurrentUserId() ?? string.Empty;

    public async Task<List<TodoTask>> GetAllTasksAsync()
    {
        // kullanıcı oturumu kapalıysa boş bir liste döndürür
        if (string.IsNullOrEmpty(CurrentUserId)) return [];

        // firebase üzerindeki kullanıcıya özel tüm görevleri çeker
        var tasks = await _firebaseClient
            .Child("Users")
            .Child(CurrentUserId)
            .Child("Todos")
            .OnceAsync<TodoTask>();
        
        return tasks.Select(x => x.Object).ToList();
    }

    public async Task AddTaskAsync(TodoTask task)
    {
        if (string.IsNullOrEmpty(CurrentUserId)) return;

        // yeni bir görevi kullanıcının görev listesine id değeriyle kaydeder
        await _firebaseClient
            .Child("Users")
            .Child(CurrentUserId)
            .Child("Todos")
            .Child(task.Id)
            .PutAsync(task);
    }

    public async Task UpdateTaskAsync(TodoTask task)
    {
        if (string.IsNullOrEmpty(CurrentUserId)) return;

        // mevcut bir görevin içeriğini id üzerinden günceller
        await _firebaseClient
            .Child("Users")
            .Child(CurrentUserId)
            .Child("Todos")
            .Child(task.Id)
            .PutAsync(task);
    }

    public async Task DeleteTaskAsync(string taskId)
    {
        if (string.IsNullOrEmpty(CurrentUserId)) return;

        // belirtilen id'ye sahip görevi veritabanından siler
        await _firebaseClient
            .Child("Users")
            .Child(CurrentUserId)
            .Child("Todos")
            .Child(taskId)
            .DeleteAsync();
    }
}