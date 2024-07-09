using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DATN.Models;

public partial class ListDeviceRegist
{
    public int RegistId { get; set; }

    public int DeviceId { get; set; }

    public int? BorrowQuantity { get; set; }

    public int? ConfirmQuantity { get; set; }
    [JsonIgnore]
    public virtual ICollection<DetailRegist> DetailRegists { get; set; } = new List<DetailRegist>();
    [JsonIgnore]
    public virtual Device Device { get; set; } = null!;
    [JsonIgnore]
    public virtual DeviceRegistration Regist { get; set; } = null!;
}
