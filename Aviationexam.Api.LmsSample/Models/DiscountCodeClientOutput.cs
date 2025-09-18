using System;

namespace Aviationexam.LmsApiSample.Models;

public sealed class DiscountCodeClientOutput
{
    public required string Code { get; set; }

    public required int DiscountPercent { get; set; }

    public required bool AlreadyUsed { get; set; }

    public string? UserClientId { get; set; }

    public required DateTimeOffset DateCreated { get; set; }

    public required DateTimeOffset ValidTill { get; set; }
}
