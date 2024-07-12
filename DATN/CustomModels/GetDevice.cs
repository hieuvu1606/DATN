namespace DATN.CustomModels
{
    public class GetDevice
    {
        public int WarehouseID { get; set; }
        public int DeviceID { get; set; }
        public string? WarehouseDescr { get; set; }
        public string? DeviceDescr { get; set; }
        public string? DeviceShortDescr { get; set; }
        public string? Image { get; set; }
        public int CurrentAmount { get; set; }
        public int TotalAmount { get; set; }

    }
}
