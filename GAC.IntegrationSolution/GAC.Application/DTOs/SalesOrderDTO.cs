using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.Application.DTOs
{
    public class SalesOrderDTO
    {
        public string OrderId { get; set; } = null!;
        public DateTime ProcessingDate { get; set; }
        public string CustomerIdentifier { get; set; } = null!;
        public string ShipmentAddress { get; set; } = null!;
        public List<SalesOrderItemDTO> Items { get; set; } = new();
    }
}
