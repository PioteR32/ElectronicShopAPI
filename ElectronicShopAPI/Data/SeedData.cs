using ElectronicShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAPI.Data;

public static class SeedData
{
    public static async Task InitializeAsync(DataContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Products.AnyAsync())
        {
            return;
        }

        var categories = new[]
        {
            new Category { Id = 1, Name = "Laptopy" },
            new Category { Id = 2, Name = "Smartfony" },
            new Category { Id = 3, Name = "Akcesoria" }
        };

        var filters = new[]
        {
            new Filter { Id = 1, Name = "Marka" },
            new Filter { Id = 2, Name = "Kolor" }
        };

        var filterOptions = new[]
        {
            new FilterOption { Id = 1, FilterId = 1, Name = "Lenovo" },
            new FilterOption { Id = 2, FilterId = 1, Name = "Samsung" },
            new FilterOption { Id = 3, FilterId = 2, Name = "Czarny" },
            new FilterOption { Id = 4, FilterId = 2, Name = "Biały" }
        };

        var products = new[]
        {
            new Product
            {
                Id = 1,
                Name = "Lenovo IdeaPad 5",
                Description = "Laptop do pracy i nauki.",
                Price = 2999.99m,
                NumOnStock = 10,
                Categories = new List<Category> { categories[0] }
            },
            new Product
            {
                Id = 2,
                Name = "Samsung Galaxy A55",
                Description = "Nowoczesny smartfon z dużym ekranem.",
                Price = 1699.00m,
                NumOnStock = 20,
                Categories = new List<Category> { categories[1] }
            },
            new Product
            {
                Id = 3,
                Name = "Mysz bezprzewodowa",
                Description = "Ergonomiczna mysz do codziennej pracy.",
                Price = 89.99m,
                NumOnStock = 50,
                Categories = new List<Category> { categories[2] }
            }
        };

        var customer = new Customer
        {
            Id = 1,
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@example.com",
            PhoneNumber = "500600700",
            Password = "password"
        };

        var towns = new[]
        {
            new Town { Id = 1, Name = "Warszawa" },
            new Town { Id = 2, Name = "Kraków" }
        };

        var categoriesFilters = new[]
        {
            new CategoriesFilter { Id = 1, CategoryId = 1, FilterId = 1 },
            new CategoriesFilter { Id = 2, CategoryId = 1, FilterId = 2 },
            new CategoriesFilter { Id = 3, CategoryId = 2, FilterId = 1 },
            new CategoriesFilter { Id = 4, CategoryId = 2, FilterId = 2 }
        };

        var comments = new[]
        {
            new Comment
            {
                Id = 1,
                Comment1 = "Bardzo dobry laptop do codziennej pracy.",
                Rating = 5,
                ProductId = 1,
                CustomerId = 1
            },
            new Comment
            {
                Id = 2,
                Comment1 = "Telefon działa szybko i ma dobry aparat.",
                Rating = 4,
                ProductId = 2,
                CustomerId = 1
            }
        };

        var orders = new[]
        {
            new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.UtcNow.AddDays(-2),
                TotalAmount = 2999.99m,
                Email = customer.Email,
                Street = "Długa",
                House = "10/2",
                TownId = 1
            }
        };

        var orderItems = new[]
        {
            new OrderItem
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                Price = 2999.99m
            }
        };

        var shoppingCarts = new[]
        {
            new ShoppingCart
            {
                Id = 1,
                CustomerId = 1,
                ProductId = 3,
                Quantity = 2
            }
        };

        var userLists = new[]
        {
            new UserList
            {
                Id = 1,
                CustomerId = 1,
                Name = "Obserwowane produkty",
                Description = "Produkty zapisane do późniejszego zakupu."
            }
        };

        var userListItems = new[]
        {
            new UserListItem
            {
                Id = 1,
                UserListId = 1,
                ProductId = 2,
                Quantity = 1
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.Filters.AddRangeAsync(filters);
        await context.FilterOptions.AddRangeAsync(filterOptions);
        await context.Products.AddRangeAsync(products);
        await context.Customers.AddAsync(customer);
        await context.Towns.AddRangeAsync(towns);
        await context.CategoriesFilters.AddRangeAsync(categoriesFilters);
        await context.Comments.AddRangeAsync(comments);
        await context.Orders.AddRangeAsync(orders);
        await context.OrderItems.AddRangeAsync(orderItems);
        await context.ShoppingCarts.AddRangeAsync(shoppingCarts);
        await context.UserLists.AddRangeAsync(userLists);
        await context.UserListItems.AddRangeAsync(userListItems);
        await context.SaveChangesAsync();

        await context.ProductFilterOptions.AddRangeAsync(
            new ProductFilterOption { Id = 1, ProductId = 1, FilterOptionId = 1 },
            new ProductFilterOption { Id = 2, ProductId = 1, FilterOptionId = 3 },
            new ProductFilterOption { Id = 3, ProductId = 2, FilterOptionId = 2 },
            new ProductFilterOption { Id = 4, ProductId = 2, FilterOptionId = 4 },
            new ProductFilterOption { Id = 5, ProductId = 3, FilterOptionId = 3 });

        await context.SaveChangesAsync();
    }
}
