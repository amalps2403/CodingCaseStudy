using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Application.DTOs
{
    public class PurchaseOrderDTO
    {
        public string OrderId { get; set; } = null!;
        public DateTime ProcessingDate { get; set; }
        public string CustomerIdentifier { get; set; } = null!;
        public List<PurchaseOrderItemDTO> Items { get; set; } = new();
    }
}
