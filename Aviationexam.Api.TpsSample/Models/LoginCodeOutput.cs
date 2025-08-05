using System.ComponentModel.DataAnnotations;

namespace Aviationexam.Api.TpsSample.Models;

public sealed class LoginCodeOutput
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required string LoginCode { get; set; }
    
    public short? LoginCodeExpiration { get; set; }    
}