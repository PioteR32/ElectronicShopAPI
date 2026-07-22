using CommunityToolkit;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace ElectronicShopApp.ViewModels
{
	public partial class ProfilePageViewModel:ObservableObject
	{
		readonly ICustomersService _customerService;
		public ProfilePageViewModel(ICustomersService customersService)
		{
			_customerService = customersService;
			LogInCommand = new Command(async () =>
			{
				await _customerService.LogIn(EntryName, EntryPassword);
				OnPropertyChanged(nameof(IsUserLoggedIn));
				OnAppearingCommand.Execute(null);

			});
			OnAppearingCommand = new Command(async () =>
			{
				Profile = await _customerService.GetProfile();

			});
			SubViewModels = new IProfileSubViewModel[]
			{
				new ProfileDataViewModel(),
				new ProfileListsViewModel()
			};
			for(int i =0; i < SubViewModels.Length; i++)
			{
				var subViewModel = SubViewModels[i];
				subViewModel.Index = i;
				subViewModel.BackCommand = new Command(() => OnBackButtonClicked());
			}
		}
		CustomerDTO? _profile;
		CustomerDTO? Profile
		{
			get { return _profile; }
			set
			{
				_profile = value;
				OnPropertyChanged();
			}
		}
		
		public bool IsUserLoggedIn
		{
			get => Global.authorizeToken != null && Global.authorizeToken != "";
		}


		string entryName = string.Empty;
		public string EntryName
		{
			get => entryName;
			set
			{
				entryName = value;
				OnPropertyChanged(nameof(EntryName));
			}
		}
		string entryPassword = string.Empty;
		public string EntryPassword
		{
			get => entryPassword;
			set
			{
				entryPassword = value;
				OnPropertyChanged();
			}
		}
		public ICommand LogInCommand { get; set;  }
		public ICommand LogOutCommand { get; set; }
		public ICommand OnAppearingCommand { get; set; }


		#region UserAbilities
		[ObservableProperty]
		private bool _isUserInAbility  = false;
		[ObservableProperty]
		private  IProfileSubViewModel[] _subViewModels;

		// Tworzy automatycznie właściwość CurrentSubViewModel
		[ObservableProperty]
		private IProfileSubViewModel _currentSubViewModel;

		// Collection wrapper so BindableLayout (which expects a collection) can render the current subview
		// Make settable to avoid readonly-assignment errors from source generators or serializers
		public  ObservableCollection<IProfileSubViewModel> CurrentSubViewModelCollection { get; set; } = new();

		[RelayCommand]
		private void OnButtonClickedView(int index)
		{
			if (index >= 0 && index < _subViewModels.Length)
			{
				IsUserInAbility = true;
				CurrentSubViewModel = _subViewModels[index];
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
