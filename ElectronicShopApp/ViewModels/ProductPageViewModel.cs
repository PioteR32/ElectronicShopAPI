using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicShopApp.Services.Interfaces;
using ElectronicShopApp.ViewModels.DTOs;
using ElectronicShopApp.Views.ProfilePageViews;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels
{
    public partial class ProductPageViewModel : ObservableObject
    {
        readonly IProductsService _productsService;
        readonly IServiceProvider _serviceProvider;
        readonly IUserListsService _userListsService;

        [ObservableProperty] private int _productID;
        [ObservableProperty] private ProductDTO _product;

        [ObservableProperty] private List<ProductSpecificationDTO> _productSpecifications;

        public ProductPageViewModel(IProductsService productsService,IUserListsService userListsService, IServiceProvider serviceProvider)
        {
            _productsService = productsService;
            _userListsService = userListsService;
            _serviceProvider = serviceProvider;
            ProductSpecifications = new List<ProductSpecificationDTO>();
        }

        public async Task LoadProductSpecifications(int productId)
        {
            ProductID = productId;
            if (ProductID > 0)
            {
                ProductSpecifications = await _productsService.GetProductSpecsById(ProductID);
                Product = await _productsService.GetProductById(ProductID);
            }
        }

        [RelayCommand]
        public async Task MenageProductOnUserLists()
        {
            var newCheckedList = await ShowMenageUserListPopupAsync();
            if (newCheckedList != null) 
                {
                    await _userListsService.MenageUserListsProduct(ProductID,
                        newCheckedList.Select(x =>
                            new CheckedUserListDTO(x.UserListDto, x.IsChecked)));
                }
           
        }

        private async Task<List<CheckedUserListDTOViewModel>> ShowMenageUserListPopupAsync()
        {
            if (Shell.Current?.CurrentPage is Page currentPage)
            {

                var viewModel = (MenageProductsOnUserListsViewModel)_serviceProvider.GetRequiredKeyedService<ICheckedUserListsPopupViewModel>("MenageProductsOnUserListsViewModel");
                viewModel.productId = ProductID;
                viewModel.InitializeCommand.Execute(null);
                var popup = new RemoveUserListsPopup(viewModel);
                var result = await Shell.Current.CurrentPage.ShowPopupAsync<List<CheckedUserListDTOViewModel>>(popup);

                if (result != null && !result.WasDismissedByTappingOutsideOfPopup)
                {
                    return result.Result;
                }
                else
                {
                    return null;
                }

            }
            return null;
        }
    }
}
