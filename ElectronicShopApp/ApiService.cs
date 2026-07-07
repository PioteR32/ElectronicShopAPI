using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Shared.DTO;
namespace ElectronicShopApp
{
	public class ApiService
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseUrl = "https://localhost:7276/";

		// HttpClient jest wstrzykiwany przez konstruktor
		public ApiService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(_baseUrl);
		}

		public async Task<bool> CreateUserAsync(CustomerDTO user)
		{
			try
			{
				// 1. Serializacja obiektu C# do formatu JSON
				string json = JsonSerializer.Serialize(user);

				// 2. Przygotowanie zawartości (Content) z odpowiednim kodowaniem i nagłówkiem Content-Type
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				// 3. Wysyłanie żądania POST
				HttpResponseMessage response = await _httpClient.PostAsync("api/users", content);

				// 4. Sprawdzenie, czy serwer odpowiedział kodem z serii 2xx (Sukces)
				if (response.IsSuccessStatusCode)
				{
					// Opcjonalnie: odczytanie danych zwrotnych z API
					string responseResult = await response.Content.ReadAsStringAsync();
					return true;
				}

				// Tutaj możesz obsłużyć specyficzne kody błędów, np. 400 Bad Request, 401 Unauthorized
				return false;
			}
			catch (HttpRequestException ex)
			{
				// Błąd sieciowy (np. brak internetu, serwer nie odpowiada)
				Debug.WriteLine($"Błąd sieci: {ex.Message}");
				return false;
			}
			catch (Exception ex)
			{
				// Inne nieprzewidziane błędy
				Debug.WriteLine($"Błąd: {ex.Message}");
				return false;
			}
		}
	}
}
