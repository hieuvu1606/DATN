using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DATN.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Descr { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
