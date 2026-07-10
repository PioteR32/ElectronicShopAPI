

using ElectronicShopApp.ViewModels;
using Shared.DTO.Filters;

namespace ElectronicShopApp.Views
{
	public partial class MainPage : ContentPage
	{
		private readonly MainPageViewModel _viewModel;

		// 1. Wstrzykujemy ViewModel przez konstruktor (Dependency Injection w MAUI)
		public MainPage(MainPageViewModel viewModel)
		{
			try
			{
				InitializeComponent();

				// 2. Przypisujemy ViewModel jako BindingContext, aby XAML widział właściwość "Products"
				_viewModel = viewModel;
				BindingContext = _viewModel;
			}
			catch 
			{ 
			}
		}

		// 3. Nadpisujemy metodę cyklu życia strony - wywołuje się za każdym razem, gdy ekran się pojawia
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			Thread.Sleep(1000); // Dodajemy opóźnienie, aby upewnić się, że UI jest gotowe do wyświetlenia danych)
			var defaultFilter = new ProductFilterDTO
			{
				FilterOptionsIds = new List<int>() 
			};

			await _viewModel.LoadBestRatedProducts();
		}
	}
}
