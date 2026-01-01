using System.Text.Json;
using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;

namespace Infrastructure.Services;

public class CurrencyApiService : ICurrencyService
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<Currency>> GetCurrenciesAsync()
    {
        var currencies = new List<Currency>();

        try
        {
            var jsonString = await _httpClient.GetStringAsync(ApiUrls.CurrencyApiUrl);
            using var doc = JsonDocument.Parse(jsonString);

            // güncellenme tarihi dışındaki tüm döviz ve altın birimlerini dolaş
            foreach (var element in doc.RootElement.EnumerateObject().Where(element => element.Name != "Update_Date"))
            {
                try
                {
                    var root = element.Value;

                    // alış ve satış değerlerini json içinden güvenli şekilde çek
                    var buying = GetProperty(root, "Alış");
                    var selling = GetProperty(root, "Satış");
                    var change = GetProperty(root, "Değişim");

                    // veri boş gelirse bu birimi listeye ekleme
                    if (string.IsNullOrEmpty(buying)) continue;

                    var currency = new Currency
                    {
                        // birim kodunu kullanıcı dostu isme dönüştür (usd -> abd doları gibi)
                        Name = ConvertCodeToName(element.Name),
                        
                        Buying = buying,
                        Selling = selling,
                        
                        // api verisi hazır formatlı geldiği için doğrudan ata
                        ChangeRate = change
                    };

                    currencies.Add(currency);
                }
                catch 
                {
                    // hatalı veya eksik verili öğeleri atlayarak devam et
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("CurrencyApiService Hatası: " + ex.Message);
        }

        return currencies;
    }
    
    private static string GetProperty(JsonElement element, string key)
    {
        if (element.TryGetProperty(key, out var value))
        {
            return value.GetString() ?? "0";
        }
        return "0";
    }
    
    private static string ConvertCodeToName(string code)
    {
        return code switch
        {
            "USD" => "ABD Doları",
            "EUR" => "Euro",
            "GBP" => "İngiliz Sterlini",
            "CHF" => "İsviçre Frangı",
            "CAD" => "Kanada Doları",
            "RUB" => "Rus Rublesi",
            "AED" => "BAE Dirhemi",
            "CNY" => "Çin Yuanı",
            
            "gram-altin" => "Gram Altın",
            "ceyrek-altin" => "Çeyrek Altın",
            "yarim-altin" => "Yarım Altın",
            "tam-altin" => "Tam Altın",
            "cumhuriyet-altini" => "Cumhuriyet Altını",
            "ata-altin" => "Ata Altın",
            "ons" => "Ons Altın",
            "resat-altin" => "Reşat Altın",
            "22-ayar-bilezik" => "22 Ayar Bilezik",
            
            "gumus" => "Gümüş",
            
            "BTC" => "Bitcoin",
            "ETH" => "Ethereum",
            
            _ => code.ToUpper()
        };
    }
}