using System.Collections.Generic;
using System.Linq;
using Shared.DTO;
using ElectronicShopAPI.Models;
// string do pobierania struktury bazy danych z serwera SQL Server i generowania modeli w projekcie. Użyj tego polecenia w konsoli Menedżera pakietów NuGet w Visual Studio, aby wygenerować modele na podstawie istniejącej bazy danych.
//Scaffold-DbContext "Server=.;Database=electronic_shop;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -Context DataContext -Force


namespace ElectronicShopAPI.Extensions
{
	public static class Extensions
	{
		// Product
		public static ProductDTO ToDto(this Product p) => new ProductDTO
		{
			Id = p.Id,
			Name = p.Name,
			Description = p.Description,
			Price = p.Price,
			NumOnStock = p.NumOnStock
		};

		public static IEnumerable<ProductDTO> ToDtos(this IEnumerable<Product> products) => products.Select(p => p.ToDto());

		// Customer
		public static CustomerDTO ToDto(this Customer c) => new CustomerDTO
		{
			Id = c.Id,
			FirstName = c.FirstName,
			LastName = c.LastName,
			Email = c.Email,
			Password = c.Password,
			PhoneNumber = c.PhoneNumber
		};

		// OrderItem
		public static OrderItemDTO ToDto(this OrderItem oi) => new OrderItemDTO
		{
			Id = oi.Id,
			ProductId = oi.ProductId ?? -1,
			Quantity = oi.Quantity
		};

		// Order (including items)
		public static OrderDTO ToDto(this Order o) => new OrderDTO
		{
			Id = o.Id,
			CustomerId = o.CustomerId,
			OrderDate = o.OrderDate,
			TotalAmount = o.TotalAmount,
			Email = o.Email,
			Street = o.Street,
			House = o.House,
			Town = o.TownId != null ? o.TownId.ToString() : null,
			OrderItemDTO = o.OrderItems?.Select(oi => oi.ToDto()).ToList() ?? new List<OrderItemDTO>()
		};

		// UserList
		public static UserListDTO ToDto(this UserList ul) => new UserListDTO
		{
			Id = ul.Id,
			CustomerId = ul.CustomerId,
			Name = ul.Name,
			Description = ul.Description
		};

		// UserListItem
		public static UserListItemDTO ToDto(this UserListItem uli) => new UserListItemDTO
		{
			Id = uli.Id,
			UserListId = uli.UserListId,
			ProductId = uli.ProductId,
			Quantity = uli.Quantity
		};

		// ShoppingCart -> ShoppingCartItemDTO requires product
		public static ShoppingCartItemDTO ToDto(this ShoppingCart sc, Product p) => new ShoppingCartItemDTO
		{
			ShoppingCartId = sc.Id,
			CustomerId = sc.CustomerId,
			ProductId = sc.ProductId,
			Product = p.ToDto(),
			Quantity = sc.Quantity
		};

		// UserListItem detail (requires product)
		public static UserListItemDetailDTO ToDetailDto(this UserListItem uli, Product p) => new UserListItemDetailDTO
		{
			Item = uli.ToDto(),
			Product = p.ToDto()
		};

		// Category
		public static CategoryDTO ToDto(this Category c) => new CategoryDTO
		{
			Id = c.Id,
			Name = c.Name,
			ParentId = c.ParentId
		};

		public static IEnumerable<CategoryDTO> ToDtos(this IEnumerable<Category> categories) => categories.Select(c => c.ToDto());

		// Filter
		public static Shared.DTO.Filters.FilterDTO ToDto(this Filter f) => new Shared.DTO.Filters.FilterDTO
		{
			Id = f.Id,
			Name = f.Name
		};

		public static IEnumerable<Shared.DTO.Filters.FilterDTO> ToDtos(this IEnumerable<Filter> filters) => filters.Select(f => f.ToDto());

		// FilterOption
		public static Shared.DTO.Filters.FilterOptionDTO ToDto(this FilterOption fo) => new Shared.DTO.Filters.FilterOptionDTO
		{
			Id = fo.Id,
			Name = fo.Name,
			FilterId = fo.FilterId,
			ParentId = fo.ParentId
		};
		public static Shared.DTO.Filters.FilterOptionDTO ToDto(this FilterOption fo, bool? hasOptions) => new Shared.DTO.Filters.FilterOptionDTO
		{
			Id = fo.Id,
			Name = fo.Name,
			FilterId = fo.FilterId,
			ParentId = fo.ParentId,
			HasOptions = hasOptions
		};


		public static IEnumerable<Shared.DTO.Filters.FilterOptionDTO> ToDtos(this IEnumerable<FilterOption> options) => options.Select(o => o.ToDto());
	}
}
