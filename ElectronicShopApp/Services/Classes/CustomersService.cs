using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ElectronicShopApp.Services.Classes
{
	public class CustomersService : ICustomersService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private const string BaseEndpoint = "Customer";

		public CustomersService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		public async Task<bool> CreateUser(CustomerDTO dto)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);
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
				var resp = await _httpClient.PostAsJsonAsync($"{BaseEndpoint}/Login", new LoginDTO { Login = login, Password = password }, _serializerOptions);
				if (!resp.IsSuccessStatusCode) return null;

				var content = await resp.Content.ReadAsStringAsync();
				using var doc = JsonDocument.Parse(content);

				if (doc.RootElement.TryGetProperty("token", out var t))
				{
					var token = t.GetString();
					if (!string.IsNullOrWhiteSpace(token)) Global.ApplyAuthHeader(_httpClient); // ensure header applied after login
					if (!string.IsNullOrWhiteSpace(token)) Global.authorizeToken = token;
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
				Global.ApplyAuthHeader(_httpClient,true);
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

		public async Task<CustomerDTO?> ChangeUserNames(string firstName, string lastName)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient, true);

				// 1. Przygotowanie obiektu do wysłania w JSON
				var requestPayload = new ChangeUserNamesDTO() { FirstName = firstName, LastName = lastName };
				string json = JsonSerializer.Serialize(requestPayload, _serializerOptions);
				using var contentPayload = new StringContent(json, Encoding.UTF8, "application/json");

				// 2. Wysyłanie żądania POST do właściwego endpointu ("ProfileChange")
				var resp = await _httpClient.PostAsync($"{BaseEndpoint}/ProfileChange", contentPayload);

				if (!resp.IsSuccessStatusCode)
					return null;

				var content = await resp.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<CustomerDTO>(content, _serializerOptions);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[ChangeUserNames Error]: {ex.Message}");
				return null;
			}
		}

		public async Task<CustomerDTO?> ChangeUserPassword(string password)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient, true);

				// 1. Przygotowanie obiektu do wysłania w JSON
				var requestPayload = new string(password);
				string json = JsonSerializer.Serialize(requestPayload, _serializerOptions);
				using var contentPayload = new StringContent(json, Encoding.UTF8, "application/json");

				// 2. Wysyłanie żądania POST do właściwego endpointu ("ProfileChange")
				var resp = await _httpClient.PostAsync($"{BaseEndpoint}/ProfilePasswordChange", contentPayload);

				if (!resp.IsSuccessStatusCode)
					return null;

				var content = await resp.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<CustomerDTO>(content, _serializerOptions);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[ChangeUserNames Error]: {ex.Message}");
				return null;
			}
		}


		// auth header applied from Global.ApplyAuthHeader
	}
}
