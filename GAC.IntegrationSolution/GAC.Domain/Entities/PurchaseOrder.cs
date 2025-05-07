using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Domain.Entities
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; } = null!;
        public DateTime ProcessingDate { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}
