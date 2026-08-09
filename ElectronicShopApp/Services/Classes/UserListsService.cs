using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ElectronicShopApp.Services.Classes
{
	public class UserListsService : IUserListsService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private const string BaseEndpoint = "UserLists";

		public UserListsService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		public async Task<IEnumerable<UserListDTO>> GetUserLists()
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient,true);
				var resp = await _httpClient.GetAsync($"{BaseEndpoint}/GetUserLists");
				if (!resp.IsSuccessStatusCode) return new List<UserListDTO>();
				var content = await resp.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<List<UserListDTO>>(content, _serializerOptions) ?? new List<UserListDTO>();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return new List<UserListDTO>();
			}
		}
		public async Task<bool> AddUserList(UserListDTO userList)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient,true);
				var requestPayload = userList;
				string json = JsonSerializer.Serialize(requestPayload, _serializerOptions);
				using var contentPayload = new StringContent(json, Encoding.UTF8, "application/json");
				var resp = await _httpClient.PostAsync($"{BaseEndpoint}/AddList", contentPayload);
				if (!resp.IsSuccessStatusCode) return false;
				
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return false;
			}
		}

		// auth header applied from Global.ApplyAuthHeader
	}
}
