using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GAC.IntegrationSolution.FilePoller.Helpers;

public static class XmlHelper
{
    public static T Deserialize<T>(string filePath)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stream = new FileStream(filePath, FileMode.Open);
        return (T)serializer.Deserialize(stream);
    }
}
