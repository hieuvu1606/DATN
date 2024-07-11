using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DATN.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string? Address { get; set; }

    public string WarehouseDescr { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
