namespace Aviationexam.Api.Common.Models;

public class ContinuationResponse<T>
{
    public IEnumerable<T> Items { get; set; } = null!;
        
    public string ContinuationToken { get; set; } = null!;
}