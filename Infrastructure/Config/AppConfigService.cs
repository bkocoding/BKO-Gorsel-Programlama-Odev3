using System.Reflection;
using System.Text.Json;
using Domain.Entities;

namespace Infrastructure.Config;

public abstract class AppConfigService
{
    private static FirebaseConfig? _config;
    
    public static FirebaseConfig GetConfig()
    {
        // eğer yapılandırma daha önce yüklendiyse tekrar yükleme yapmadan döndürür
        if (_config != null) return _config;

        // çalışan mevcut assembly bilgisini alır
        var assembly = Assembly.GetExecutingAssembly();
        // gömülü kaynak olarak saklanan json dosyasının tam yolu
        const string resourceName = "Infrastructure.Properties.firebase_config.json";

        // belirtilen kaynaktan veri akışını açar
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            // dosya bulunamazsa hata fırlatır
            if (stream == null)
            {
                throw new FileNotFoundException($"Config dosyası bulunamadı: {resourceName}.");
            }

            // akış içindeki veriyi okumak için okuyucu nesnesi oluşturur
            using (var reader = new StreamReader(stream))
            {
                // dosya içeriğini metin olarak okur
                var json = reader.ReadToEnd();
                // okunan metni firebaseconfig nesnesine dönüştürür
                _config = JsonSerializer.Deserialize<FirebaseConfig>(json) ?? throw new InvalidOperationException();
            }
        }
        
        // yüklenen yapılandırmayı döndürür
        return _config;
    }
}