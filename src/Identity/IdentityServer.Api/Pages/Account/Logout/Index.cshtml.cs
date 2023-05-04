using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Api.Pages.Logout;

[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationSchemeProvider _schemeProvider;

    [BindProperty]
    public string LogoutId { get; set; }

    public Index(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider)
    {
        _interaction = interaction;
        _schemeProvider = schemeProvider;
    }

    public async Task<IActionResult> OnGet(string logoutId)
    {
        LogoutId = logoutId;
        return await OnPost();
    }

    public async Task<IActionResult> OnPost()
    {
            LogoutId ??= await _interaction.CreateLogoutContextAsync();

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            foreach (var scheme in schemes)
            {
                await HttpContext.SignOutAsync(scheme.Name);
            }

            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp != null && idp != Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider)
            {
                if (await HttpContext.GetSchemeSupportsSignOutAsync(idp))
                {
                    string url = Url.Page("/Account/Logout/Loggedout", new { logoutId = LogoutId });

                    return SignOut(new AuthenticationProperties { RedirectUri = url }, idp);
                }
            }

        var logout = await _interaction.GetLogoutContextAsync(LogoutId);

        return Redirect(logout.PostLogoutRedirectUri);
    }
}