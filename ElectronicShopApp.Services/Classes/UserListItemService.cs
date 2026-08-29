using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicShopApp.Services;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;

namespace ElectronicShopApp.Services.Classes
{
    public class UserListItemService : IUserListItemService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private const string BaseEndpoint = "UserListItem";

        public UserListItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Global.baseUrl);
            _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<IEnumerable<UserListItemDetailDTO>> GetAllUserListItems(int listId)
        {
            try
            {
                Global.ApplyAuthHeader(_httpClient, true);
                var resp = await _httpClient.GetAsync($"{BaseEndpoint}/listId={listId}");
                //problem: the API returns 404 if the list is empty, so we need to handle that case
                if (!resp.IsSuccessStatusCode) return new List<UserListItemDetailDTO>();
                var content = await resp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<UserListItemDetailDTO>>(content, _serializerOptions) ?? new List<UserListItemDetailDTO>();
            }
            catch (Exception ex)
            {
                return new List<UserListItemDetailDTO>();
            }
        }

        public async Task DeleteUserListItems(int userId, int listId, IEnumerable<UserListItemDTO> userListItemDtos)
        {
            try
            {
                Global.ApplyAuthHeader(_httpClient, true);
                var json = JsonSerializer.Serialize(userListItemDtos, _serializerOptions);
                var resp = await _httpClient.PostAsync($"{BaseEndpoint}/delete",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                Console.WriteLine($"Status: {resp.StatusCode}");
                Console.WriteLine($"Response Content: {await resp.Content.ReadAsStringAsync()}");

                // Sprawdź czy operacja się powiodła
                if (!resp.IsSuccessStatusCode)
                {
                    throw new Exception($"API call failed with status: {resp.StatusCode}, content: {await resp.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteUserListItems: {ex.Message}");
                // log
                throw; // lub zwróć false jeśli to metoda void
            }
        }


        public async Task<IEnumerable<UserListItemDTO>> GetUserListItems(int userId, int userListId)
        {
            try
            {
                Global.ApplyAuthHeader(_httpClient, true);
                var resp = await _httpClient.GetAsync($"{BaseEndpoint}/GetUserListItems/{userId}/{userListId}");
                if (!resp.IsSuccessStatusCode) return new List<UserListItemDTO>();
                var content = await resp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<UserListItemDTO>>(content, _serializerOptions) ?? new List<UserListItemDTO>();
            }
            catch (Exception ex)
            {
                // Log error
                return new List<UserListItemDTO>();
            }
        }

        public async Task<bool> AddUserListItem(UserListItemDTO userListItem)
        {
            try
            {
                Global.ApplyAuthHeader(_httpClient, true);
                var requestPayload = userListItem;
                string json = JsonSerializer.Serialize(requestPayload, _serializerOptions);
                using var contentPayload = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await _httpClient.PostAsync($"{BaseEndpoint}/AddUserListItem", contentPayload);
                if (!resp.IsSuccessStatusCode) return false;

                return true;
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }

        public async Task<bool> RemoveUserListItem(int itemId)
        {
            try
            {
                Global.ApplyAuthHeader(_httpClient, true);
                var resp = await _httpClient.DeleteAsync($"{BaseEndpoint}/RemoveUserListItem/{itemId}");
                if (!resp.IsSuccessStatusCode) return false;

                return true;
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }

        public async Task<bool> UpdateUserListItem(UserListItemDTO userListItem)
        {
            try
            {
                Global.ApplyAuthHeader(_httpClient, true);
                var requestPayload = userListItem;
                string json = JsonSerializer.Serialize(requestPayload, _serializerOptions);
                using var contentPayload = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await _httpClient.PutAsync($"{BaseEndpoint}/UpdateUserListItem", contentPayload);
                if (!resp.IsSuccessStatusCode) return false;

                return true;
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }
    }
}
