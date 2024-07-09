using DATN.Models;

namespace DATN.CustomModels
{
    public class GetListDetail
    {
        public ListDeviceRegist DeviceRegist { get; set; }
        public List<DetailRegist> ListDetails { get; set; }
    }
}
