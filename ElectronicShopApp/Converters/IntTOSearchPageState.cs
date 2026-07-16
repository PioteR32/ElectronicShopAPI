using System;
using System.Collections.Generic;
using System.Globalization;
using ElectronicShopApp.ViewModels;
using System.Text;

namespace ElectronicShopApp.Converters
{
	public class IntToSearchPageStateConverter : IValueConverter
	{
		public int Value { get; set; }
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value != null && value is int valueSPG)
			{
				return (SearchPageState)valueSPG;
			}
			else
				return (SearchPageState)Value;
			throw new NotImplementedException();
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
