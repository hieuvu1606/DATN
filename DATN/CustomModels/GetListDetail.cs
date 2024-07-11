using DATN.Models;

namespace DATN.CustomModels
{
    public class GetListDetail
    {
        public string DeviceDescr { get; set; }
        public ListDeviceRegist DeviceRegist { get; set; }
        public List<DetailRegist> ListDetails { get; set; }
    }
}
