using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductId { get; set; } = null!;

    public string? ProductName { get; set; }

    public short? Price { get; set; }

    public string? Category { get; set; }

    public string? Uom { get; set; }

    public string? Description { get; set; }

    public string? Country { get; set; }

    public string? Baking { get; set; }

    public string? Flavor { get; set; }

    public string? CreateUser { get; set; }

    public string? UpdateUser { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Timelimit { get; set; }

    public byte? Fragrance { get; set; }

    public byte? Sour { get; set; }

    public byte? Bitter { get; set; }

    public byte? Sweet { get; set; }

    public byte? Strong { get; set; }

    public string? Method { get; set; }

    public string? Img { get; set; }

    public bool? Status { get; set; }

    public string? Weight { get; set; }
}
