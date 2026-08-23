using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicShopApp.Services.Interfaces;
using ElectronicShopApp.ViewModels.DTOs;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels
{
    public partial class MenageProductsOnUserListsViewModel : ObservableObject, ICheckedUserListsPopupViewModel
    {
        private readonly IPopupService _popupService;
        private readonly IUserListsService _userListsService;

        public MenageProductsOnUserListsViewModel(IPopupService popupService, IUserListsService userListsService)
        {
            _popupService = popupService;
            _userListsService = userListsService;

        }

        public int productId;
        [RelayCommand]
        public async Task Initialize()
        {
            var userLists = await _userListsService.GetCheckedUserLists(productId);
            List<CheckedUserListDTOViewModel> userListsViewModels = new List<CheckedUserListDTOViewModel>();
            foreach (var userListDto in userLists)
            {
                userListsViewModels.Add(new CheckedUserListDTOViewModel(userListDto));
            }
            CheckedUserLists = userListsViewModels;
        }
        [ObservableProperty]
        private List<CheckedUserListDTOViewModel> _checkedUserLists = new List<CheckedUserListDTOViewModel>();

        [ObservableProperty]
        private string _title = "Products lists";
        [RelayCommand]
        public async Task ConfirmAsync()
        {
            if (Shell.Current?.CurrentPage is Page currentPage)
            {
                // Przekazujemy stronę ORAZ wynik (true)
                await _popupService.ClosePopupAsync(currentPage, CheckedUserLists);
            }
        }

        [RelayCommand]
        public async Task CancelAsync()
        {
            if (Shell.Current?.CurrentPage is Page currentPage)
            {
                // Przekazujemy stronę ORAZ wynik (true)
                await _popupService.ClosePopupAsync<UserListDTO>(currentPage, null);
            }
        }

      
    }
}
