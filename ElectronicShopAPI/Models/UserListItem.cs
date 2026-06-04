using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class UserListItem
{
    public int Id { get; set; }

    public int? UserListId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual UserList? UserList { get; set; }
}
