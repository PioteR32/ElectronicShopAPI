using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels
{
	public interface IProfileSubViewModel
	{
		string Title { get; }
		int Index { get; set; }

		Command BackCommand { get; set; }
		Command AppearingCommand { get; set; }
	}
}
