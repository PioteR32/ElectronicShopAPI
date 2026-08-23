using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.ViewModels
{
    public interface ICommunicateWithView
    {
        public Task NavigateToProductPage(ProductPageViewModel viewModel, int productId);
        public Task NavigateToItemsUserListPage();

        public Task<T> NavigateRemoveUserListsPopup<T>(ICheckedUserListsPopupViewModel viewModel,Type resultType) where T : class;
    }
}
