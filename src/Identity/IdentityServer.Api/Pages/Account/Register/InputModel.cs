using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Api.Pages.Register;

public class InputModel
{
    [Required]
    public string Username { get; set; }
        
    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Role { get; set; }

    public string ReturnUrl { get; set; }

    public string Button { get; set; }
}