using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ProductSpecificationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int FilterId { get; set; }

        public string FilterName { get; set; } = null;

        public int? ParentId { get; set; }

        public bool? HasOptions { get; set; } = null;
    }
}
