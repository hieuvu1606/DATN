using DATN.Models;

namespace DATN.CustomModels
{
    public class CreatePenalty
    {
        public int RegistId { get; set; }
        public int ManagerId { get; set; }
        public string Proof { get; set; }
        public int? TotalFine { get; set; }
        public List<DetailsPenaltyTicket> Details { get; set; }
    }
}
