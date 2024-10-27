using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Lot
{
    public int LotId { get; set; }

    public int AccountId { get; set; }

    public int PartnerId { get; set; }

    public string LotCode { get; set; } = null!;

    public DateTime DateIn { get; set; }

    public string? Note { get; set; }

    public int Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<LotDetail> LotDetails { get; } = new List<LotDetail>();

    public virtual Partner Partner { get; set; } = null!;
}
