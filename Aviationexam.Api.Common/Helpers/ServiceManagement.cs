namespace Aviationexam.Api.Common.Helpers;

public class ServiceManagement(string clientId, string clientSecret, string apiScope, string authUrl, string apiUrl)
{
    // this will hold the Access Token returned from the server.
    private static string? _accessToken;
    
    private HttpClient _client = null!;
    
    public async Task<string?> GetAccessTokenAsync()
    {
        _accessToken = await HttpClientHelper.GetAccessTokenAsync(authUrl, clientId, clientSecret, apiScope);

        _client = HttpClientHelper.GetClient(apiUrl, _accessToken);       
        
        return _accessToken;
    }    
    
    public Task<TResult> GetAsync<TResult>(string url) => 
        _client.GetAsync<TResult>(url);

    public Task<TResult> PutAsync<TData, TResult>(TData data, string url) =>
        _client.PutAsync<TData, TResult>(data, url);

    public Task<TResult> PostAsync<TData, TResult>(TData data, string url) =>
        _client.PostAsync<TData, TResult>(data, url);
    
    public Task<IReadOnlyCollection<TResult>> GetContinuationRequestAsync<TResult>(string queryUrl) =>
        _client.GetContinuationRequestAsync<TResult>(queryUrl); 
}