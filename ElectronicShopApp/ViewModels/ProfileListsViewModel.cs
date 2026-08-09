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

namespace ElectronicShopApp.ViewModels
{
	public partial class ProfileListsViewModel : ObservableObject, IProfileSubViewModel
	{
		readonly IUserListsService _userListsService;
		private readonly IPopupService _addUserListPopup;
		public ProfileListsViewModel(IUserListsService userListsService, IPopupService addUserListPopup)
		{
			_userListsService = userListsService;
			_addUserListPopup = addUserListPopup;
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
			var newList = await ShowCustomPopupAsync();
			if (newList != null && newList.Name != null && newList.Description != null)
				_userListsService.AddUserList(newList);
			//if (await _userListsService.AddUserList(new UserListDTO() { Name = "Testowa", Description = "Testowa" }))
			//{
			//	UserLists = (await _userListsService.GetUserLists()).ToList();
			//}
		}

		
		private async Task<UserListDTO> ShowCustomPopupAsync()
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

	}
}
