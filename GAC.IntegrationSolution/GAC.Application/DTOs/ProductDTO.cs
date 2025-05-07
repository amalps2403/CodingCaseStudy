using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Application.DTOs
{
    public class ProductDTO
    {
        public string ProductCode { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Dimensions { get; set; } = null!;
    }
}
