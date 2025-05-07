using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GAC.IntegrationSolution.FilePoller.DTOs
{
    public class PurchaseOrderItemDto
    {
        [XmlElement(ElementName = "ProductCode")]
        public string ProductCode { get; set; }

        [XmlElement(ElementName = "Quantity")]
        public int Quantity { get; set; }
    }
}
