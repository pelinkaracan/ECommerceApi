namespace ECommerceApi.RestApi.Models.Common
{
    // Represents a paged result set with information about the items, total number of items, current page, and page size
    public class PagedResult<T>
    {
        // The collection of items in the current page
        public IEnumerable<T> Items { get; }

        // The total number of items across all pages
        public int TotalItems { get; }

        // The current page number
        public int Page { get; }

        // The number of items per page
        public int PageSize { get; }

        // The total number of pages
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        // Initializes a new instance of the PagedResult class
        public PagedResult(IEnumerable<T> items, long totalItems, int page, int pageSize)
        {
            Items = items;
            TotalItems = (int)totalItems;
            Page = page;
            PageSize = pageSize;
        }
    }
}
