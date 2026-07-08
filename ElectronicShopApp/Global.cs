using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp
{
	internal class Global
	{
		public static string baseUrl = "https://localhost:7276/api/";
		public static string authorizeToken = "";

		public static void ApplyAuthHeader(System.Net.Http.HttpClient client)
		{
			if (client == null) return;
			if (!string.IsNullOrWhiteSpace(authorizeToken))
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
