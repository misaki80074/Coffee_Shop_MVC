using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Orderheader
{
    public string OrderId { get; set; } = null!;

    public int Id { get; set; }

    public string? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Payment { get; set; }

    public int? Total { get; set; }

    public string? ShipStatus { get; set; }

    public string? CreateUser { get; set; }

    public string? UpdateUser { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Status { get; set; }

    public string? Name { get; set; }

    public string? Mail { get; set; }

    public string? Phone { get; set; }

    public string? Comment { get; set; }

    public string? ShippingMethod { get; set; }

    public string? Address { get; set; }
}
