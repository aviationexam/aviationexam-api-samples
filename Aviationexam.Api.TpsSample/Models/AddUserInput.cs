namespace Aviationexam.Api.TpsSample.Models;

public class AddUserInput
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Identificator { get; set; }
    
    public string? Email { get; set; }

    public int? GroupId { get; set; }

    public bool IsTestUser { get; set; }
    
    public required AddressInput Address { get; set; }    
}
