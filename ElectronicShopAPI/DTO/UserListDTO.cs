namespace ElectronicShopAPI.DTO
{
	public class UserListDTO
	{
		public int? Id { get; set; }
		public int? CustomerId { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public static UserListDTO FromModel(Models.UserList ul) => new UserListDTO
		{
			Id = ul.Id,
			CustomerId = ul.CustomerId,
			Name = ul.Name,
			Description = ul.Description
		};

	}
}
