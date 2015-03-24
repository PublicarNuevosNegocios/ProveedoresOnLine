using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Models
{
    public class AsociateProviderModel
    {
        public int AsociateProviderId { get; set; }

        public RelatedProviderModel RelatedProviderBO { get; set; }

        public RelatedProviderModel RelatedProviderDM { get; set; }
        
        public string Email { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
