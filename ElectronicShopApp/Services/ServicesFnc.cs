
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ElectronicShopApp.Services
{
	public static class ServicesFnc
	{
		public static void AddAuthHeaderIfPresent(HttpClient httpClient)
		{
			if (!string.IsNullOrWhiteSpace(Global.authorizeToken))
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Global.authorizeToken);
			else
				httpClient.DefaultRequestHeaders.Authorization = null;
		}
	}
}
