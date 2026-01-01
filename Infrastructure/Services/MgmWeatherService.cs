using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services;

public class MgmWeatherService : IWeatherService
{
    private readonly string _filePath;

    public MgmWeatherService()
    {
        // uygulama verilerinin saklanacağı yerel klasör yolunu belirler
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _filePath = Path.Combine(folder, "mgm_cities.json");
    }

    public async Task<List<WeatherCity>> GetSavedCitiesAsync()
    {
        // dosya mevcut değilse boş bir liste döndürür
        if (!File.Exists(_filePath))
            return [];

        try
        {
            // kayıtlı şehirleri dosyadan okuyup listeye dönüştürür
            var json = await File.ReadAllTextAsync(_filePath);
            var list = JsonSerializer.Deserialize<List<WeatherCity>>(json);
            return list ?? [];
        }
        catch
        {
            // okuma sırasında hata oluşursa güvenli şekilde boş liste döner
            return [];
        }
    }

    public async Task AddCityAsync(WeatherCity city)
    {
        var cities = await GetSavedCitiesAsync();

        // aynı isimde şehir varsa kaydetmez
        if (cities.Any(c => c.Name == city.Name))
            return;

        cities.Add(city);
        await SaveToFile(cities);
    }

    public async Task RemoveCityAsync(string cityName)
    {
        var cities = await GetSavedCitiesAsync();
        var item = cities.FirstOrDefault(c => c.Name == cityName);

        // silinmek istenen şehir listede varsa kaldırır ve dosyayı günceller
        if (item != null)
        {
            cities.Remove(item);
            await SaveToFile(cities);
        }
    }

    public List<string> GetAllCityNames()
    {
        return
        [
            "ADANA", "ADIYAMAN", "AFYONKARAHISAR", "AGRI", "AKSARAY", "AMASYA", "ANKARA", "ANTALYA", "ARDAHAN",
            "ARTVIN", "AYDIN",
            "BALIKESIR", "BARTIN", "BATMAN", "BAYBURT", "BILECIK", "BINGOL", "BITLIS", "BOLU", "BURDUR", "BURSA",
            "CANAKKALE",
            "CANKIRI", "CORUM", "DENIZLI", "DIYARBAKIR", "DUZCE", "EDIRNE", "ELAZIG", "ERZINCAN", "ERZURUM",
            "ESKISEHIR",
            "GAZIANTEP", "GIRESUN", "GUMUSHANE", "HAKKARI", "HATAY", "IGDIR", "ISPARTA", "ISTANBUL", "IZMIR",
            "K.MARAS",
            "KARABUK", "KARAMAN", "KARS", "KASTAMONU", "KAYSERI", "KILIS", "KIRIKKALE", "KIRKLARELI", "KIRSEHIR",
            "KOCAELI",
            "KONYA", "KUTAHYA", "MALATYA", "MANISA", "MARDIN", "MERSIN", "MUGLA", "MUS", "NEVSEHIR", "NIGDE", "ORDU",
            "OSMANIYE",
            "RIZE", "SAKARYA", "SAMSUN", "SANLIURFA", "SIIRT", "SINOP", "SIRNAK", "SIVAS", "TEKIRDAG", "TOKAT",
            "TRABZON",
            "TUNCELI", "USAK", "VAN", "YALOVA", "YOZGAT", "ZONGULDAK"
        ];
    }
    
    // dosyaya kaydetme yardımcı fonksiyonu
    private async Task SaveToFile(List<WeatherCity> cities)
    {
        try
        {
            var json = JsonSerializer.Serialize(cities);
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Dosya Yazma Hatası: {ex.Message}");
        }
    }
}