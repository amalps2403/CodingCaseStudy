using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.IntegrationSolution.FilePoller.Settings
{
    public class FilePollingSettings
    {
        public string WatchFolder { get; set; } = "SampleFiles";
        public string CronSchedule { get; set; } = "*/1 * * * *";
    }

    public class WmsApiSettings
    {
        public string Endpoint { get; set; } = "http://localhost:5112/api/";
    }
}
