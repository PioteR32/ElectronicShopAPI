using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using ElectronicShopApp.ViewModels;

namespace ElectronicShopApp.Views.ProfilePageViews;

public partial class AddUserListPopup : Popup
{
	readonly AddUserListPopupViewModel _addUserListPopupViewModel;
	public AddUserListPopup(AddUserListPopupViewModel addUserListPopupViewModel)
	{
		_addUserListPopupViewModel = addUserListPopupViewModel;
		InitializeComponent();
		BindingContext = _addUserListPopupViewModel;

	}
}