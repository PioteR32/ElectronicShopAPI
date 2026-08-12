using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;

namespace ElectronicShopApp.ViewModels
{
    public partial class ProductPageViewModel : ObservableObject
    {
        readonly IProductsService _productsService;

        [ObservableProperty] private int _productID;
        [ObservableProperty] private ProductDTO _product;

        [ObservableProperty] private List<ProductSpecificationDTO> _productSpecifications;

        public ProductPageViewModel(IProductsService productsService)
        {
            _productsService = productsService;
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
    }
}
