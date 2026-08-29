using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using ElectronicShopApp.ViewModels;
using ElectronicShopApp.Views;
using ElectronicShopApp.Views.ProfilePageViews;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp
{
    public class CommunicateWithViewModel : ICommunicateWithView
    {
        readonly IServiceProvider _serviceProvider;
        public CommunicateWithViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task NavigateToItemsUserListPage(UserListDTO userListDTO)
        {
           
            await Shell.Current.CurrentPage.Navigation.PushAsync(new ItemsUserListPage(userListDTO , _serviceProvider.GetRequiredService<ItemsUserListViewModel>()));
        }
        public async Task NavigateToProductPage(ProductPageViewModel viewModel, int productId)
        {
            await Shell.Current.CurrentPage.Navigation.PushAsync(new ProductPage(viewModel, productId));
        }

        public async Task<IPopupResult<T>> NavigateRemoveUserListsPopup<T>(ICheckedUserListsPopupViewModel viewModel) where T:class
        {
            // Show the popup and return result if supported. If not available, return null to indicate dismissal.
            
            try
            {
                if (Shell.Current?.CurrentPage is Page currentPage)
                {
                    var popup = new RemoveUserListsPopup(viewModel);

                    var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);
                    return (IPopupResult<T>)result;
                }
            }
            catch
            {
                // ignore
            }
            return await Task.FromResult<IPopupResult<T>>(null);
        }
    }
}
