﻿using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Descr { get; set; } = null!;

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
