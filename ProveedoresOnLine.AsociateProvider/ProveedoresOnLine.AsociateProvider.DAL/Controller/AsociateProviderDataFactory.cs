using ProveedoresOnLine.AsociateProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.DAL.Controller
{
    internal class AsociateProviderDataFactory
    {
        public IAsociateProviderData GetDataInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.AsociateProvider.DAL.MySqlDao.AsociateProvider_MySqlDao,ProveedoresOnLine.AsociateProvider.DAL");
            IAsociateProviderData oRetorno = (IAsociateProviderData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
