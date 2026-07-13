
using Shared.DTO.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElectronicShopApp.ViewModels.DTOs
{
	public class FilterDTOViewModel:BindableObject
	{
		public FilterDTOViewModel(FilterDTO filterDTO)
		{
			FilterDTO = filterDTO;
		}
		public FilterDTO FilterDTO { get; }

		List<FilterOptionDTOViewModel> _filterOptions;
		public List<FilterOptionDTOViewModel> FilterOptions
		{
			get { return _filterOptions; }
			set
			{
				_filterOptions = value;
				OnPropertyChanged();
			}
		}

		public int Id { get; set; }
		public string Name 
		{ 
			get => FilterDTO.Name; 
		} 
	}
}
