using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Application.Interfaces;

namespace UI.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public HomeViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Ana Sayfa";
        
        StudentName = "Burak Kaan ORAK"; 
        StudentNumber = "21010310020";
        Department = "Bartın Üniversitesi\nBilgisayar Mühendisliği";
        ProfileImage = "student_photo.png";
    }
    
    [ObservableProperty] private string _studentName;
    [ObservableProperty] private string _studentNumber;
    [ObservableProperty] private string _department;
    [ObservableProperty] private string _profileImage;
    
    [RelayCommand]
    private async Task Logout()
    {
        // kullanıcıdan çıkış işlemi için onay alınması
        var answer = await Shell.Current.DisplayAlertAsync("Çıkış", "Oturumu kapatmak istiyor musunuz?", "Evet", "Hayır");
        if (answer)
        {
            // servis üzerinden oturumu sonlandırıp giriş ekranına yönlendirir
            _authService.SignOut();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}