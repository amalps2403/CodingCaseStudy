using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GAC.IntegrationSolution.FilePoller.DTOs
{
    [XmlRoot("PurchaseOrder")]
    public class PurchaseOrderDto
    {
        [XmlElement(ElementName = "OrderId")]
        public string OrderId { get; set; }

        [XmlElement(ElementName = "ProcessingDate")]
        public DateTime ProcessingDate { get; set; }

        [XmlElement(ElementName = "Customer")]
        public string Customer { get; set; }

        [XmlArray("Products")]
        [XmlArrayItem("Product")]
        public List<PurchaseOrderItemDto> Products { get; set; } = new();
    }
}
