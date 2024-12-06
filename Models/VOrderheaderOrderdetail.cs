using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class VOrderheaderOrderdetail
{
    public string? OrderId { get; set; }

    public short OrderItem { get; set; }

    public string? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Payment { get; set; }

    public string? ShipStatus { get; set; }

    public string? ProductId { get; set; }

    public short? Qty { get; set; }

    public string? Uom { get; set; }

    public short? UnitPrice { get; set; }

    public int? Totle { get; set; }

    public string? Status { get; set; }

    public int? Total { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Mail { get; set; }

    public string? Comment { get; set; }

    public string? CustomerName { get; set; }

    public string? OrderStatus { get; set; }

    public string? ShipStatu { get; set; }

    public string? PayMathod { get; set; }

    public string? ShipMethod { get; set; }

    public string? ShippingMethod { get; set; }

    public string? Address { get; set; }

    public string? ProductName { get; set; }

    public string? ImgSrc { get; set; }
}
