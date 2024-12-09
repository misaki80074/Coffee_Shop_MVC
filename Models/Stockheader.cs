using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Stockheader
{
    public string StockId { get; set; } = null!;

    public int Id { get; set; }

    public string? ProductId { get; set; }

    public string? Uom { get; set; }

    public short? Qty { get; set; }

    public string? CreateUser { get; set; }

    public string? UpdateUser { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Status { get; set; }
}
