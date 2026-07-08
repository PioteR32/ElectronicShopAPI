using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class CategoriesFilter
{
    public int Id { get; set; }

    public int FilterId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Filter Filter { get; set; } = null!;
}
