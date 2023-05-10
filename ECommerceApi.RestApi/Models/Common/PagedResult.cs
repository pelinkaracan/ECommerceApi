namespace ECommerceApi.RestApi.Models.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int TotalItems { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public PagedResult(IEnumerable<T> items, long totalItems, int page, int pageSize)
        {
            Items = items;
            TotalItems = (int)totalItems;
            Page = page;
            PageSize = pageSize;
        }
    }
}
