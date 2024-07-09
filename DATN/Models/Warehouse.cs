using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string? Address { get; set; }

    public string WarehouseDescr { get; set; } = null!;

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
