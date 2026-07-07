using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicShopApp.Services
{
	public class CustomersService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private const string BaseEndpoint = "Customers";

		public CustomersService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(GlobalStaticVariables.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		public async Task<bool> CreateUser(CustomerDTO dto)
		{
			try
			{
				AddAuthHeaderIfPresent();
				var resp = await _httpClient.PostAsJsonAsync(BaseEndpoint, dto, _serializerOptions);
				return resp.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<string?> LogIn(string login, string password)
		{
			try
			{
				// Login endpoint returns token in JSON { Token: "..." }
				var resp = await _httpClient.PostAsync($"{BaseEndpoint}/Login/{login}/Password/{password}", null);
				if (!resp.IsSuccessStatusCode) return null;
				var content = await resp.Content.ReadAsStringAsync();
				using var doc = JsonDocument.Parse(content);
				if (doc.RootElement.TryGetProperty("Token", out var t))
				{
					var token = t.GetString();
					if (!string.IsNullOrWhiteSpace(token)) GlobalStaticVariables.authorizeToken = token;
					return token;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return null;
		}

		public async Task<CustomerDTO?> GetProfile()
		{
			try
			{
				AddAuthHeaderIfPresent();
				var resp = await _httpClient.GetAsync($"{BaseEndpoint}/Profile");
				if (!resp.IsSuccessStatusCode) return null;
				var content = await resp.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<CustomerDTO>(content, _serializerOptions);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
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
