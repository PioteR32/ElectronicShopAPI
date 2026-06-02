using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Email { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string House { get; set; } = null!;

    public int TownId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
