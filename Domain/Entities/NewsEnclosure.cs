using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.Entities;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class NewsEnclosure
{
    [JsonPropertyName("link")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public required string Link { get; set; }
}