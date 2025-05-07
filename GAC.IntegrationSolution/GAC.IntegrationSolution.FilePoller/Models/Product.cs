using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GAC.IntegrationSolution.FilePoller.Models
{
    public class Product
    {
        [XmlElement("ProductCode")]
        public string ProductCode { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Dimensions")]
        public Dimensions Dimensions { get; set; }
    }
}
