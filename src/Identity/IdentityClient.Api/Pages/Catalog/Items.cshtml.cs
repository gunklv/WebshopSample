using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace IdentityClient.Api.Pages.Catalog;

public class ItemsModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await client.GetAsync("https://localhost:56540/items");

        if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Redirect("/Error/Unauthorized");
        }

        var items = JArray.Parse(await response.Content.ReadAsStringAsync()).ToString();
        ViewData["Items"] = items;

        return Page();
    }
}