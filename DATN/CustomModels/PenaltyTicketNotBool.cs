namespace DATN.CustomModels
{
    public class PenaltyTicketNotBool
    {
        public int PenaltyId { get; set; }

        public int ManagerId { get; set; }

        public string? Proof { get; set; }

        public string Status { get; set; }

        public int? TotalFine { get; set; }
    }
}
