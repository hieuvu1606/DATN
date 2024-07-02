namespace DATN.CustomModels
{
    public class CreateItem
    {
        public int DeviceId { get; set; }
        public DateTime ImportDate { get; set; }
        public int WarrantyPeriod { get; set; }
        public int MaintenanceTime { get; set; }
        public int ImporterId { get; set; }
        public int? PosId { get; set; }
    }
}
