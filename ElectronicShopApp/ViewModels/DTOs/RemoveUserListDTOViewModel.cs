using CommunityToolkit.Mvvm.ComponentModel;
using Shared.DTO;
using Shared.DTO.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels.DTOs
{
    public partial class RemoveUserListDTOViewModel : ObservableObject
    {
        public RemoveUserListDTOViewModel(UserListDTO userListDto)
        {
            _userListDto = userListDto;
        }
        [ObservableProperty]
        private UserListDTO _userListDto;

        [ObservableProperty]
        private bool _isToRemove = false;

    }
}
