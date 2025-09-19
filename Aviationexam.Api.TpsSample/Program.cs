using Aviationexam.Api.Common.Helpers;
using Aviationexam.Api.TpsSample.Models;

namespace Aviationexam.Api.TpsSample;

static class Program
{
    /// The client information used to get the OAuth Access Token from the server.
    private const string ClientId = "REPLACE_WITH_YOUR_CLIENT_ID";
    private const string ClientSecret = "REPLACE_WITH_YOUR_CLIENT_SECRET";
    private const string ApiScope = "public-client";

    // Api url address
    private const string AuthUrl = "https://auth.beta.exams.aero/auth/connect/token";
    private const string ApiUrl = "https://api.beta.exams.aero/api/client/";

    private static ServiceManagement _service = null!;
    
    /// <summary>
    /// This method does all the work to get an Access Token and get all users and their exams.
    /// </summary>
    private static async Task<LoginCodeOutput> GenerateLoginCode(int userId)
        => await _service.PutAsync<string?, LoginCodeOutput>(null, $"user/{userId}/generate-login-code");

    /// <summary>
    /// Get all countries. 
    /// </summary>
    private static async Task<IReadOnlyCollection<CountryOutput>> GetCountries()
        => await _service.GetAsync<IReadOnlyCollection<CountryOutput>>("country");
    
    private static async Task<UserOutput> AddUser()
    {
        var testUser = new AddUserInput
        {
            FirstName = "John",
            LastName = "Doe",
            Identificator = "123456789",
            IsTestUser = true, // for testing purposes only, set false for real students
            Address = new AddressInput
            {
                Salutation = "Mr.",
                Phone = "123456789",
                CountryId = 66, // required (66 is for Germany)
            }
        };
        
        return await _service.PostAsync<AddUserInput, UserOutput>(testUser, "user");        
    }

    private static async Task<int> AddExam(int userId)
    {
        var newExam = new PlanningSimpleExamSetInput
        {
            IdQuestionBank = 294, // 294 == Droniq Question Bank
            IdUser = userId,
            ProctoredExams = true,
        };
        
        return await _service.PostAsync<PlanningSimpleExamSetInput, int>(newExam, "exam");
    }

    private static async Task<ExamSetOutput> GetExamSet(int userId, int examSetId) 
        => await _service.GetAsync<ExamSetOutput>($"exam/{userId}/{examSetId}");

    private static async Task<IReadOnlyCollection<ExamSetOutput>> GetAllExamSets(int userId)
        => await _service.GetAsync<IReadOnlyCollection<ExamSetOutput>>($"exam/{userId}");
    
    static async Task Main()
    {
        Console.WriteLine("Started");

        _service = new ServiceManagement(ClientId, ClientSecret, ApiScope, AuthUrl, ApiUrl);
 
        var accessToken = await _service.GetAccessTokenAsync();
        
        Console.WriteLine(!string.IsNullOrEmpty(accessToken) ? "Authentication successful." : "Authentication failed.");
        
        // Get countries. CountryId is required to add a new user. 
        // var countries = await GetCountries();

        var testUser = await AddUser();
        
        Console.WriteLine($"User {testUser.FirstName} {testUser.LastName} created. UserId: {testUser.Id}");

        var loginCode = await GenerateLoginCode(testUser.Id);
        
        Console.WriteLine($"Login code: {loginCode.LoginCode}");

        var examSetId = await AddExam(testUser.Id);

        Console.WriteLine($"ExamSet planned successfully with ID {examSetId}.");
        
        // get all user's exams
        var exams = await GetAllExamSets(testUser.Id);

        Console.WriteLine($"Got {exams.Count} exams.");
        
        // get singe exam
        //var exam = await GetExamSet(testUser.Id, 16360);
        
        Console.WriteLine("Done");
        Console.ReadLine();
    }
}