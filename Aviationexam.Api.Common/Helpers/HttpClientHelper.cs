using Aviationexam.Api.Common.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aviationexam.Api.Common.Helpers;

public static class HttpClientHelper
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new BigIntJsonConverter()
        }
    };
    
    /// <summary>
    /// This method uses the OAuth Client Credentials Flow to get an Access Token to provide
    /// Authorization to the APIs.
    /// </summary>    
    internal static async Task<string?> GetAccessTokenAsync(string authUrl, string clientId, string clientSecret, string scope)
    {
        using var client = GetClient(authUrl);
        // Build up the data to POST.
        var postData = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", clientId),
            new("client_secret", clientSecret),
            new("scope", scope)
        };

        var content = new FormUrlEncodedContent(postData);

        // Post to the Server and parse the response.
        var response = await client.PostAsync("Token", content);
        string jsonString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var accessToken = JsonSerializer.Deserialize<AccessToken>(jsonString);
            return accessToken!.Token;
        }
        
        throw new Exception($"Error getting access token. StatusCode {response.StatusCode}");       
    }

    public static HttpClient GetClient(string baseUrl, string? accessToken = null)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        // Add the Authorization header with the AccessToken.
        if (!string.IsNullOrEmpty(accessToken))
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        }

        return client;
    }    
    
    public static async Task<IReadOnlyCollection<T>> GetContinuationRequestAsync<T>(this HttpClient client, string queryUrl)
    {
        var list = new List<T>();

        string? continuationToken = "";

        do
        {
            var url = string.IsNullOrEmpty(continuationToken) ? queryUrl : queryUrl + $"&continuationToken={continuationToken}";

            // make the request
            var response = await client.GetAsync(url);
            string jsonString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(jsonString))
            {
                var responseData = JsonSerializer.Deserialize<ContinuationResponse<T>>(jsonString);
                if (responseData?.Items != null)
                {
                    list.AddRange(responseData.Items);
                }

                continuationToken = responseData?.ContinuationToken;
            }
            else
            {
                break;
            }

        } while (!string.IsNullOrEmpty(continuationToken));

        // parse the response and return the data.
        return list;
    }

    public static async Task<TResult> GetAsync<TResult>(this HttpClient client, string queryUrl)
    {
        var response = await client.GetAsync(queryUrl);
        
        return await GetResponse<TResult>(response);    
    }
    
    public static async Task<TResult> PutAsync<TData, TResult>(this HttpClient client, TData data, string queryUrl)
    {
        using var content = CreateContent(data);        
        
        var response = await client.PutAsync(queryUrl, content);

        return await GetResponse<TResult>(response);       
    }

    public static async Task<TResult> PostAsync<TData, TResult>(this HttpClient client, TData data, string queryUrl)
    {
        using var content = CreateContent(data);        
        
        var response = await client.PostAsync(queryUrl, content);

        return await GetResponse<TResult>(response);       
    }
    
    private static async Task<T> GetResponse<T>(HttpResponseMessage response)
    {
        string jsonString = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(jsonString))
        {
            var responseData = JsonSerializer.Deserialize<T>(jsonString, SerializerOptions);

            return responseData!;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            
        }

        throw new Exception($"Error getting data. StatusCode {response.StatusCode}");       
    }
    
    private static HttpContent CreateEmptyContent() => new StringContent(string.Empty);
    
    private static HttpContent CreateContent<TContract>(
        TContract data
    ) => data == null
        ? CreateEmptyContent()
        : new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");    
}