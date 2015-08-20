using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public class GenericReportModel
    {
        public byte[] File { get; set; }

        public string MimeType { get; set; }

        public string FileName { get; set; }
    }
}
