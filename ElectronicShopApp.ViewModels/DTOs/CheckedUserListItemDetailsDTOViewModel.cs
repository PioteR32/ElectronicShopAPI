using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Shared.DTO;

namespace ElectronicShopApp.ViewModels.DTOs
{
    public partial class CheckedUserListItemDetailsDTOViewModel: ObservableObject  
    {
        private UserListItemDetailDTO _userListItemDTO;
        public UserListItemDetailDTO UserListItemDTO
        {
            get => _userListItemDTO;
            
        }

        [ObservableProperty] 
        private bool _isChecked;

        public CheckedUserListItemDetailsDTOViewModel(UserListItemDetailDTO userListItemDTO, bool isChecked)
        {
            _userListItemDTO = userListItemDTO;
            _isChecked = isChecked;
        }


    }
}
