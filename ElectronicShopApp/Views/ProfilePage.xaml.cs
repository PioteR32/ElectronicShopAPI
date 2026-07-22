
using ElectronicShopApp.ViewModels;

namespace ElectronicShopApp.Views;

public partial class ProfilePage : ContentPage
{
	readonly ProfilePageViewModel _viewModel;
	public ProfilePage(ProfilePageViewModel pageViewModel)
	{
		_viewModel = pageViewModel;
		InitializeComponent();
		this.BindingContext = _viewModel;
		
	}
}