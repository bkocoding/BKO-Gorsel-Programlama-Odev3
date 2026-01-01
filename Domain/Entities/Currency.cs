using System.Text.Json.Serialization;
namespace Domain.Entities;

public class Currency
{

    [JsonPropertyName("name")] 
    public required string Name { get; set; }

    [JsonPropertyName("buying")]
    public required string Buying { get; set; }

    [JsonPropertyName("selling")]
    public required string Selling { get; set; }

    [JsonPropertyName("change_rate")]
    public required string ChangeRate { get; set; }
    
    public string DirectionSymbol => ChangeRate.StartsWith("%-") ? "↓" : "↑";
}