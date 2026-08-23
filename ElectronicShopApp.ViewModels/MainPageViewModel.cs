using System;
using System.Collections.Generic;
using System.Text;
using ElectronicShopApp.Services.Interfaces;
using Shared;
using Shared.DTO;
using Shared.DTO.Filters;
namespace ElectronicShopApp.ViewModels
{
	public class MainPageViewModel:BindableObject
	{
		readonly IProductsService _productService;
		public MainPageViewModel(IProductsService productService)
		{
			_productService = productService;
			
		}
		ProductDTO[] bestRatedProducts;
		public ProductDTO[] BestRatedProducts
		{ 
			get => bestRatedProducts;
			set
			{
				bestRatedProducts = value;
				OnPropertyChanged();
			}
		}

		public async Task LoadBestRatedProducts()
		{
			var filter = new ProductFilterDTO
			{
				OrderByOptions = OrderByOptions.SortByRatingDesc,
				PageIndex = 1,
				PageSize = 10
			};
			var products = await _productService.GetFilteredProducts(filter);
			BestRatedProducts = products.ToArray();
		}

	}
}
