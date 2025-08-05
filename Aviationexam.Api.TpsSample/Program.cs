using Aviationexam.Api.Common.Helpers;
using Aviationexam.Api.TpsSample.Models;

namespace Aviationexam.Api.TpsSample;

static class Program
{
    /// The client information used to get the OAuth Access Token from the server.
    private const string ClientId = "fc1ab6dd-15e9-403f-9007-44e37d857b78";
    private const string ClientSecret = "KM3HCW3GKM3477C445XFDKM7RJD53KFACXW4M8HMGA8P8QZBYN26BLR5KR67ZNTZ";
    private const string ApiScope = "public-client";

    // Api url address
    private const string AuthUrl = "https://localhost:5102/auth/connect/token";
    private const string ApiUrl = "https://localhost:5100/api/client/";

    // this will hold the Access Token returned from the server.
    private static string? _accessToken;

    /// <summary>
    /// This method does all the work to get an Access Token and get all users and their exams.
    /// </summary>
    private static async Task<LoginCodeOutput> GenerateLoginCode(int userId)
        => await PutAsync<string?, LoginCodeOutput>(null, $"user/{userId}/generate-login-code");

    private static Task<T> GetAsync<T>(string url)
        => HttpClientHelper.GetAsync<T>(ApiUrl, url, _accessToken!);

    private static Task<T> PutAsync<TData, T>(TData data, string url)
        => HttpClientHelper.PutAsync<TData, T>(data, ApiUrl, url, _accessToken!);    

    static async Task Main()
    {
        Console.WriteLine("Started");

        _accessToken = await HttpClientHelper.GetAccessTokenAsync(AuthUrl, ClientId, ClientSecret, ApiScope);

        Console.WriteLine("Access token obtained");
        
        var loginCode = await GenerateLoginCode(32);
        
        Console.WriteLine($"Login code: {loginCode.LoginCode}");
        
        Console.WriteLine("Done");
        Console.ReadLine();
    }
}