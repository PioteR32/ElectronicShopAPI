using CommunityToolkit.Maui;
using ElectronicShopApp.ViewModels.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels
{
    public interface ICheckedUserListsPopupViewModel
    {
        Task Initialize();
        Task ConfirmAsync();
        Task CancelAsync();
        string Title { get; set; }
        List<CheckedUserListDTOViewModel> CheckedUserLists { get; set; }
    }
}
