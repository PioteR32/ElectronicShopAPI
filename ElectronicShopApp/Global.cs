using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp
{
	internal class Global
	{
		// string do pobierania struktury bazy danych z serwera SQL Server i generowania modeli w projekcie. Użyj tego polecenia w konsoli Menedżera pakietów NuGet w Visual Studio, aby wygenerować modele na podstawie istniejącej bazy danych.
		//Scaffold-DbContext "Server=.;Database=electronic_shop;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -Context DataContext -Force


		//
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
