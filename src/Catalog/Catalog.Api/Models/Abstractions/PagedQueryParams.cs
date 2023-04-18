using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Models.Abstractions
{
    public class PagedQueryParams
    {
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        [FromQuery(Name = "page")]
        public int Page { get; set; } = DEFAULT_PAGE_NUMBER;

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = DEFAULT_PAGE_SIZE;
    }
}
