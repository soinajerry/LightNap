namespace LightNap.Core.Api
{
    public class PaginationRequestBase
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => this._pageSize;
            set => this._pageSize = (value > PaginationRequestBase.MaxPageSize) ? PaginationRequestBase.MaxPageSize : value;
        }
    }

}
