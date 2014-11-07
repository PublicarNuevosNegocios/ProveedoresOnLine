using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderFileLogModel
    {
        public string Url { get; set; }

        public DateTime CreateDate { get; set; }

        public string FieldType { get; set; }
    }
}
