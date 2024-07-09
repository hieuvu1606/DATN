using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class PenaltyTicket
{
    public int PenaltyId { get; set; }

    public int ManagerId { get; set; }

    public string? Proof { get; set; }

    public bool? Status { get; set; }

    public int? TotalFine { get; set; }
    [JsonIgnore]
    public virtual ICollection<DetailsPenaltyTicket> DetailsPenaltyTickets { get; set; } = new List<DetailsPenaltyTicket>();
}
