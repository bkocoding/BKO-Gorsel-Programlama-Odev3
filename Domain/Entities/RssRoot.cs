using System.Text.Json.Serialization;

namespace Domain.Entities;

public class RssRoot
{
    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("items")]
    public required List<NewsItem> Items { get; init; }
}