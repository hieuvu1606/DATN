using DATN.Models;

namespace DATN.CustomModels
{
    public class CreatePenalty
    {
        public int RegistId { get; set; }
        public int ManagerId { get; set; }
        public List<PenaltyItem> ListPenalty { get; set; }
    }

    public class PenaltyItem
    {
        public int ItemID { get; set; }
        public int Fine { get; set; }
    }
}
