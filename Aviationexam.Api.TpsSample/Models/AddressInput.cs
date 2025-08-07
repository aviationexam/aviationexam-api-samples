namespace Aviationexam.Api.TpsSample.Models;

public class AddressInput
{
    public string? Salutation { get; set; }
    public string? Degree { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    
    public required int CountryId { get; set; }
}
