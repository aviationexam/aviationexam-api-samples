namespace Aviationexam.Api.TpsSample.Models;

public sealed class UserOutput
{
    public required int Id { get; set; }

    public int? IdCaa { get; set; }
    public string? Username { get; set; }

    public required bool Valid { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
    public string? Identificator { get; set; }
    public DateTime? DateRegistered { get; set; }

    public required bool IsArchived { get; set; }

    public required bool IsTestUser { get; set; }

    public AddressOutput? Address { get; set; }
}
