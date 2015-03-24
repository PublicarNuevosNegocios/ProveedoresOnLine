﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Interfaces
{
    internal interface IAsociateProviderClientData
    {
        int DMProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumer);

        int BOProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber);
        
        void AP_AsociateProviderUpsert(string BOProviderPublicId, string DMProviderPublicId, string Email);
    }
}
