using DATN.Models;

namespace DATN.CustomModels
{
    public class GetListDetail
    {
        public string DeviceDescr { get; set; }
        public ListDeviceRegist DeviceRegist { get; set; }
        public List<CustomDetail> ListDetails { get; set; }
    }

    public class CustomDetail : DetailRegist
    {
        public string DeviceDescr { get; set; }
    }
}
