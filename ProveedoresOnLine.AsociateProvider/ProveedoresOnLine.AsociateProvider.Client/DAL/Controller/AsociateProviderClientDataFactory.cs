using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.DAL.Controller
{
    internal class AsociateProviderClientDataFactory
    {
        public ProveedoresOnLine.AsociateProvider.Client.Interfaces.IAsociateProviderClientData GetAsociateProviderClientInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.AsociateProvider.Client.DAL.MySQLDAO.AsociateProviderClient_MySqlDao,ProveedoresOnLine.AsociateProvider.Client");
            ProveedoresOnLine.AsociateProvider.Client.Interfaces.IAsociateProviderClientData oRetorno = (ProveedoresOnLine.AsociateProvider.Client.Interfaces.IAsociateProviderClientData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
