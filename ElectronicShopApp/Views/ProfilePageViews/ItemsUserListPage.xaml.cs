using ElectronicShopApp.ViewModels;
using Shared.DTO;

namespace ElectronicShopApp.Views.ProfilePageViews;

public partial class ItemsUserListPage : ContentPage
{

    public ItemsUserListPage(UserListDTO userListDTO , ItemsUserListViewModel viewModel)
    {
        InitializeComponent();
        viewModel.AppearingCommand.Execute(userListDTO);
        BindingContext = viewModel;
    }
}