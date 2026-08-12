using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicShopApp.Services.Interfaces;
using ElectronicShopApp.Views.ProfilePageViews;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Maui.Extensions;
using ElectronicShopApp.ViewModels.DTOs;

namespace ElectronicShopApp.ViewModels
{
	public partial class ProfileListsViewModel : ObservableObject, IProfileSubViewModel
	{
		readonly IUserListsService _userListsService;
		private readonly IPopupService _addUserListPopup;
		private readonly IServiceProvider _serviceProvider;
		public ProfileListsViewModel(IUserListsService userListsService, IPopupService addUserListPopup,IServiceProvider serviceProvider)
		{
			_userListsService = userListsService;
			_addUserListPopup = addUserListPopup;
            _serviceProvider = serviceProvider;
            AppearingCommand = new Command(async () =>
			{
				var tmp = await _userListsService.GetUserLists();
				 UserLists = tmp.ToList();
			});


		}

		public string Title { get => "Krok 2: Adres Dostawy"; }
		public int Index { get; set; }

		[ObservableProperty]
		private string _city;

		[ObservableProperty]
		private UserListDTO _newUserListDTO;

		[ObservableProperty]
		private List<UserListDTO> _userLists;


		public Command BackCommand { get; set; }
		public Command AppearingCommand { get; set; }


		[RelayCommand]
		public async Task AddList()
		{
			var newList = await ShowAddListPopupAsync();
			if (newList != null && newList.Name != null && newList.Description != null)
                if (await _userListsService.AddUserList(newList))
                {
                    UserLists = (await _userListsService.GetUserLists()).ToList();
                }
			
		}
        [RelayCommand]
		public async Task RemoveList()
		{
			var newList = await ShowRemoveListPopupAsync();
			if(newList == null) return;
			var idToRemoveList = newList.Where(l => l.IsChecked )
                .Select(l => l.UserListDto.Id )
                .ToList();
			if (idToRemoveList.Any())
			{
				await _userListsService.RemoveUserLists(idToRemoveList);
			}

            AppearingCommand.Execute(null);

        }

		
		private async Task<UserListDTO> ShowAddListPopupAsync()
		{
			if (Shell.Current?.CurrentPage is Page currentPage)
			{
				// Otwiera popup i czeka na odpowiedź (bool) z MyCustomPopupViewModel
				var result = await _addUserListPopup.ShowPopupAsync<AddUserListPopupViewModel, UserListDTO>(currentPage);

				if (result != null && !result.WasDismissedByTappingOutsideOfPopup)
				{
					return result.Result;
				}
				else
				{
					return null;
				}
				
			}
			return null;
		}
        private async Task<List<CheckedUserListDTOViewModel>> ShowRemoveListPopupAsync()
		{
			if (Shell.Current?.CurrentPage is Page currentPage)
			{
                
                var viewModel = _serviceProvider.GetRequiredKeyedService<ICheckedUserListsPopupViewModel>("RemoveUserListsPopupViewModel");
                var popup = new RemoveUserListsPopup(viewModel);
                var result =  await Shell.Current.CurrentPage.ShowPopupAsync<List<CheckedUserListDTOViewModel>>(popup);

				if (result != null && !result.WasDismissedByTappingOutsideOfPopup)
				{ 
					return result.Result;
				}
				else
				{
					return null;
				}
				
			}
			return null;
		}

	}
}
