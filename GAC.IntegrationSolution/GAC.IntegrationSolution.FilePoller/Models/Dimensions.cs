using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GAC.IntegrationSolution.FilePoller.Models
{
    public class Dimensions
    {
        [XmlElement("Length")]
        public double Length { get; set; }

        [XmlElement("Width")]
        public double Width { get; set; }

        [XmlElement("Height")]
        public double Height { get; set; }
    }
}
