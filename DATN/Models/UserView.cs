using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class UserView
{
    public int UserId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string CitizenId { get; set; } = null!;
}
