using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Interfaces
{
    internal interface IAsociateProviderClientData
    {
        int BOProviderUpsert();

        int DMProviderUpsert();

        void AP_AsociateProvider();
    }
}
