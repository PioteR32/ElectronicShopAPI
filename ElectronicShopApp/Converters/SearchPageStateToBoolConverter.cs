using System;
using System.Collections.Generic;
using System.Globalization;
using ElectronicShopApp.ViewModels;
using System.Text;

namespace ElectronicShopApp.Converters
{
	public class SearchPageStateToBoolConverter : IValueConverter
	{
		/// <summary>
		/// Converts a SearchPageState value to a boolean value.
		/// </summary>
		/// <param name="value">The SearchPageState value to convert.</param>
		/// <param name="targetType"></param>
		/// <param name="parameter"> Correct value for the conversion.</param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if(value != null  && value is SearchPageState valueSPG && parameter is SearchPageState paramSPG)
			{
				return valueSPG == paramSPG;
			}
			throw new NotImplementedException();
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
