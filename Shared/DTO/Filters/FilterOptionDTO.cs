using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Filters
{
	public class FilterOptionDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;

		public int FilterId { get; set; }

		public int? ParentId { get; set; }

		public bool? HasOptions { get; set; } = null;

		public FilterOptionDTO SetHaveOption(bool value)
		{
			HasOptions = value;
			return this;
		}
	}
}
