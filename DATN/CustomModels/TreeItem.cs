namespace DATN.CustomModels
{
    public class TreeItem
    {
        public int WarehouseID { get; set; }
        public string WarehouseDescr { get; set; }
        public List<DeviceNode> ListDevice { get; set; }
    }

    public class DeviceNode
    {
        public int DeviceID { get; set; }
        public string DeviceDescr { get; set; }
        public List<int> ListItemID { get; set; }
    }
}
