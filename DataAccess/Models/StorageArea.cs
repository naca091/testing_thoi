using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class StorageArea
{
    public int AreaId { get; set; }

    public string AreaCode { get; set; } = null!;

    public string AreaName { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
