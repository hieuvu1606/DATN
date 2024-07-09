using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class ListDeviceRegist
{
    public int RegistId { get; set; }

    public int DeviceId { get; set; }

    public int? BorrowQuantity { get; set; }

    public int? ConfirmQuantity { get; set; }

    public virtual ICollection<DetailRegist> DetailRegists { get; set; } = new List<DetailRegist>();

    public virtual Device Device { get; set; } = null!;

    public virtual DeviceRegistration Regist { get; set; } = null!;
}
