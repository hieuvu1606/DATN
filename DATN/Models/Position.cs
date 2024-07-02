using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class Position
{
    public int PosId { get; set; }

    public int WarehouseId { get; set; }

    public string PositionDescr { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}
