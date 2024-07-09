using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class DetailsPenaltyTicket
{
    public int PenaltyId { get; set; }

    public int RegistId { get; set; }

    public int ItemId { get; set; }

    public int DeviceId { get; set; }

    public int Fine { get; set; }

    public virtual DetailRegist DetailRegist { get; set; } = null!;

    public virtual PenaltyTicket Penalty { get; set; } = null!;
}
