using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class VOrderHistory
{
    public string OrderId { get; set; } = null!;

    public string? Status { get; set; }

    public string? Name { get; set; }

    public string? Classno { get; set; }

    public DateTime? Updatedate { get; set; }

    public string? Updateuser { get; set; }
}
