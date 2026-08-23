using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicShopApp.Services;
using ElectronicShopApp.Services.Classes;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
namespace ElectronicShopApp.ViewModels
{
	public partial class ProfileDataViewModel : ObservableObject, IProfileSubViewModel
	{
		private readonly IPopupService _addUserListPopup;
		private readonly ICustomersService customersService;

		public ProfileDataViewModel(ICustomersService customersService)
		{
			this.customersService = customersService;
			AppearingCommand = new Command(async () =>
			{
				Profile = await customersService.GetProfile();
			});
		}
		
		public string Title { get => "Krok 1: Dane Osobowe"; }
		public int Index { get; set; }
		public Command BackCommand { get; set; }
		public Command AppearingCommand { get; set; }

		[ObservableProperty]
		private string _userName;

		[ObservableProperty]
		CustomerDTO _profile;

		[RelayCommand]
		private void Clear() => UserName = string.Empty;


		[ObservableProperty]
		private bool _isNameChanging = false;

		[RelayCommand]
		private async void ChangeName()
		{
			if (_isNameChanging)
			{
				await customersService.ChangeUserNames(Profile.FirstName, Profile.LastName);
				Profile = await customersService.GetProfile();
				OnPropertyChanged(nameof(Profile));
			}
			IsNameChanging = !IsNameChanging;
		}

		[ObservableProperty]
		private bool _isPasswordChanging = false;

		[ObservableProperty]
		private string _passwordToCheck;

		[ObservableProperty]
		private bool _isCorrectPassword;

		[RelayCommand]
		private async void ChangePassword()
		{
			if (IsCorrectPassword)
			{
				Profile = await customersService.ChangeUserPassword(_passwordToCheck);
				IsCorrectPassword = false;
				PasswordToCheck = "";
				
			}
			IsPasswordChanging = !IsPasswordChanging;
		}

		[RelayCommand]
		private async void CheckPassword()
		{
			try
			{
				if (PasswordToCheck == Profile.Password)
				{
					IsCorrectPassword = true;
					_passwordToCheck = "";
				}
			}
			catch (Exception ex)
			{

			}
		}

	}
}
