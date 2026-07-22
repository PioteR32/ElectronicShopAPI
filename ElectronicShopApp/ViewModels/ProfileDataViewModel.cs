using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
namespace ElectronicShopApp.ViewModels
{
	public partial class ProfileDataViewModel : ObservableObject, IProfileSubViewModel
	{
		public string Title { get => "Krok 1: Dane Osobowe"; }
		public int Index { get; set; }
		public Command BackCommand { get; set; }
		[ObservableProperty]
		private string _userName;

		[RelayCommand]
		private void Clear() => UserName = string.Empty;
	}
}
