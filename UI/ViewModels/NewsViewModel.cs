using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Constants;
using Domain.Entities;
using Application.Interfaces;
using UI.Views;

namespace UI.ViewModels;

public partial class NewsViewModel : BaseViewModel
{
    private readonly INewsService _newsService;

    public NewsViewModel(INewsService newsService)
    {
        _newsService = newsService;
        Title = "Haberler";

        Categories =
        [
            new NewsUiCategory("Manşet", ApiUrls.TrtManset),
            new NewsUiCategory("Gündem", ApiUrls.TrtGundem),
            new NewsUiCategory("Ekonomi", ApiUrls.TrtEkonomi),
            new NewsUiCategory("Spor", ApiUrls.TrtSpor),
            new NewsUiCategory("Bilim", ApiUrls.TrtBilim)
        ];
        // uygulama açıldığında ilk kategoriyi seçili hale getirir
        SelectedCategory = Categories.First();
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public ObservableCollection<NewsUiCategory> Categories { get; }

    [ObservableProperty]
    private ObservableCollection<NewsItem> _newsList = [];
    
    [ObservableProperty]
    private NewsUiCategory _selectedCategory;
    
    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string? _errorMessage;
    
    // seçili kategori değiştiğinde haberleri otomatik olarak yeniden yükler
    partial void OnSelectedCategoryChanged(NewsUiCategory value)
    {
            LoadNewsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadNews()
    {
        // halihazırda bir yükleme işlemi varsa tekrar başlatmaz
        if (IsBusy && !IsRefreshing) return;

        try
        {
            HasError = false;
            ErrorMessage = string.Empty;
            
            if (!IsRefreshing) IsBusy = true;
            
            // internet bağlantısını kontrol eder
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("İnternet bağlantısı yok. Lütfen kontrol edin.");
            }
            
            // seçili kategoriye ait haberleri servisten çeker
            var items = await _newsService.GetNewsAsync(SelectedCategory.RssUrl);

            if (items.Count == 0)
            {
                throw new Exception("Haberler alınamadı.");
            }
            
            // UI ana threadde olduğundan invoke ederek güncelliyoruz.
            MainThread.BeginInvokeOnMainThread(() =>
            {
                NewsList.Clear();
                foreach (var item in items)
                {
                    NewsList.Add(item);
                }
            });
        }
        catch (Exception ex)
        {
            // hata oluşursa kullanıcıya göstermek üzere bilgileri ayarlar
            MainThread.BeginInvokeOnMainThread(() =>
            {
                HasError = true;
                ErrorMessage = ex.Message;
                NewsList.Clear();
            });
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task GoToDetail(NewsItem? item)
    {
        if (item == null) return;
        
        // seçilen haberi parametre olarak detay sayfasına gönderir
        var param = new Dictionary<string, object> { { "NewsDetail", item } };
        await Shell.Current.GoToAsync(nameof(NewsDetailPage), param);
    }
}