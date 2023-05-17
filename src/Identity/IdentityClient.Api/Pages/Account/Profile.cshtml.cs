using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityClient.Api.Pages.Account;

[Authorize]
public class ProfileModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);

        var timeBack = jwtSecurityToken.ValidTo - DateTime.UtcNow;

        if (!(timeBack < TimeSpan.FromSeconds(1770)))
            return Page();

        var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

        var client = new HttpClient();

        var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = "https://localhost:5001/connect/token",
            ClientId = "identityClient",
            ClientSecret = "secret",
            RefreshToken = refreshToken,
            GrantType = "authorization_code"
        });

        if (tokenResponse.IsError)
        {
            return SignOut("Cookies", "oidc");
        }

        var auth = await HttpContext.AuthenticateAsync("Cookies");
        auth.Properties.StoreTokens(new List<AuthenticationToken>()
        {
            new AuthenticationToken()
            {
                Name = OpenIdConnectParameterNames.AccessToken,
                Value = tokenResponse.AccessToken
            },
            new AuthenticationToken()
            {
                Name = OpenIdConnectParameterNames.RefreshToken,
                Value = tokenResponse.RefreshToken
            },
            new AuthenticationToken()
            {
                Name = OpenIdConnectParameterNames.IdToken,
                Value = tokenResponse.IdentityToken
            }
        });

        var jwtSecurityToken2 = handler.ReadJwtToken(tokenResponse.AccessToken);

        auth.Properties.SetString(".Token.expires_at", jwtSecurityToken2.Payload.ValidTo.ToString());

        await HttpContext.SignInAsync(auth.Principal, auth.Properties);
        return Page();
    }
}