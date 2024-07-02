using Azure;

namespace DATN.Utils.Response
{
    public class PagedResponse<T> : Response<T>
    {
        public int ItemCount { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int soLuongCM { get; set; }
        public int soLuongHT { get; set; }
        public int soLuongTong { get; set; }

        public PagedResponse(T data, int page, int pageSize, int itemCount, bool succeeded)
        {
            this.ItemCount = itemCount;
            this.page = page;
            this.pageSize = pageSize;
            this.Data = data;
            this.Succeeded = succeeded;
        }

        public PagedResponse(T data, int page, int pageSize, int itemCount, int soLuongCM, int soLuongHT, int soLuongTong)
        {
            this.ItemCount = itemCount;
            this.soLuongCM = soLuongCM;
            this.soLuongHT = soLuongHT;
            this.soLuongTong = soLuongTong;
            this.page = page;
            this.pageSize = pageSize;
            this.Data = data;
        }
    }
}
