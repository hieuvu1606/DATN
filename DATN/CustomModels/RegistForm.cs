using DATN.Models;

namespace DATN.CustomModels
{
    public class RegistForm
    {
        // Properties from DeviceRegistration
        public int UserID { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public int WarehouseID { get; set; }
        public string Notice {  get; set; }

        // Properties from ListDeviceRegist
        public List<ListRegist> ListRegist { get; set; }
    }


    public class ListRegist
    {
        public int DeviceID { get; set; }
        public int Quantity { get; set; }
    }
}
