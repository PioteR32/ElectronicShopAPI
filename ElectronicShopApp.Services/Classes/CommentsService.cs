using ElectronicShopApp.Services.Interfaces;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElectronicShopApp.Services.Classes
{
	public class CommentsService : ICommentService
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions;
		private const string BaseEndpoint = "Comments";

		public CommentsService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Global.baseUrl);
			_serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		}

		public async Task<IEnumerable<CommentDTO>> GetCommentsForProduct(int productId)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);
				var resp = await _httpClient.GetAsync($"{BaseEndpoint}/product/{productId}");
				if (!resp.IsSuccessStatusCode) return new List<CommentDTO>();
				var content = await resp.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<List<CommentDTO>>(content, _serializerOptions) ?? new List<CommentDTO>();
			}
			catch (Exception ex) { Debug.WriteLine(ex.Message); }
			return new List<CommentDTO>();
		}

		public async Task<CommentDTO?> GetById(int id)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);
				var resp = await _httpClient.GetAsync($"{BaseEndpoint}/{id}");
				if (!resp.IsSuccessStatusCode) return null;
				var content = await resp.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<CommentDTO>(content, _serializerOptions);
			}
			catch (Exception ex) { Debug.WriteLine(ex.Message); }
			return null;
		}

		public async Task<int?> Create(CommentDTO dto)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);
				var resp = await _httpClient.PostAsJsonAsync(BaseEndpoint, dto, _serializerOptions);
				if (!resp.IsSuccessStatusCode) return null;
				var content = await resp.Content.ReadAsStringAsync();
				using var doc = JsonDocument.Parse(content);
				if (doc.RootElement.TryGetProperty("id", out var idElem)) return idElem.GetInt32();
			}
			catch (Exception ex) { Debug.WriteLine(ex.Message); }
			return null;
		}

		public async Task<bool> Update(int id, CommentDTO dto)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);
				var resp = await _httpClient.PutAsJsonAsync($"{BaseEndpoint}/{id}", dto, _serializerOptions);
				return resp.IsSuccessStatusCode;
			}
			catch (Exception ex) { Debug.WriteLine(ex.Message); }
			return false;
		}

		public async Task<bool> Delete(int id)
		{
			try
			{
				Global.ApplyAuthHeader(_httpClient);
				var resp = await _httpClient.DeleteAsync($"{BaseEndpoint}/{id}");
				return resp.IsSuccessStatusCode;
			}
			catch (Exception ex) { Debug.WriteLine(ex.Message); }
			return false;
		}
	}
}
