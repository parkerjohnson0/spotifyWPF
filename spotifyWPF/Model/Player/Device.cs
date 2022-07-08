using System.Text.Json.Serialization;

namespace spotifyWPF.Model.Player;

public class Device
{
    [JsonPropertyName("is_private_session")]
    public bool IsPrivateSession { get; set; } 
    [JsonPropertyName("is_restricted")]
    public bool IsRestricted { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("id")]
    public string ID { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("volume_percent")]
    public int Volume { get; set; }
    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
}