using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicShopApp.ViewModels.DTOs;

namespace ElectronicShopApp.ViewModels
{
    public class MenageProductsOnUserListsViewModel : ObservableObject, ICheckedUserListsPopupViewModel
    {
        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public Task ConfirmAsync()
        {
            throw new NotImplementedException();
        }

        public Task CancelAsync()
        {
            throw new NotImplementedException();
        }

        public string Title { get; set; }
        public List<CheckedUserListDTOViewModel> CheckedUserLists { get; set; }
    }
}
