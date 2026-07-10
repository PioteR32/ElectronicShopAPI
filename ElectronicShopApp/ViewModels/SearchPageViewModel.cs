using ElectronicShopApp.Services.Interfaces;
using Shared;
using Shared.DTO;
using Shared.DTO.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ElectronicShopApp.ViewModels
{
	public enum SearchPageState
	{
		Search,
		Categories,
		Filters
	}
	public class SearchPageViewModel : BindableObject
	{
		public SearchPageViewModel(ICategoriesService categoriesService, IProductsService _productsService)
		{
			_categoriesService = categoriesService;
			this._productsService = _productsService;
		}
		readonly ProductFilterDTO _productFilter = new ProductFilterDTO();
		#region ContentPageProperties
		SearchPageState _currentState = SearchPageState.Categories;
		public SearchPageState CurrentState 
		{ 
			get { return _currentState; }
			set 
			{ 
				_currentState = value;
				OnPropertyChanged();
			}
		}

		public string? SearchText
		{
			get { return _productFilter.SearchPhrase; }
			set
			{
				_productFilter.SearchPhrase = value;
			}
		}

		public int? CategoryId
		{
			get { return _productFilter.CategoryId; }
			set
			{
				_productFilter.CategoryId = value;
			}
		}

		//public decimal? MinPrice
		//{
		//	get { return _productFilter.MinPrice; }
		//	set
		//	{
		//	}
		//}
		//public decimal? MaxPrice
		//{
		//	get { return _productFilter.MaxPrice; }
		//	set
		//	{
		//	}
		//}
		//public bool? IsInStock
		//{
		//	get { return _productFilter.IsInStock; }
		//	set
		//	{
		//	}
		//}
		//public int PageIndex
		//{
		//	get { return _productFilter.PageIndex; }
		//	set
		//	{
		//	}
		//}
		//public int PageSize
		//{
		//	get { return _productFilter.PageSize; }
		//	set
		//	{
		//	}
		//}

		//public OrderByOptions OrderByOptions
		//{
		//	get { return _productFilter.OrderByOptions; }
		//	set
		//	{
		//	}
		//}

		//int? _selectedCategoryId = null;




		public ICommand SearchCommand => new Command(async () =>
		{
			_productFilter.SearchPhrase = SearchText;
			_productFilter.CategoryId = CategoryId;
			await _productsService.GetFilteredProducts(_productFilter);
		});
		#endregion

		#region Products

		private readonly IProductsService _productsService;

		List<ProductDTO> _products;
		public List<ProductDTO> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				OnPropertyChanged();
			}
		}

		#endregion
		#region Categories

		private readonly ICategoriesService _categoriesService;

		List<CategoryDTO> _allCategories;

		List<CategoryDTO> _categories;
		public List<CategoryDTO> Categories
		{
			get { return _categories; }
			set
			{
				_categories = value;
				OnPropertyChanged();
			}
		}
		public ICommand CategoryClickCommand => new Command<CategoryDTO>(async (category) =>
		{
			if (category != null)
			{
				CategoryId = category.Id;
				Categories = _allCategories.Where(c => c.ParentId == CategoryId).ToList();
				if (Categories.Count == 0)
				{
					Products = new List<ProductDTO>(await _productsService.GetFilteredProducts(_productFilter));
					CurrentState = SearchPageState.Search;
				}
			}
		});
		public ICommand CategoryBackCommand => new Command(async () =>
		{
			if (CategoryId != null)
			{
				CategoryId = _allCategories.Find(c => c.Id == CategoryId)?.ParentId;
				Categories = _allCategories.Where(c => c.ParentId == CategoryId).ToList();
			}
		});
		public async Task LoadCategoriesAsync()
		{
			_allCategories = (await _categoriesService.GetCategories()).ToList();
			Categories = _allCategories.Where(c => c.ParentId == CategoryId).ToList();
		}

		#endregion
	}
}
