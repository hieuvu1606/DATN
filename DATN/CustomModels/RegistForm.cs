using DATN.Models;

namespace DATN.CustomModels
{
    public class RegistForm
    {
        // Properties from DeviceRegistration
        public int RegistID { get; set; }
        public int UserID { get; set; }
        public int ManagerID { get; set; }
        public string Proof { get; set; }
        public DateTime RegistDate { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public string Status { get; set; }
        public int WarehouseID { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public DateTime? ActualBorrowDate { get; set; }
        public string Reason { get; set; }

        // Properties from ListDeviceRegist
        public List<ListDeviceRegist> ListDeviceRegists { get; set; } = new List<ListDeviceRegist>();
    }
}
