using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DATN.Models;

public partial class Item
{
    public int DeviceId { get; set; }

    public int ItemId { get; set; }

    public DateTime ImportDate { get; set; }

    public int WarrantyPeriod { get; set; }

    public int MaintenanceTime { get; set; }

    public DateOnly LastMaintenance { get; set; }

    public string Status { get; set; } = null!;

    public bool IsStored { get; set; }

    public int ImporterId { get; set; }

    public int? PosId { get; set; }

    public string Qr { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<DetailRegist> DetailRegists { get; set; } = new List<DetailRegist>();
    [JsonIgnore]
    public virtual Device Device { get; set; } = null!;
    [JsonIgnore]
    public virtual User Importer { get; set; } = null!;
    [JsonIgnore]
    public virtual Position? Pos { get; set; }
}
