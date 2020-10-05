using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passninja
{
    public class PassResponseData
    {
        public Urls urls { get; set; }
        public string url { get; set; }
        public string serialNumber { get; set; }
        public string passType { get; set; }
        public string message { get; set; }
    }
}
