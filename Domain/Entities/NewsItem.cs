using System.Text.Json.Serialization;
// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Domain.Entities;

public class NewsItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("pubDate")]
    public string PubDate { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("enclosure")]
    public NewsEnclosure Enclosure { get; set; }
    
    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; }
    
    public string MainImageUrl => !string.IsNullOrEmpty(Enclosure?.Link) ? Enclosure.Link : Thumbnail;
}