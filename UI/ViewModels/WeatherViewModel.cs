using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;

namespace UI.ViewModels;

public partial class WeatherViewModel : BaseViewModel
{
    private readonly IWeatherService _weatherService;
    private readonly List<string> _allCitiesSource;

    public WeatherViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;
        Title = "Hava Durumu";
        // tüm şehir listesini başlangıçta servis üzerinden çekiyoruz
        _allCitiesSource = _weatherService.GetAllCityNames();

        // verileri arka planda yüklemeye başla
        Task.Run(LoadData);
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public ObservableCollection<WeatherCity> SavedCities { get; } = [];

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public ObservableCollection<string> SearchResults { get; } = [];

    [ObservableProperty] private string? _searchText;

    [ObservableProperty] private bool _isSearchResultsVisible;

    [ObservableProperty] private bool _hasError;

    [ObservableProperty] private string? _errorMessage;

    [RelayCommand]
    private async Task Retry()
    {
        // hata durumunda veriyi tekrar yüklemeyi dener
        await LoadData();
    }

    partial void OnSearchTextChanged(string? value)
    {
        // arama metni boşsa sonuçları temizle ve görünümü gizle
        if (string.IsNullOrWhiteSpace(value))
        {
            SearchResults.Clear();
            IsSearchResultsVisible = false;
            return;
        }

        // büyük-küçük harf duyarlılığı için türkçe karakterleri dönüştürerek anahtar oluştur
        var searchKey = ToEnglishChars(value);

        // şehir listesinde ara ve ilk 5 sonucu getir
        var filtered = _allCitiesSource
            .Where(city => ToEnglishChars(city).Contains(searchKey))
            .Take(5)
            .ToList();

        SearchResults.Clear();
        foreach (var city in filtered) SearchResults.Add(city);

        // arama sonuçları varsa listeyi kullanıcıya göster
        IsSearchResultsVisible = SearchResults.Count > 0;
    }

    [RelayCommand]
    private async Task AddCity(string cityName)
    {
        // şehir zaten ekliyse işlemi iptal et
        if (SavedCities.Any(x => x.Name == cityName))
        {
            SearchText = string.Empty;
            SearchResults.Clear();
            IsSearchResultsVisible = false;
            return;
        }

        var isDark = IsDark();

        var newCity = new WeatherCity
        {
            Name = cityName,
            IsDarkTheme = isDark
        };

        // şehri hem servise hem de arayüzdeki listeye ekle
        await _weatherService.AddCityAsync(newCity);
        SavedCities.Add(newCity);

        // ekleme sonrası arama alanını temizle
        SearchText = string.Empty;
        SearchResults.Clear();
        IsSearchResultsVisible = false;
    }

    [RelayCommand]
    private async Task RemoveCity(WeatherCity city)
    {
        // silme işlemi onayı
        var answer = await Shell.Current.DisplayAlertAsync("Sil", $"{city.Name} silinsin mi?", "Evet", "Hayır");
        if (answer)
        {
            await _weatherService.RemoveCityAsync(city.Name);
            SavedCities.Remove(city);
        }
    }

    private async Task LoadData()
    {
        // zaten yükleme yapılıyorsa çık
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            HasError = false;
            // internet bağlantı kontrolü ile uyarı mekanizması
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("İnternet bağlantısı yok.");
            }

            var cities = await _weatherService.GetSavedCitiesAsync();
            
        // uygulamanın o anki tema modunu kontrol eder ve ona göre mgm'den getirilecek resimleri renklendirir
            var isDark = IsDark();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                SavedCities.Clear();
                foreach (var c in cities)
                {
                    c.IsDarkTheme = isDark;
                    SavedCities.Add(c);
                }
            });
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ErrorMessage = ex.Message;
                HasError = true;
                SavedCities.Clear();
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static bool IsDark()
    {
        // uygulamanın o anki tema modunu kontrol eder ve ona göre mgm'den getirilecek resimleri renklendirir
        var isDark = Microsoft.Maui.Controls.Application.Current?.RequestedTheme == AppTheme.Dark;
        return isDark;
    }

    private static string ToEnglishChars(string text)
    {
        // metindeki türkçe karakterleri arama kolaylığı için ingilizce karakterlere dönüştürür
        if (string.IsNullOrEmpty(text)) return "";
        return text.ToUpper(CultureInfo.CurrentCulture)
            .Replace('İ', 'I').Replace('ı', 'I').Replace('Ğ', 'G')
            .Replace('Ü', 'U').Replace('Ş', 'S').Replace('Ö', 'O').Replace('Ç', 'C');
    }
}