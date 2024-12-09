using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Admlookup
{
    public int Id { get; set; }

    public string Lookupid { get; set; } = null!;

    public string? Name { get; set; }

    public string? Classno { get; set; }

    public DateTime? Createdate { get; set; }

    public string? Createuser { get; set; }

    public DateTime? Updatedate { get; set; }

    public string? Updateuser { get; set; }

    public string? Status { get; set; }
}
