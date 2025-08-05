using Aviationexam.Api.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aviationexam.LmsApiSample;

static class Program
{
    /// The client information used to get the OAuth Access Token from the server.
    private const string ClientId = "REPLACE_WITH_YOUR_CLIENT_ID";
    private const string ClientSecret = "REPLACE_WITH_YOUR_CLIENT_SECRET";
    private const string ApiScope = "public-lms";

    // Api url address
    private const string AuthUrl = "https://auth.beta.aviationexam.com/auth/connect/token";
    private const string ApiUrl = "https://api.beta.aviationexam.com/api/client/";

    // this will hold the Access Token returned from the server.
    static string _accessToken;

    // max rows per request (max 1000)
    static readonly int PageSize = 100;

    /// <summary>
    /// This method does all the work to get an Access Token and get all users and their exams.
    /// </summary>
    private static async Task<int> GetUsersAndExamsAsync()
    {
        // Next time, use the date of the previous synchronization instead of DateTime.MinValue
        var users = await GetUsersAsync(DateTime.MinValue);
        Console.WriteLine($"Users count: {users?.Count}");
            
        var exams = await GetExamsAsync(DateTime.MinValue);

        Console.WriteLine($"Exams count: {exams?.Count}");

        return 0;
    }
    
    /// <summary>
    /// Get users.
    /// </summary>
    /// <param name="lastDateUpdate">Date of last synchronization. Only users registered after this date will be returned.</param>
    /// <returns>The page of articles.</returns>
    private static async Task<IReadOnlyCollection<GetLmsStudentOutput>> GetUsersAsync(DateTime lastDateUpdate)
    {
        string url = $"lms/users?pageSize={PageSize}&lastDateUpdate={lastDateUpdate:O}";

        return await GetContinuationRequestAsync<GetLmsStudentOutput>(url);
    }

    /// <summary>
    /// Get exams.
    /// </summary>
    /// <param name="lastDateUpdate">Date of last synchronization. Only exams generated after this date will be returned.</param>
    /// <returns>The page of articles.</returns>
    private static async Task<IReadOnlyCollection<GetLmsStudentExamOutput>> GetExamsAsync(DateTime lastDateUpdate)
    {
        string url = $"lms/exams?pageSize={PageSize}&lastDateUpdate={lastDateUpdate:O}";

        return await HttpClientHelper.GetContinuationRequestAsync<GetLmsStudentExamOutput>(ApiUrl, url, _accessToken);
    }

    private static async Task<IReadOnlyCollection<T>> GetContinuationRequestAsync<T>(string url)
        => await HttpClientHelper.GetContinuationRequestAsync<T>(ApiUrl, url, _accessToken);

    static async Task Main()
    {
        Console.WriteLine("Started");
        
        _accessToken = await HttpClientHelper.GetAccessTokenAsync(AuthUrl, ClientId, ClientSecret, ApiScope);        

        GetUsersAndExamsAsync().Wait();

        Console.WriteLine("Done");
        Console.ReadLine();
    }
}