using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.Controllers;

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
