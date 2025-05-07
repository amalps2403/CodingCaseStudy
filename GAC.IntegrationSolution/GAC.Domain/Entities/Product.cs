using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Dimensions { get; set; } = null!;
    }
}
