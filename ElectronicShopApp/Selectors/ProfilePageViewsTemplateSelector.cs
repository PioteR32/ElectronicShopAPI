using System;
using System.Collections.Generic;
using System.Text;
using ElectronicShopApp.ViewModels;
namespace ElectronicShopApp.Selectors
{
    class ProfilePageViewsTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ProfileDataViewModel { get; set; }
		public DataTemplate ProfileListsViewModel { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			return item switch
			{
				ElectronicShopApp.ViewModels.ProfileDataViewModel _ => ProfileDataViewModel,
				ElectronicShopApp.ViewModels.ProfileListsViewModel _ => ProfileListsViewModel,
				_ => null
			};
		}
	}
}
