using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
