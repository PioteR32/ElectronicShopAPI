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
using Shared.DTO.Filters;

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

		public async Task<IEnumerable<ProductDTO>> GetProducts(int pageNumber, int pageSize)
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
		public async Task<IEnumerable<ProductDTO>> GetFilteredProducts(ProductFilterDTO filter)
		{
			string queryString = $"?pageIndex={filter.PageIndex}" +
				$"&pageSize={filter.PageSize}" +
				$"&searchPhrase={filter.SearchPhrase}" +
				$"&categoryId={filter.CategoryId}" +
				$"&minPrice={filter.MinPrice}" +
				$"&maxPrice={filter.MaxPrice}" +
				$"&isInStock={filter.IsInStock}" +
				$"&orderByOptions={filter.OrderByOptions}";
			queryString += ServicesFnc.TableToUrlString("filterOptionsIds", filter.FilterOptionsIds);

			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + @"/filter" + queryString);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<ProductDTO>>(content, _serializerOptions) ?? new List<ProductDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"ProductsService.GetFilteredProducts Błąd: {ex.Message}"); }
			return new List<ProductDTO>();
		}

        public async Task<List<ProductSpecificationDTO>> GetProductSpecsById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(baseEndpoint + @"/GetProductSpecsById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<ProductSpecificationDTO>>(content, _serializerOptions) ?? new List<ProductSpecificationDTO>();
                }
            }
            catch (Exception ex) { Debug.WriteLine($"ProductsService.GetProductSpecsById Błąd: {ex.Message}"); }
            return new List<ProductSpecificationDTO>();
        }
    }
}
