using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class DetailsRegistration
{
    public int RegistId { get; set; }

    public string Sku { get; set; } = null!;

    public string? Status { get; set; }

    public virtual ICollection<DetailsPenaltyTicket> DetailsPenaltyTickets { get; set; } = new List<DetailsPenaltyTicket>();

    public virtual DeviceRegistration Regist { get; set; } = null!;

    public virtual Item SkuNavigation { get; set; } = null!;
}
