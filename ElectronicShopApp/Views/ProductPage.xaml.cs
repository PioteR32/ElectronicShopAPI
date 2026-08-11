using ElectronicShopApp.ViewModels;

namespace ElectronicShopApp.Views;

public partial class ProductPage : ContentPage
{

	public ProductPage(ProductPageViewModel viewModel, int productId)
	{
		InitializeComponent();
		BindingContext = viewModel;
        viewModel.LoadProductSpecifications(productId);
    }
}