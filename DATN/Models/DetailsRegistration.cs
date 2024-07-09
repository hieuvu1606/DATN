using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DATN.Models;

public partial class DetailsRegistration
{
    public int RegistId { get; set; }

    public string Sku { get; set; } = null!;

    public string? Status { get; set; }
    [JsonIgnore]
    public virtual ICollection<DetailsPenaltyTicket> DetailsPenaltyTickets { get; set; } = new List<DetailsPenaltyTicket>();
    [JsonIgnore]
    public virtual DeviceRegistration Regist { get; set; } = null!;
    [JsonIgnore]
    public virtual Item SkuNavigation { get; set; } = null!;
}
