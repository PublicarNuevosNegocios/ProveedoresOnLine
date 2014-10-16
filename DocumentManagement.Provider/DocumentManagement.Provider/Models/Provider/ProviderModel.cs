using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Models.Provider
{
    public class ProviderModel
    {
        public string ProviderPublicId { get; set; }

        public string Name { get; set; }

        public CatalogModel IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public string Email { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
