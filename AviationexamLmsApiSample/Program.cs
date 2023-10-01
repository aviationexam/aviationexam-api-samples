using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Aviationexam.LmsApiSample
{
    class Program
    {
        /// The client information used to get the OAuth Access Token from the server.
        private const string ClientId = "REPLACE_WITH_YOUR_CLIENT_ID";
        private const string ClientSecret = "REPLACE_WITH_YOUR_CLIENT_SECRET";

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
            // Get the Access Token.
            _accessToken = await GetAccessTokenAsync();
            Console.WriteLine(_accessToken != null ? "Got Token" : "No Token found");

            // Next time use date of the previous synchronization instead of DateTime.MinValue
            var users = await GetUsersAsync(DateTime.MinValue);
            Console.WriteLine($"Users count: {users?.Count}");
            
            var exams = await GetExamsAsync(DateTime.MinValue);

            Console.WriteLine($"Exams count: {exams?.Count}");

            return 0;
        }
        
        /// <summary>
        /// This method uses the OAuth Client Credentials Flow to get an Access Token to provide
        /// Authorization to the APIs.
        /// </summary>
        private static async Task<string> GetAccessTokenAsync()
        {
            using var client = GetClient(AuthUrl);
            // Build up the data to POST.
            var postData = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "client_credentials"),
                new("client_id", ClientId),
                new("client_secret", ClientSecret),
                new("scope", "public-lms")
            };

            var content = new FormUrlEncodedContent(postData);

            // Post to the Server and parse the response.
            var response = await client.PostAsync("Token", content);
            string jsonString = await response.Content.ReadAsStringAsync();
            object responseData = JsonConvert.DeserializeObject(jsonString);

            // return the Access Token.
            return ((dynamic) responseData)?.access_token;
        }

        /// <summary>
        /// Get users.
        /// </summary>
        /// <param name="lastDateUpdate">Date of last synchronization. Only users registered after this date will be returned.</param>
        /// <returns>The page of articles.</returns>
        private static async Task<List<GetLmsStudentOutput>> GetUsersAsync(DateTime lastDateUpdate)
        {
            string url = $"lms/users?pageSize={PageSize}&lastDateUpdate={lastDateUpdate:O}";

            return await GetContinuationRequestAsync<GetLmsStudentOutput>(url);
        }

        /// <summary>
        /// Get exams.
        /// </summary>
        /// <param name="lastDateUpdate">Date of last synchronization. Only exams generated after this date will be returned.</param>
        /// <returns>The page of articles.</returns>
        private static async Task<List<GetLmsStudentExamOutput>> GetExamsAsync(DateTime lastDateUpdate)
        {
            string url = $"lms/exams?pageSize={PageSize}&lastDateUpdate={lastDateUpdate:O}";

            return await GetContinuationRequestAsync<GetLmsStudentExamOutput>(url);
        }

        private static HttpClient GetClient(string baseUrl)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Add the Authorization header with the AccessToken.
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            return client;
        }

        private static async Task<List<T>> GetContinuationRequestAsync<T>(string queryUrl)
        {
            var list = new List<T>();

            using var client = GetClient(ApiUrl);
            string continuationToken = "";

            do
            {
                var url = string.IsNullOrEmpty(continuationToken) ? queryUrl : queryUrl + $"&continuationToken={continuationToken}";

                // make the request
                var response = await client.GetAsync(url);
                string jsonString = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(jsonString))
                {
                    var responseData = JsonConvert.DeserializeObject<ContinuationResponse<T>>(jsonString);
                    if (responseData.Items != null)
                    {
                        list.AddRange(responseData.Items);
                    }

                    continuationToken = responseData.ContinuationToken;
                }
                else
                {
                    break;
                }

            } while (!string.IsNullOrEmpty(continuationToken));

            // parse the response and return the data.
            return list;
        }

        static void Main()
        {
            Console.WriteLine("Started");

            GetUsersAndExamsAsync().Wait();

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}