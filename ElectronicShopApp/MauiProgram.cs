using CommunityToolkit.Maui;
using ElectronicShopApp.Services.Interfaces;
using ElectronicShopApp.ViewModels;
using ElectronicShopApp.Views.ProfilePageViews;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
namespace ElectronicShopApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
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
			builder.Services.AddHttpClient<ICategoriesService, Services.Classes.CategoriesService>();
			builder.Services.AddHttpClient<IFilterService, Services.Classes.FiltersService>();
			builder.Services.AddHttpClient<IFilterOptionService, Services.Classes.FilterOptionsService>();

			builder.Services.AddTransientPopup<AddUserListPopup, AddUserListPopupViewModel>();

			builder.Services.AddSingleton<ElectronicShopApp.Views.MainPage>();
			builder.Services.AddSingleton<ElectronicShopApp.Views.SearchPage>();
			builder.Services.AddSingleton<ElectronicShopApp.Views.ProfilePage>();
			builder.Services.AddTransient<ElectronicShopApp.Views.ProfilePageViews.AddUserListPopup>();

			builder.Services.AddSingleton<MainPageViewModel>();

			builder.Services.AddSingleton<SearchPageViewModel>();

			builder.Services.AddSingleton<ProfilePageViewModel>();
			builder.Services.AddTransient<ProfileDataViewModel>();
			builder.Services.AddTransient<ProfileListsViewModel>();
			builder.Services.AddTransient<AddUserListPopupViewModel>();
			

			return builder.Build();

		

			




		}
	}
}
