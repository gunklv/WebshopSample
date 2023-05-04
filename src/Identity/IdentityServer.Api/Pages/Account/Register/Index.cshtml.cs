using IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Api.Pages.Register;

[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    [BindProperty]
    public InputModel Input { get; set; }

    public Index(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Input.Button != "register")
        {
            return Redirect("~/");
        }

        if (ModelState.IsValid)
        {
            var role = await _roleManager.FindByNameAsync(Input.Role);

            var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email };

            await _userManager.CreateAsync(user, Input.Password);

            await _userManager.AddToRoleAsync(user, role.Name);
        }

        return Redirect("~/");
    }
}