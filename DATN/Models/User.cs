using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DATN.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string CitizenId { get; set; } = null!;

    public int RoleId { get; set; }

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool RandomPassword { get; set; }
    [JsonIgnore]
    public virtual ICollection<DeviceRegistration> DeviceRegistrations { get; set; } = new List<DeviceRegistration>();
    [JsonIgnore]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;
}
