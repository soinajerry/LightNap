namespace LightNap.Core.Api
{
    public class PagedResponse<T>
    {
        public IList<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(this.TotalCount / (double)this.PageSize);

        public PagedResponse(IList<T> data, int pageNumber, int pageSize, int totalCount)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
        }
    }

}
