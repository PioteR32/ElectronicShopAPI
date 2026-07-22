using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels
{
	public partial class ProfileListsViewModel : ObservableObject, IProfileSubViewModel
	{
		public string Title { get => "Krok 2: Adres Dostawy"; }
		public int Index { get; set; }
		public Command BackCommand { get; set; }
		

		[ObservableProperty]
		private string _city;
	}
}
