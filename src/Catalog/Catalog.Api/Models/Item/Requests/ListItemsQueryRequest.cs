using Catalog.Api.Models.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Models.Item.Requests
{
    public class ListItemsQueryRequest : PagedQueryParams
    {
        [FromQuery(Name = "categoryId")]
        public Guid? CategoryId { get; set; }
    }
}
