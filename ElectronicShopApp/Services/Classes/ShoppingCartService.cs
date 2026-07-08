using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicShopApp.Services.Classes
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private const string BaseEndpoint = "ShoppingCarts";

		public ShoppingCartService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		public async Task<IEnumerable<ShoppingCartItemDTO>> GetCart(int customerId)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);
				var resp = await _httpClient.GetAsync($"{BaseEndpoint}/GetCart/{customerId}");
				if (!resp.IsSuccessStatusCode) return new List<ShoppingCartItemDTO>();
				var content = await resp.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<List<ShoppingCartItemDTO>>(content, _serializerOptions) ?? new List<ShoppingCartItemDTO>();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return new List<ShoppingCartItemDTO>();
			}
		}

		// auth header applied from Global.ApplyAuthHeader
	}
}
