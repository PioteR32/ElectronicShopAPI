using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int NumOnStock { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

    public virtual ICollection<UserListItem> UserListItems { get; set; } = new List<UserListItem>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
