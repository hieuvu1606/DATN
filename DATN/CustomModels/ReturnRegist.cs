namespace DATN.CustomModels
{
    public class ReturnRegist
    {
        public int RegistID { get; set; }
        public List<ReturnItem> ListItem { get; set; }
    }

    public class ReturnItem
    {
        public int ItemId { get; set;}
        public string AfterStatus { get; set;}
    }
}
