using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;

namespace UI.ViewModels;

[QueryProperty(nameof(NewsItem), "NewsDetail")]
public partial class NewsDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    private NewsItem _newsItem;

    // Paylaş Butonu Komutu
    [RelayCommand]
    private async Task ShareNews()
    {
        if (NewsItem == null) return;

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Uri = NewsItem.Link,
            Title = NewsItem.Title,
            Text = $"{NewsItem.Title} - Haberin Detayı İçin Tıklayın"
        });
    }
    
    [RelayCommand]
    private async Task OpenInBrowser()
    {
        if (NewsItem?.Link != null)
        {
            await Launcher.OpenAsync(new Uri(NewsItem.Link));
        }
    }
}