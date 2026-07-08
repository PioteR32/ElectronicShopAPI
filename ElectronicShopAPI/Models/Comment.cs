using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class Comment
{
    public int Id { get; set; }

    public string Comment1 { get; set; } = null!;

    public byte Rating { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
