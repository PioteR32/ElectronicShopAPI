using ElectronicShopApp.ViewModels;

namespace ElectronicShopApp.Views;

public partial class SearchPage : ContentPage
{
	readonly SearchPageViewModel _searchPageViewModel;
	public SearchPage(SearchPageViewModel searchPageViewModel)
	{
		try
		{
			InitializeComponent();
			this.BindingContext = searchPageViewModel;
			_searchPageViewModel = searchPageViewModel;
		}
		catch
		{

		}
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await Task.Delay(500);
		await _searchPageViewModel.LoadCategoriesAsync();
	}
}