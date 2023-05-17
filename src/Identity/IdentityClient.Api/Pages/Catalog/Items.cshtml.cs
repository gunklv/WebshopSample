using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace IdentityClient.Api.Pages.Catalog;

public class ItemsModel : PageModel
{
    private readonly IOptions<CatalogConfiguration> _catalogConfiguration;

    public ItemsModel(IOptions<CatalogConfiguration> catalogConfiguration)
    {
        _catalogConfiguration = catalogConfiguration;
    }

    public async Task<IActionResult> OnGet()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await client.GetAsync($"{_catalogConfiguration.Value.BaseUrl}/items");

        if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Redirect("/Error/Unauthorized");
        }

        var items = JArray.Parse(await response.Content.ReadAsStringAsync()).ToString();
        ViewData["Items"] = items;

        return Page();
    }
}