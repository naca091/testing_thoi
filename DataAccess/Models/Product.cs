using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public int AreaId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string? Name { get; set; }

    public int Quantity { get; set; }

    public int Status { get; set; }

    public virtual StorageArea Area { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<LotDetail> LotDetails { get; } = new List<LotDetail>();

    public virtual ICollection<StockOutDetail> StockOutDetails { get; } = new List<StockOutDetail>();
}
