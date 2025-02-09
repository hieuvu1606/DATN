﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

    public DateTime? ActualReturnDate { get; set; }

    public DateTime? ActualBorrowDate { get; set; }

    public string? Reason { get; set; }

    public string? Notice { get; set; }

    public bool? IsFine { get; set; }
    [JsonIgnore]
    public virtual ICollection<DetailsPenaltyTicket> DetailsPenaltyTickets { get; set; } = new List<DetailsPenaltyTicket>();
    [JsonIgnore]
    public virtual ICollection<ListDeviceRegist> ListDeviceRegists { get; set; } = new List<ListDeviceRegist>();
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
