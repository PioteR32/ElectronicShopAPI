using CommunityToolkit;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ElectronicShopApp.Services;
namespace ElectronicShopApp.ViewModels
{
	public partial class ProfilePageViewModel:ObservableObject
	{
		readonly ICustomersService _customerService;
		readonly IUserListsService _userListsService;
		readonly IPopupService _addUserListPopup;
		readonly IServiceProvider _serviceProvider;
		public ProfilePageViewModel(ICustomersService customersService, IUserListsService userListsService, IPopupService popupService, IServiceProvider serviceProvider)
		{
			_customerService = customersService;
			_userListsService = userListsService;
			_addUserListPopup = popupService;
			_serviceProvider = serviceProvider;
			//LogInCommand = new Command(async () =>
			//{
			//	await _customerService.LogIn(EntryName, EntryPassword);
			//	OnPropertyChanged(nameof(IsUserLoggedIn));
			//});
			
			SubViewModels = new IProfileSubViewModel[]
			{
				new ProfileDataViewModel(_customerService),
				new ProfileListsViewModel(_userListsService,_addUserListPopup,_serviceProvider,_serviceProvider.GetRequiredService<ICommunicateWithView>())
			};

			for(int i =0; i < SubViewModels.Length; i++)
			{
				var subViewModel = SubViewModels[i];
				subViewModel.Index = i;
				subViewModel.BackCommand = new Command(() => OnBackButtonClicked());
			}
		}
		
		
		public bool IsUserLoggedIn
		{
			get => Global.authorizeToken != null && Global.authorizeToken != "";
		}

		[ObservableProperty]
		string _entryName = string.Empty;

		[ObservableProperty]
		string _entryPassword = string.Empty;

		public ICommand LogOutCommand { get; set; }
		


		#region UserAbilities
		[ObservableProperty]
		private bool _isUserInAbility  = false;
		[ObservableProperty]
		private  IProfileSubViewModel[] _subViewModels;
		[ObservableProperty]
		private IProfileSubViewModel _currentSubViewModel;

		// Collection wrapper so BindableLayout (which expects a collection) can render the current subview
		// Make settable to avoid readonly-assignment errors from source generators or serializers
		public  ObservableCollection<IProfileSubViewModel> CurrentSubViewModelCollection { get; set; } = new();

		[RelayCommand]
		public async void LogIn()
		{
            await _customerService.LogIn(EntryName, EntryPassword);
            OnPropertyChanged(nameof(IsUserLoggedIn));
        }

		[RelayCommand]
		private void OnButtonClickedView(int index)
		{
			if (index >= 0 && index < _subViewModels.Length)
			{
				IsUserInAbility = true;
				CurrentSubViewModel = _subViewModels[index];
				CurrentSubViewModel.AppearingCommand.Execute(this);
			}
		}
		
		[RelayCommand]
		private void OnBackButtonClicked()
		{
			IsUserInAbility = false;
		}

		// Called by CommunityToolkit when CurrentSubViewModel changes
		partial void OnCurrentSubViewModelChanged(IProfileSubViewModel value)
		{
			CurrentSubViewModelCollection.Clear();
			if (value != null)
			{
				CurrentSubViewModelCollection.Add(value);
			}
		}
		#endregion
	}
}
