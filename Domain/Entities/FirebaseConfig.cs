using System.Text.Json.Serialization;

namespace Domain.Entities;

public class FirebaseConfig
{
    [JsonPropertyName("apiKey")]
    public required string ApiKey { get; init; }

    [JsonPropertyName("authDomain")]
    public required string AuthDomain { get; init; }

    [JsonPropertyName("databaseURL")]
    public required string DatabaseUrl { get; init; }

    [JsonPropertyName("projectId")]
    public required string ProjectId { get; init; }

    [JsonPropertyName("storageBucket")]
    public required string StorageBucket { get; init; }

    [JsonPropertyName("messagingSenderId")]
    public required string MessagingSenderId { get; init; }

    [JsonPropertyName("appId")]
    public required string AppId { get; init; }
}