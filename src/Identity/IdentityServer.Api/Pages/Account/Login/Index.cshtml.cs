using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Api.Pages.Login;

[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    [BindProperty]
    public InputModel Input { get; set; }

    public Index(
        IIdentityServerInteractionService interaction,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager
        )
    {
        _interaction = interaction;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        if (Input.Button != "login")
        {
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            return Redirect(Input.ReturnUrl);
        }

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(Input.Username);

            if ((await _signInManager.PasswordSignInAsync(user, Input.Password, false, false)).Succeeded)
            {
                var isuser = new IdentityServerUser(user.Id)
                {
                    DisplayName = user.UserName
                };

                await HttpContext.SignInAsync(isuser);

                return Redirect(Input.ReturnUrl);
            }
        }

        Input = new InputModel
        {
            ReturnUrl = Input.ReturnUrl
        };

        return Page();
    }
}