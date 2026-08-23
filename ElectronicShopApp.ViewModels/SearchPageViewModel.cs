
using ElectronicShopApp.Services.Interfaces;
using ElectronicShopApp.ViewModels.DTOs;
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
		Products,
		Categories,
		Filters
	}
	public class SearchPageViewModel : BindableObject
	{
		readonly IServiceProvider _serviceProvider;
		readonly ICommunicateWithView _communicateWithView;
		public SearchPageViewModel(ICategoriesService categoriesService, IProductsService _productsService, IFilterService filterService, IFilterOptionService filterOptionService, IServiceProvider serviceProvider, ICommunicateWithView communicateWithView)
		{
			_categoriesService = categoriesService;
			this._productsService = _productsService;
			_filterService = filterService;
			_filterOptionService = filterOptionService;
			this._serviceProvider = serviceProvider;
			this._communicateWithView = communicateWithView;
			ChangeView = new Command<string>(async (state) =>
			{
				CurrentState = (SearchPageState)int.Parse(state);
				if (CurrentState == SearchPageState.Products)
				{
					SearchCommand.Execute(null);
				}
				else if (CurrentState == SearchPageState.Categories)
				{
					if (CategoryId != null)
					{
						Categories = _allCategories.Where(c => c.ParentId == CategoryId).ToList();
					}
					else
						Categories = _allCategories.Where(c => c.ParentId == null).ToList();
				}
				else if (CurrentState == SearchPageState.Filters)
				{
					var actualFilters = await GetFilters();
					var filtersIds = Filters.Select(f => f.Id);
					if (actualFilters.Select(af => af.Id).Where(afId => !filtersIds.Any(fId => fId == afId)).Any())
					{
						Filters = actualFilters;
					}
					OnPropertyChanged(nameof(Filters));
				}
			});
            ProductClickedCommand = new Command<int>(async (productId) =>
            {
                await ProductClicked(productId);
            });

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

		public ICommand ChangeView { get; set; }
		public ICommand SearchCommand => new Command(async () =>
		{
			_productFilter.SearchPhrase = SearchText;
			_productFilter.CategoryId = CategoryId;
			_productFilter.FilterOptionsIds = Filters.SelectMany(f => GetFilterOptions(f.FilterOptions).Select(fo => fo.FilterOptionDTO.Id)).ToList();

			CurrentState = SearchPageState.Products;
			var tmp = await _productsService.GetFilteredProducts(_productFilter);
			Products = new List<ProductDTO>(tmp);
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

        private ICommand _productClickedCommand;
        public ICommand ProductClickedCommand
        {
            get => _productClickedCommand;
            set
            {
                _productClickedCommand = value;
				OnPropertyChanged();
            }
        }

		public async Task ProductClicked(int productId)
		{
			var pageViewModel = _serviceProvider.GetRequiredService<ProductPageViewModel>();

			await _communicateWithView.NavigateToProductPage(pageViewModel, productId);
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
					CurrentState = SearchPageState.Products;
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

		#region Filters
		List<FilterDTOViewModel> _filters = new List<FilterDTOViewModel>();
		public List<FilterDTOViewModel> Filters
		{
			get { return _filters; }
			set
			{
				_filters = value;
				OnPropertyChanged();
			}
		}

		private readonly IFilterService _filterService;
		private readonly IFilterOptionService _filterOptionService;

		//public async Task LoadFiltersAsync()
		//{
		//	Filters = (await _filterService.GetFilters());
		//}

		async Task<List<FilterDTOViewModel>> GetFilters()
		{
			if (CategoryId != null)
			{
				var filters = await _filterService.GetFiltersForCategory(CategoryId.Value);
				return filters.Select(f => new FilterDTOViewModel(f)).ToList();
			}
			else
			{
				var filters = await _filterService.GetFilters();
				return filters.Select(f => new FilterDTOViewModel(f)).ToList();
			}
		}


		List<FilterOptionDTOViewModel> _clickedFilterOptions = new List<FilterOptionDTOViewModel>();
		List<FilterOptionDTOViewModel> ClickedFilterOptions
		{
			get { return _clickedFilterOptions; }
			set
			{
				_clickedFilterOptions = value;
				OnPropertyChanged();
			}
		}
		public ICommand FilterClickedCommand => new Command<FilterDTOViewModel>(async (filter) =>
		{
			if (filter != null)
			{
				filter.ShowOptions = !filter.ShowOptions;
				var tmp = await _filterOptionService.GetOptionsForFilter(filter.Id);
				filter.FilterOptions = tmp.Select(fo => new FilterOptionDTOViewModel(fo)).ToList();

				FilterOptionSelected(filter.FilterOptions, filter);
			}
		});
		public ICommand FilterOptionClickCommand => new Command<FilterOptionDTOViewModel>(async (filterOption) =>
		{
			if (filterOption != null)
			{
				if (filterOption.HaveOptions.HasValue && filterOption.HaveOptions.Value)
				{
					filterOption.ShowOptions = !filterOption.ShowOptions;
					if (filterOption.ShowOptions)
					{
						filterOption.FilterOptionValues = (await _filterOptionService.GetOptionsForParentFilterOption(filterOption.FilterOptionDTO.Id))
							.Select(fo => new FilterOptionDTOViewModel(fo))
							.ToList();

						FilterOptionSelected(filterOption.FilterOptionValues, Filters.Find(f => filterOption.FilterOptionDTO.FilterId == f.Id));
					}
				}
			}
		});
		void FilterOptionSelected(List<FilterOptionDTOViewModel> filterOption, FilterDTOViewModel filter)
		{
			if (filterOption == null && filter == null)
				return;

			foreach (var item in filter.SelectedFilterOptions)
			{
				var finded = filterOption.Find(fo => item.FilterOptionDTO.Id == fo.FilterOptionDTO.Id);
				if (finded != null)
				{
					finded.IsSelected = true;
				}
			}
		}
		public List<FilterOptionDTOViewModel> GetFilterOptions(List<FilterOptionDTOViewModel> filterOptions)
		{
			var newList = filterOptions.ToList();
			foreach (var option in filterOptions)
			{
				if (option.FilterOptionValues != null)
					if (option.FilterOptionValues.Count > 0)
					{
						newList.AddRange(GetFilterOptions(option.FilterOptionValues));
					}
			}
			return newList;
		}
		public ICommand SaveFilterOptionsCommand => new Command<FilterDTOViewModel>((filter) =>
		{
			var mainFiltersOptions = filter.FilterOptions;

			filter.SelectedFilterOptions = new(GetFilterOptions(mainFiltersOptions)
				.Where(fo => fo.IsSelected));
		});

		#endregion
	}
}
