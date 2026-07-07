using Microsoft.Extensions.Logging;
using ElectronicShopApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
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
			// Register API services and HttpClient
			
			builder.Services.AddHttpClient<Services.CustomersService>();
			builder.Services.AddHttpClient<Services.ProductsService>();
			builder.Services.AddHttpClient<Services.OrdersService>();
			builder.Services.AddHttpClient<Services.ShoppingCartService>();
			builder.Services.AddHttpClient<Services.UserListsService>();
			return builder.Build();

		

			




		}
	}
}
