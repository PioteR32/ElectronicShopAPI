using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicShopApp.Services
{
	public class UserListsService
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

		public async Task<IEnumerable<UserListDTO>> GetUserLists(int customerId)
		{
			try
			{
			Global.ApplyAuthHeader(_httpClient);
				var resp = await _httpClient.GetAsync($"{BaseEndpoint}/GetUserLists/{customerId}");
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

		// auth header applied from Global.ApplyAuthHeader
	}
}
