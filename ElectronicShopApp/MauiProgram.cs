using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using ElectronicShopApp.Services.Interfaces;
using ElectronicShopApp.ViewModels;
namespace ElectronicShopApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

#if DEBUG
			builder.Logging.AddDebug();

#endif
			// Register API services and HttpClient (use interfaces)

			// Register implementations for interfaces
			builder.Services.AddHttpClient<ICustomersService, Services.Classes.CustomersService>();
			builder.Services.AddHttpClient<IProductsService, Services.Classes.ProductsService>();
			builder.Services.AddHttpClient<IOrdersService, Services.Classes.OrdersService>();
			builder.Services.AddHttpClient<IShoppingCartService, Services.Classes.ShoppingCartService>();
			builder.Services.AddHttpClient<IUserListsService, Services.Classes.UserListsService>();
			builder.Services.AddHttpClient<ICommentService, Services.Classes.CommentsService>();

			builder.Services.AddSingleton<MainPageViewModel>();

			return builder.Build();

		

			




		}
	}
}
