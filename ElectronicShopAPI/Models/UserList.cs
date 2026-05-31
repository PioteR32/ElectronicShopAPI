using System;
using System.Collections.Generic;

namespace ElectronicShopAPI.Models;

public partial class UserList
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<UserListItem> UserListItems { get; set; } = new List<UserListItem>();
}
