using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Application.Interfaces;
using Domain.Entities;

namespace UI.ViewModels;

public partial class CurrencyViewModel : BaseViewModel
{
    private readonly ICurrencyService _currencyService;

    public CurrencyViewModel(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
        Title = "Piyasalar";
        
        // uygulama açıldığında verileri arka planda otomatik olarak getirir
        Task.Run(async () => await LoadCurrencies());
    }
    
    // hata durumu kontrolü
    [ObservableProperty]
    private bool _hasError;

    // kullanıcıya gösterilecek hata mesajı
    [ObservableProperty]
    private string? _errorMessage;
    
    // ekranda listelenecek döviz verileri
    [ObservableProperty]
    private ObservableCollection<Currency> _currencies = [];

    // yenileme işlemi kontrolü
    [ObservableProperty]
    private bool _isRefreshing;
    
    // son başarılı güncelleme zamanı metni
    [ObservableProperty]
    private string _lastUpdatedText = "Yükleniyor...";

    // döviz verilerini servisten çeken ana komut
    [RelayCommand]
    private async Task LoadCurrencies()
    {
        // eğer halihazırda bir işlem yapılıyorsa tekrar başlatmaz
        if (IsBusy) return;

        try
        {
            // işlem durumunu ve hata kontrolünü başlatır
            if (!IsRefreshing) IsBusy = true;
            HasError = false;

            // cihazın internet bağlantısını kontrol eder
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("İnternet bağlantısı yok.");
            }
            
            // güncel verileri servisten talep eder
            var data = await _currencyService.GetCurrenciesAsync();
            
            // gelen verinin boş olup olmadığını kontrol eder
            if (data == null || data.Count == 0)
            {
                throw new Exception("Veri alınamadı.");
            }
            
            // arayüzü main thread üzerinden günceller
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Currencies = new ObservableCollection<Currency>(data);
                LastUpdatedText = $"Son Güncelleme: {DateTime.Now:HH:mm:ss}";
                HasError = false;
            });
        }
        catch (Exception ex)
        {
            // hata oluşursa hata mesajını kullanıcıya yansıtır
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ErrorMessage = ex.Message;
                HasError = true;
                Currencies.Clear();
            });
        }
        finally
        {
            // işlem bittiğinde yükleme göstergelerini kapatır
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}