using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels
{
	public partial class AddUserListPopupViewModel : ObservableObject
	{
		private readonly IPopupService _popupService;

		public AddUserListPopupViewModel(IPopupService popupService)
		{
			_popupService = popupService;
		}

		[ObservableProperty]
		private string _title = "Add List";

		[ObservableProperty]
		private string _name;

		[ObservableProperty]
		private string _description;
		
		

		[RelayCommand]
		public async Task ConfirmAsync()
		{
			if (Shell.Current?.CurrentPage is Page currentPage)
			{
				// Przekazujemy stronę ORAZ wynik (true)
				await _popupService.ClosePopupAsync(currentPage, new UserListDTO() { Description=_description,Name=_name});
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
