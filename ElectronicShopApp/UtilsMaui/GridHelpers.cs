using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopApp.UtilsMaui
{
	namespace UtilsMaui.Utils
	{
		public class GridHelpers
		{
			#region RowCount Property

			/// <summary>
			/// Adds the specified number of Rows to RowDefinitions. 
			/// Default Height is Auto
			/// </summary>
			public static readonly BindableProperty RowCountProperty =
				BindableProperty.CreateAttached(
					"RowCount", typeof(int), typeof(GridHelpers), -1,
					propertyChanged: RowCountChanged);

			// Get
			public static int GetRowCount(BindableObject obj)
			{
				return (int)obj.GetValue(RowCountProperty);
			}

			// Set
			public static void SetRowCount(BindableObject obj, int value)
			{
				obj.SetValue(RowCountProperty, value);
			}

			// Change Event - Adds the Rows
			public static void RowCountChanged(BindableObject obj, object oldValue, object newValue)
			{
				if (!(obj is Grid) || (int)newValue < 0)
					return;

				Grid grid = (Grid)obj;
				grid.RowDefinitions.Clear();

				for (int i = 0; i < (int)newValue; i++)
					grid.RowDefinitions.Add(
						new RowDefinition() { Height = GridLength.Star });
			}

			public static readonly BindableProperty RowDefinitionsHelperProperty =
					BindableProperty.CreateAttached(
						"RowDefinitionsHelper", typeof(int[]), typeof(GridHelpers), new int[0],
						propertyChanged: RowDefinitionsHelperChanged);
			public static int[] GetRowDefinitionsHelper(BindableObject obj)
			{
				return (int[])obj.GetValue(RowDefinitionsHelperProperty);
			}

			public static void SetRowDefinitionsHelper(BindableObject obj, int[] value)
			{
				obj.SetValue(RowDefinitionsHelperProperty, value);
			}

			public static void RowDefinitionsHelperChanged(BindableObject obj, object oldValue, object newValue)
			{
				if (!(obj is Grid) || ((int[])(newValue)).Length <= 0)
					return;

				Grid grid = (Grid)obj;
				grid.RowDefinitions.Clear();
				int[] rowDefinitions = (int[])(newValue);
				for (int i = 0; i < rowDefinitions.Length; i++)
					grid.RowDefinitions.Add(
						new RowDefinition() { Height = new GridLength(rowDefinitions[i], GridUnitType.Star) });
			}
			#endregion

			#region ColumnCount Property

			/// <summary>
			/// Adds the specified number of Columns to ColumnDefinitions. 
			/// Default Width is Auto
			/// </summary>
			public static readonly BindableProperty ColumnCountProperty =
				BindableProperty.CreateAttached(
					"ColumnCount", typeof(int), typeof(GridHelpers), -1,
					propertyChanged: ColumnCountChanged);

			// Get
			public static int GetColumnCount(BindableObject obj)
			{
				return (int)obj.GetValue(ColumnCountProperty);
			}

			// Set
			public static void SetColumnCount(BindableObject obj, int value)
			{
				obj.SetValue(ColumnCountProperty, value);
			}

			// Change Event - Add the Columns
			public static void ColumnCountChanged(BindableObject obj, object oldValue, object newValue)
			{
				if (!(obj is Grid) || (int)newValue < 0)
					return;

				Grid grid = (Grid)obj;
				grid.ColumnDefinitions.Clear();

				for (int i = 0; i < (int)newValue; i++)
					grid.ColumnDefinitions.Add(
						new ColumnDefinition() { Width = GridLength.Star });
			}
			#endregion

			#region GridDefinitionsHelper Property
			public static readonly BindableProperty ColumnDefinitionsHelperProperty =
		BindableProperty.CreateAttached(
			"ColumnDefinitionsHelper", typeof(int[]), typeof(GridHelpers), new int[0],
			propertyChanged: ColumnDefinitionsHelperChanged);

			public static int[] GetColumnDefinitionsHelper(BindableObject obj)
			{
				return (int[])obj.GetValue(ColumnDefinitionsHelperProperty);
			}

			public static void SetColumnDefinitionsHelper(BindableObject obj, int[] value)
			{
				obj.SetValue(ColumnDefinitionsHelperProperty, value);
			}

			public static void ColumnDefinitionsHelperChanged(BindableObject obj, object oldValue, object newValue)
			{
				if (!(obj is Grid) || newValue == null ? true : ((int[])(newValue)).Length <= 0)
					return;
				 
				Grid grid = (Grid)obj;
				grid.ColumnDefinitions.Clear();
				int[] columnDefinitions = (int[])(newValue);
				for (int i = 0; i < columnDefinitions.Length; i++)
					grid.ColumnDefinitions.Add(
						new() { Width = new GridLength(columnDefinitions[i], GridUnitType.Star) });
			}
			#endregion
		}
	}
}
