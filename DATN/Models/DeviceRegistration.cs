using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class DeviceRegistration
{
    public int RegistId { get; set; }

    public int UserId { get; set; }

    public int? ManagerId { get; set; }

    public string? Proof { get; set; }

    public DateTime RegistDate { get; set; }

    public DateOnly BorrowDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public int WarehouseId { get; set; }

    public string? ActualReturnDate { get; set; }

    public string? ActualBorrowDate { get; set; }

    public string? Reason { get; set; }

    public virtual ICollection<ListDeviceRegist> ListDeviceRegists { get; set; } = new List<ListDeviceRegist>();

    public virtual User User { get; set; } = null!;
}
