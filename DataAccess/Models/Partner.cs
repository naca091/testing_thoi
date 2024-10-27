using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Partner
{
    public int PartnerId { get; set; }

    public string PartnerCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<LotDetail> LotDetails { get; } = new List<LotDetail>();

    public virtual ICollection<Lot> Lots { get; } = new List<Lot>();

    public virtual ICollection<StockOut> StockOuts { get; } = new List<StockOut>();
}
