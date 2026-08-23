using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.Services

{
	public class Global
	{

		public static string baseUrl = "https://localhost:7276/api/";
		public static string authorizeToken = "";
		public static void Login(string? auth)
		{
			if (auth != null)
			{
				authorizeToken = auth;
			}
		}
		public static void ApplyAuthHeader(System.Net.Http.HttpClient client,bool authNeeded = false)
		{
			if (client == null) return;
			if (!string.IsNullOrWhiteSpace(authorizeToken) && authNeeded)
			{
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizeToken);
			}
			else
			{
				client.DefaultRequestHeaders.Authorization = null;
			}
		}
	}
}
