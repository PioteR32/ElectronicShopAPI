using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class FilterOption
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int FilterId { get; set; }

    public int? ParentId { get; set; }

    public virtual Filter Filter { get; set; } = null!;

    public virtual ICollection<FilterOption> InverseParent { get; set; } = new List<FilterOption>();

    public virtual FilterOption? Parent { get; set; }
}
