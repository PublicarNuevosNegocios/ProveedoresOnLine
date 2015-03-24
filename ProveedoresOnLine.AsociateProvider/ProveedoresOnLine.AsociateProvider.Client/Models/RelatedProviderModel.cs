using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Models
{
    public class RelatedProviderModel
    {
        public int ProviderId { get; set; }

        public string ProviderPublicId { get; set; }

        public string ProviderName { get; set; }

        public string IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
