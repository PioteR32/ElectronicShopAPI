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
	public class FilterOptionsService : IFilterOptionService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly string baseEndpoint = "FilterOptions";

		public FilterOptionsService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		// Convenience wrapper calling API endpoint for category filter options
		public async Task<IEnumerable<FilterOptionDTO>> GetCategoryFilterOptions(int categoryId)
		{
			return await GetOptionsForCategory(categoryId);
		}

		public async Task<IEnumerable<FilterOptionDTO>> GetOptionsForCategory(int categoryId)
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/ByCategory/{categoryId}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<FilterOptionDTO>>(content, _serializerOptions) ?? new List<FilterOptionDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.GetOptionsForCategory Błąd: {ex.Message}"); }
			return new List<FilterOptionDTO>();
		}

		public async Task<IEnumerable<FilterOptionDTO>> GetOptions()
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + "/filtersOptions");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<FilterOptionDTO>>(content, _serializerOptions) ?? new List<FilterOptionDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.GetOptions Błąd: {ex.Message}"); }
			return new List<FilterOptionDTO>();
		}

		public async Task<IEnumerable<FilterOptionDTO>> GetOptionsForFilter(int filterId)
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/ByFilter/{filterId}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<FilterOptionDTO>>(content, _serializerOptions) ?? new List<FilterOptionDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.GetOptionsForFilter Błąd: {ex.Message}"); }
			return new List<FilterOptionDTO>();
		}
		public async Task<IEnumerable<FilterOptionDTO>> GetOptionsForParentFilterOption(int parentFilterOptionId)
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/ByFilterOption/{parentFilterOptionId}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<FilterOptionDTO>>(content, _serializerOptions) ?? new List<FilterOptionDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.GetOptionsForFilterOption Błąd: {ex.Message}"); }
			return new List<FilterOptionDTO>();
		}

		public async Task<FilterOptionDTO?> GetById(int id)
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/{id}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<FilterOptionDTO>(content, _serializerOptions);
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.GetById Błąd: {ex.Message}"); }
			return null;
		}

		public async Task<IEnumerable<int>> AddOptions(FilterOptionDTO[] options)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync(baseEndpoint + "/AddFilterOptions", options, _serializerOptions);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.AddOptions Błąd: {ex.Message}"); }
			return new List<int>();
		}

		public async Task<IEnumerable<int>> DeleteOptions(int[] ids)
		{
			try
			{
				var query = string.Join('&', Array.ConvertAll(ids, i => $"id={i}"));
				var response = await _httpClient.DeleteAsync(baseEndpoint + "/DeleteFilterOptions?" + query);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.DeleteOptions Błąd: {ex.Message}"); }
			return new List<int>();
		}

		public async Task<IEnumerable<int>> ModifyOptions(FilterOptionDTO[] options)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync(baseEndpoint + "/ModifyFilterOptions", options, _serializerOptions);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"FilterOptionsService.ModifyOptions Błąd: {ex.Message}"); }
			return new List<int>();
		}
	}
}
