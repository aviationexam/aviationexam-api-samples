namespace Aviationexam.Api.TpsSample.Models;

public sealed class AddressOutput
{
    public required int Id { get; set; }

    public required bool Valid { get; set; }

    public string? Salutation { get; set; }
    public string? Degree { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public string? Url { get; set; }

    public int? CountryId { get; set; }
}
