using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GAC.IntegrationSolution.FilePoller.DTOs
{
    [XmlRoot("PurchaseOrder", Namespace = "")]
    public class PurchaseOrderXmlRoot
    {
        [XmlElement("OrderId")]
        public string OrderId { get; set; }

        [XmlElement("ProcessingDate")]
        public DateTime ProcessingDate { get; set; }

        [XmlElement("Customer")]
        public string Customer { get; set; }

        [XmlArray("Products")]
        [XmlArrayItem("Product")]
        public List<PurchaseOrderItemDto> Products { get; set; } = new();
    }
}
