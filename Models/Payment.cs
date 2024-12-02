using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Payment
{
    public string PayId { get; set; } = null!;

    public int Id { get; set; }

    public string OrderId { get; set; } = null!;

    public string? CustomerId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? PaymentAmount { get; set; }

    public string? Currency { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public string? CreateUser { get; set; }

    public string? UpdateUser { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Status { get; set; }
}
