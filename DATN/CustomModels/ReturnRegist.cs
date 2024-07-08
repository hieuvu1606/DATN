namespace DATN.CustomModels
{
    public class ReturnRegist
    {
        public int RegistID { get; set; }
        public List<ReturnItem> ListItem { get; set; }
    }

    public class ReturnItem
    {
        public int ItemID { get; set;}
        public string CurrentStatus { get; set;}
    }
}
