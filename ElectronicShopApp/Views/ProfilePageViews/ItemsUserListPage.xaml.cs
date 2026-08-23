using ElectronicShopApp.ViewModels;

namespace ElectronicShopApp.Views.ProfilePageViews;

public partial class ItemsUserListPage : ContentPage
{
	public ItemsUserListPage(ItemsUserListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}