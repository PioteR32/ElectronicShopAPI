using Shared.DTO.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElectronicShopApp.ViewModels.DTOs
{
	public class FilterOptionDTOViewModel : BindableObject
	{
		public FilterOptionDTOViewModel(FilterOptionDTO filterOptionDTO)
		{
			FilterOptionDTO = filterOptionDTO;
		}
		

		public FilterOptionDTO FilterOptionDTO { get; }

		public string Name
		{
			get
			{
				return FilterOptionDTO.Name;
			}
		}

		List<FilterOptionDTOViewModel> _filterOptionValues;
		List<FilterOptionDTOViewModel> FilterOptionValues
		{
			get { return _filterOptionValues; }
			set
			{
				_filterOptionValues = value;
				OnPropertyChanged(nameof(FilterOptionValues));
			}
		}

		bool _isSelected;


		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				OnPropertyChanged(nameof(IsSelected));
			}
		}
	}
}
