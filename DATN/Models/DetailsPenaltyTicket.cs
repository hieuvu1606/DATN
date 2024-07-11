using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class DetailsPenaltyTicket
{
    public int PenaltyId { get; set; }

    public int RegistId { get; set; }

    public int LineRef { get; set; }

    public string Descr { get; set; } = null!;

    public int Fine { get; set; }
    [JsonIgnore]
    public virtual PenaltyTicket Penalty { get; set; } = null!;
    [JsonIgnore]
    public virtual DeviceRegistration Regist { get; set; } = null!;
}
