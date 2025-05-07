using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Application.DTOs
{
    public class PurchaseOrderItemDTO
    {
        public string ProductCode { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
