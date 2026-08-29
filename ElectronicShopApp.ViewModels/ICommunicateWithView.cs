using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.DTO;

namespace ElectronicShopApp.ViewModels
{
    public interface ICommunicateWithView
    {
        public Task NavigateToProductPage(ProductPageViewModel viewModel, int productId);
        public Task NavigateToItemsUserListPage(UserListDTO userListDTO);

        public Task<IPopupResult<T>> NavigateRemoveUserListsPopup<T>(ICheckedUserListsPopupViewModel viewModel)
            where T : class;
    }
}
