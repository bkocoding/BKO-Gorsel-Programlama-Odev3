using System.Text.Json;
using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;

namespace Infrastructure.Services;

public class RssNewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RssNewsService()
    {
        _httpClient = new HttpClient();
        // bazı rss servisleri user-agent beklediği için varsayılan başlık ekleniyor...
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    }

    public async Task<List<NewsItem>> GetNewsAsync(string rssUrl)
    {
        try
        {
            // rss verisini json formatına dönüştüren servis uç noktası hazırlanıyor.
            var apiUrl = $"{ApiUrls.RssToJsonBaseUrl}{rssUrl}";

            var response = await _httpClient.GetStringAsync(apiUrl);

            var options = DefaultSerializerOptions;
            var root = JsonSerializer.Deserialize<RssRoot>(response, options);

            // yanıt durumu başarılıysa haber listesi, değilse boş liste döndürülüyor
            return root is { Status: "ok" } ? root.Items : [];
        }
        catch (Exception ex)
        {
            // hata durumunda detaylar hata ayıklama penceresine yazdırılıyor.
            System.Diagnostics.Debug.WriteLine($"Haber çekme hatası: {ex.Message}");
            return [];
        }
    }
}