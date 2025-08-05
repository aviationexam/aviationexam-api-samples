using System.Text.Json.Serialization;

namespace Aviationexam.Api.Common.Models;

public class AccessToken
{
    [JsonPropertyName("access_token")]
    public required string? Token { get; set; }
    
    [JsonPropertyName("token_type")]
    
    public required string TokenType { get; set; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}