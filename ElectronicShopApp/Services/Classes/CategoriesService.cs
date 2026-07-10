using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;

namespace ElectronicShopApp.Services.Classes
{
	public class CategoriesService : ICategoriesService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly string baseEndpoint = "Categories";

		public CategoriesService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			};
		}

		public async Task<IEnumerable<CategoryDTO>> GetCategories()
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<CategoryDTO>>(content, _serializerOptions) ?? new List<CategoryDTO>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"CategoriesService.GetCategories Błąd: {ex.Message}"); }
			return new List<CategoryDTO>();
		}

		public async Task<CategoryDTO?> GetCategoryById(int id)
		{
			try
			{
				var response = await _httpClient.GetAsync(baseEndpoint + $"/{id}");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<CategoryDTO>(content, _serializerOptions);
				}
			}
			catch (Exception ex) { Debug.WriteLine($"CategoriesService.GetCategoryById Błąd: {ex.Message}"); }
			return null;
		}

		public async Task<IEnumerable<int>> AddCategories(CategoryDTO[] categories)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync(baseEndpoint + "/AddCategories", categories, _serializerOptions);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"CategoriesService.AddCategories Błąd: {ex.Message}"); }
			return new List<int>();
		}

		public async Task<IEnumerable<int>> DeleteCategories(int[] ids)
		{
			try
			{
				// build query ?id=1&id=2
				var query = string.Join('&', Array.ConvertAll(ids, i => $"id={i}"));
				var response = await _httpClient.DeleteAsync(baseEndpoint + "/DeleteCategories?" + query);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"CategoriesService.DeleteCategories Błąd: {ex.Message}"); }
			return new List<int>();
		}

		public async Task<IEnumerable<int>> ModifyCategories(CategoryDTO[] categories)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync(baseEndpoint + "/ModifyCategories", categories, _serializerOptions);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<List<int>>(content, _serializerOptions) ?? new List<int>();
				}
			}
			catch (Exception ex) { Debug.WriteLine($"CategoriesService.ModifyCategories Błąd: {ex.Message}"); }
			return new List<int>();
		}
	}
}
