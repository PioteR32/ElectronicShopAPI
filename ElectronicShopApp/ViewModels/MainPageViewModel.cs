using System;
using System.Collections.Generic;
using System.Text;
using ElectronicShopApp.Services.Interfaces;
namespace ElectronicShopApp.ViewModels
{
	internal class MainPageViewModel
	{
		readonly IProductsService _productService;
		public MainPageViewModel(IProductsService productService)
		{
			_productService = productService;
		}
	}
}
