using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Orderdetail
{
    public string OrderId { get; set; } = null!;

    public short OrderItem { get; set; }

    public int Id { get; set; }

    public string? ProductId { get; set; }

    public short? Qty { get; set; }

    public string? Uom { get; set; }

    public short? UnitPrice { get; set; }

    public int? Totle { get; set; }

    public string? CreateUser { get; set; }

    public string? UpdateUser { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
