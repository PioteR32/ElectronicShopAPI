namespace Shared.DTO
{
	public class UserListDTO
	{
        public UserListDTO() { }
        public UserListDTO(UserListDTO other)
		{
			this.Id = other.Id;
			this.CustomerId = other.CustomerId;
			this.Name = other.Name;
			this.Description = other.Description;
		}

		public int Id { get; set; }
		public int? CustomerId { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		// conversion moved to ElectronicShopAPI.Extensions.DtoExtensions

	}
}
