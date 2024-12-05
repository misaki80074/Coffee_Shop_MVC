using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class OrderStatusHistory
{
    public int HistoryId { get; set; }

    public string OrderId { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? Updatedate { get; set; }

    public string? Updateuser { get; set; }
}
