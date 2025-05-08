using Microsoft.AspNetCore.Mvc;

namespace VCommerce.Mvc.Extensions;

public static class UrlHelperExtensions
{
    public static string? EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
    {
        return urlHelper.Action(
            action: "ConfirmEmail",
            controller: "Account",
            values: new { userId, code },
            protocol: scheme);
    }
}
