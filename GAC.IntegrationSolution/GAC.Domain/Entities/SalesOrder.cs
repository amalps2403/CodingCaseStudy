using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Domain.Entities
{
    public class SalesOrder
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; } = null!;
        public DateTime ProcessingDate { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string ShipmentAddress { get; set; } = null!;
        public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    }
}
