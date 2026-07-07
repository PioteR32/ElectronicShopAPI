using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicShopApp.Services
{
	public class ShoppingCartService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private const string BaseEndpoint = "ShoppingCarts";

		public ShoppingCartService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(GlobalStaticVariables.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		public async Task<IEnumerable<ShoppingCartItemDTO>> GetCart(int customerId)
		{
			try
			{
				AddAuthHeaderIfPresent();
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

		private void AddAuthHeaderIfPresent()
		{
			if (!string.IsNullOrWhiteSpace(GlobalStaticVariables.authorizeToken))
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalStaticVariables.authorizeToken);
			else
				_httpClient.DefaultRequestHeaders.Authorization = null;
		}
	}
}
