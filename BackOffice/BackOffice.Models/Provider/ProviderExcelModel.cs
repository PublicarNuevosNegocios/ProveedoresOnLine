using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderExcelModel
    {
        public string ProviderPublicId { get; set; }
        public string BlackListStatus { get; set; }

        public string RazonSocial { get; set; }
        public string IdentificationNumber { get; set; }
        public string Cargo { get; set; }
        public string Estado { get; set; }
    }
}
