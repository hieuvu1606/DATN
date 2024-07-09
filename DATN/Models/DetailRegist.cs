using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class DetailRegist
{
    public int RegistId { get; set; }

    public int DeviceId { get; set; }

    public int ItemId { get; set; }

    public string BeforeStatus { get; set; } = null!;

    public string? AfterStatus { get; set; }

    public virtual ICollection<DetailsPenaltyTicket> DetailsPenaltyTickets { get; set; } = new List<DetailsPenaltyTicket>();

    public virtual Item Item { get; set; } = null!;

    public virtual ListDeviceRegist ListDeviceRegist { get; set; } = null!;
}
