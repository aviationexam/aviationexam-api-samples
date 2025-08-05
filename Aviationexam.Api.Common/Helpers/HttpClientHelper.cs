using Aviationexam.Api.Common.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Aviationexam.Api.Common.Helpers;

public static class HttpClientHelper
{
    /// <summary>
    /// This method uses the OAuth Client Credentials Flow to get an Access Token to provide
    /// Authorization to the APIs.
    /// </summary>    
    public static async Task<string?> GetAccessTokenAsync(string authUrl, string clientId, string clientSecret, string scope)
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

    private static HttpClient GetClient(string baseUrl, string? accessToken = null)
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
    
    public static async Task<IReadOnlyCollection<T>> GetContinuationRequestAsync<T>(string apiUrl, string queryUrl, string accessToken)
    {
        var list = new List<T>();

        using var client = GetClient(apiUrl, accessToken);
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

    public static async Task<T> GetAsync<T>(string apiUrl, string queryUrl, string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("Get access token at first");
        }
        
        using var client = GetClient(apiUrl, accessToken);

        // make the request
        var response = await client.GetAsync(queryUrl);
        string jsonString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(jsonString))
        {
            var responseData = JsonSerializer.Deserialize<T>(jsonString);

            return responseData!;
        }

        throw new Exception($"Error getting data. StatusCode {response.StatusCode}");       
    }
    
    private static HttpContent CreateEmptyContent() => new StringContent(string.Empty);
    
    private static HttpContent CreateContent<TContract>(
        TContract data
    ) => data == null
        ? CreateEmptyContent()
        : new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");    
    
    public static async Task<TResult> PutAsync<TData, TResult>(TData data, string apiUrl, string queryUrl, string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("Get access token at first");
        }
        
        using var client = GetClient(apiUrl, accessToken);

        using var content = CreateContent(data);        
        
        // make the request
        var response = await client.PutAsync(queryUrl, content);
        string jsonString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(jsonString))
        {
            var responseData = JsonSerializer.Deserialize<TResult>(jsonString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            return responseData!;
        }

        throw new Exception($"Error getting data. StatusCode {response.StatusCode}");       
    }    
}