using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class StockOutDetail
{
    public int StockOutDetailId { get; set; }

    public int ProductId { get; set; }

    public int StockOutId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual StockOut StockOut { get; set; } = null!;
}
