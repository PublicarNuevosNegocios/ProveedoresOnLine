﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Models
{
    public class AsociateProviderModel
    {
        public int AsociateProviderId { get; set; }

        public ProviderModel RelatedProviderBO { get; set; }

        public ProviderModel RelatedProviderDM { get; set; }
        
        public string Email { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
