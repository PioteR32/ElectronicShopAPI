using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicShopApp.Services
{
	public class OrdersService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private const string BaseEndpoint = "Order";

		public OrdersService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(GlobalStaticVariables.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		public async Task<int?> CreateOrder(OrderDTO dto)
		{
			try
			{
				AddAuthHeaderIfPresent();
				var resp = await _httpClient.PostAsJsonAsync($"{BaseEndpoint}/CreateOrder", dto, _serializerOptions);
				if (!resp.IsSuccessStatusCode) return null;
				var content = await resp.Content.ReadAsStringAsync();
				using var doc = JsonDocument.Parse(content);
				if (doc.RootElement.TryGetProperty("orderId", out var idElem)) return idElem.GetInt32();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return null;
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
