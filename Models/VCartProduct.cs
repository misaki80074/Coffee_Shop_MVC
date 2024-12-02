using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class VCartProduct
{
    public string CartId { get; set; } = null!;

    public short CartItemId { get; set; }

    public string? ProductId { get; set; }

    public short? Qty { get; set; }

    public short? UnitPrice { get; set; }

    public int? TotalPrice { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? ProductName { get; set; }

    public string? UserId { get; set; }

    public string? Status { get; set; }

    public string? DStatus { get; set; }

    public string? Img { get; set; }
}
