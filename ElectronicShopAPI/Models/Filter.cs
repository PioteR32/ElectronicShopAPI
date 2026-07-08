using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class Filter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CategoriesFilter> CategoriesFilters { get; set; } = new List<CategoriesFilter>();

    public virtual ICollection<FilterOption> FilterOptions { get; set; } = new List<FilterOption>();
}
