using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Name { get; set; }

    public bool? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? ImgSrc { get; set; }

    public string? Language { get; set; }

    public string? ReceiverAddress { get; set; }

    public string? CreateUser { get; set; }

    public string? UpdateUser { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Status { get; set; }

    public bool IsSuspended { get; set; }

    public bool IsDeleted { get; set; }
}
