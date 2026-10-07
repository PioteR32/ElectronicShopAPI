            using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ElectronicShopApp.Services;
using ElectronicShopApp.ViewModels.DTOs;
using IUserListItemService = ElectronicShopApp.Services.Interfaces.IUserListItemService;

namespace ElectronicShopApp.ViewModels
{
    public partial class ItemsUserListViewModel : ObservableObject
    {
        private readonly IUserListsService _userListsService;
        private readonly IProductsService _productsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserListItemService _userListItemService;
        private readonly IUserListService _userListService;
        private readonly ICommunicateWithView _communicateWithView;

        public ItemsUserListViewModel(IUserListsService userListsService, 
            IProductsService productsService, 
            IServiceProvider serviceProvider,
            IUserListItemService userListItemService,
            IUserListService userListService,
            ICommunicateWithView communicateWithView)
        {
            _userListsService = userListsService;
            _productsService = productsService;
            _serviceProvider = serviceProvider;
            _userListItemService = userListItemService;
            _userListService = userListService;
            _communicateWithView = communicateWithView;

            // Initialize the collection
            UserListItems = new List<CheckedUserListItemDetailsDTOViewModel>();
        }

       

        [ObservableProperty]
        private string _listName;

        [ObservableProperty]
        private int _userId;

        [ObservableProperty]
        private int _userListId;

        [ObservableProperty] 
        private List<CheckedUserListItemDetailsDTOViewModel> _userListItems;

     
        [RelayCommand]
        public async Task Appearing(UserListDTO userListDTO)
        {
            try
            {
                UserId = userListDTO.CustomerId ?? 0;
                UserListId = userListDTO.Id;
                ListName = userListDTO.Name;

                if (UserListItems.Count == 0 && _userListId > 0)
                {
                    UserListItems = (await _userListItemService.GetAllUserListItems(UserListId)).Select(dto => new CheckedUserListItemDetailsDTOViewModel(dto, false)).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading user list items: {ex.Message}");
            }
        }
        [RelayCommand]
        public async Task ItemClicked(CheckedUserListItemDetailsDTOViewModel item)
        {
            if(item.UserListItemDTO.ProductId != null)
            {
                _communicateWithView.NavigateToProductPage(_serviceProvider.GetService<ProductPageViewModel>(), (int)item.UserListItemDTO.ProductId);
            }
        }
        [RelayCommand]
        public async Task DeleteItem(CheckedUserListItemDetailsDTOViewModel item)
        {
            if (item == null || item.UserListItemDTO?.Id == null) return;

            try
            {
                // Remove the item from the server
                var dto = new UserListItemDTO()
                {
                    Id = item.UserListItemDTO.Id,
                    ProductId = item.UserListItemDTO.ProductId,
                    Quantity = item.UserListItemDTO.Quantity,
                    UserListId = item.UserListItemDTO.UserListId
                };

                await _userListItemService.DeleteUserListItems(_userId, _userListId, new List<UserListItemDTO> { dto });

                // Remove it from the local collection
                UserListItems = (await _userListItemService.GetAllUserListItems(UserListId)).Select(dto => new CheckedUserListItemDetailsDTOViewModel(dto, false)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting item: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task DeleteItems()
        {
           
            try
            {
               
                await _userListItemService.DeleteUserListItems(_userId, _userListId, UserListItems.Where(item => item.IsChecked)
                    .Select(item => new UserListItemDTO()
                    { 
                        Id = item.UserListItemDTO.Id ,
                        ProductId = item.UserListItemDTO.ProductId ,
                        Quantity = item.UserListItemDTO.Quantity ,
                        UserListId = item.UserListItemDTO.UserListId
                    }).ToList());

                UserListItems = (await _userListItemService.GetAllUserListItems(UserListId)).Select(dto => new CheckedUserListItemDetailsDTOViewModel(dto, false)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting items: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddItem()
        {
            // This would typically open a popup or navigation to add items
            // For now, we'll just log that this was called
            Debug.WriteLine("Add item command executed");
        }

        [RelayCommand]
        public async Task Back()
        {
            // Navigate back to the previous page
            await Shell.Current.GoToAsync("..");
        }
    }
}