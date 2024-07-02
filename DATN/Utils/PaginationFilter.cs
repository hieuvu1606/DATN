namespace DATN.Utils
{
    public class PaginationFilter
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public PaginationFilter()
        {
            this.page = 1;
            this.pageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.page = pageNumber < 1 ? 1 : pageNumber;
            this.pageSize = pageSize < 10 ? 10 : pageSize;
        }
    }
}
