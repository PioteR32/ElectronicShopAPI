using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ElectronicShopApp.Services.Interfaces;

namespace ElectronicShopApp.Services.Classes
{
	public class ProductsService : IProductsService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly string baseEndpoint = "Products";
		// HttpClient jest wstrzykiwany przez konstruktor
		public ProductsService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Swagger domyślnie używa camelCase
				WriteIndented = true
			};
		}

		public async Task<IEnumerable<ProductDTO>> GetProducts(int pageNumber,int pageSize)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);

				var response = await _httpClient.GetAsync(baseEndpoint + @"/1-10");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<ProductDTO>>(content, _serializerOptions) ?? new List<ProductDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"Błąd: {ex.Message}"); }
			return new List<ProductDTO>();
		}

		// auth header applied from Global.ApplyAuthHeader

	}
}
