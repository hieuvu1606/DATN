using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Descr { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
