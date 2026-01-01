using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

public class WeatherCity
{
    public required string Name { get; init; }
    
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public bool IsDarkTheme { get; set; } = false; 

    public string WidgetUrl 
    {
        get
        {
            
            var background = IsDarkTheme ? "1E1E1E" : "ffffff";
            var border = IsDarkTheme ? "333333" : "e0e0e0";

            return $"https://www.mgm.gov.tr/sunum/tahmin-klasik-5070.aspx?m={Name}&basla=1&bitir=5&rC={border}&rZ={background}";
        }
    }
}