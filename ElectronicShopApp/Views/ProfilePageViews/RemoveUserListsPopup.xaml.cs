using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicShopApp.UtilsMaui;
using ElectronicShopApp.ViewModels;
namespace ElectronicShopApp.Views.ProfilePageViews;

public partial class RemoveUserListsPopup : Popup
{
    public RemoveUserListsPopup(ICheckedUserListsPopupViewModel viewModel)
    {
        InitializeComponent();

        this.AttachAutoResizing(0.8,0.3);

        BindingContext = viewModel;

    }
}