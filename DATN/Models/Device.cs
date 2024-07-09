using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;


namespace DATN.Models;

public partial class Device
{
    public int DeviceId { get; set; }

    public int CategoryId { get; set; }

    public string Descr { get; set; } = null!;

    public string ShortDescr { get; set; } = null!;

    public string? Image { get; set; }

    public string? DescrFunction { get; set; }

    public string? Pdf { get; set; }
    [JsonIgnore]
    public virtual Category Category { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    [JsonIgnore]
    public virtual ICollection<ListDeviceRegist> ListDeviceRegists { get; set; } = new List<ListDeviceRegist>();
}
