using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO.Filters;

namespace ElectronicShopApp.Services.Classes
{
	public class FiltersService : IFilterService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly string baseEndpoint = "Filters";

		public FiltersService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		// Convenience wrapper calling API endpoint for category filters
		

		public async Task<IEnumerable<FilterDTO>> GetFiltersForCategory(int categoryId)
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/by-category/{categoryId}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<FilterDTO>>(content, _serializerOptions) ?? new List<FilterDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FiltersService.GetFiltersForCategory Błąd: {ex.Message}"); }
			return new List<FilterDTO>();
		}

		public async Task<IEnumerable<FilterDTO>> GetFilters()
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/filters");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<FilterDTO>>(content, _serializerOptions) ?? new List<FilterDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FiltersService.GetFilters Błąd: {ex.Message}"); }
			return new List<FilterDTO>();
		}

		public async Task<FilterDTO?> GetById(int id)
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/{id}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<FilterDTO>(content, _serializerOptions);
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FiltersService.GetById Błąd: {ex.Message}"); }
			return null;
		}

		public async Task<IEnumerable<int>> AddFilters(FilterDTO[] filters)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync(baseEndpoint + "/AddFilters", filters, _serializerOptions);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FiltersService.AddFilters Błąd: {ex.Message}"); }
			return new List<int>();
		}

		public async Task<IEnumerable<int>> DeleteFilters(int[] ids)
		{
			try
			{
				var query = string.Join('&', Array.ConvertAll(ids, i => $"id={i}"));
				var response = await _httpClient.DeleteAsync(baseEndpoint + "/DeleteFilters?" + query);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FiltersService.DeleteFilters Błąd: {ex.Message}"); }
			return new List<int>();
		}

		public async Task<IEnumerable<int>> ModifyFilters(FilterDTO[] filters)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync(baseEndpoint + "/ModifyFilters", filters, _serializerOptions);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FiltersService.ModifyFilters Błąd: {ex.Message}"); }
			return new List<int>();
		}
	}
}
