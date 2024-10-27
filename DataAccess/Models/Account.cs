using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string AccountCode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }

    public string? Phone { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Lot> Lots { get; } = new List<Lot>();

    public virtual ICollection<StockOut> StockOuts { get; } = new List<StockOut>();
}
