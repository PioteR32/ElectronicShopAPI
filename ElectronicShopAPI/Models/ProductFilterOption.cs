using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class ProductFilterOption
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int FilterOptionId { get; set; }

    public virtual FilterOption FilterOption { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
