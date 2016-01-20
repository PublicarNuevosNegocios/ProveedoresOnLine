using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.DAL.Controller
{
    internal class AsociateProviderClientController : ProveedoresOnLine.AsociateProvider.Client.Interfaces.IAsociateProviderClientData
    {
        #region singleton instance
        private static ProveedoresOnLine.AsociateProvider.Client.Interfaces.IAsociateProviderClientData oInstance;

        internal static ProveedoresOnLine.AsociateProvider.Client.Interfaces.IAsociateProviderClientData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new AsociateProviderClientController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.AsociateProvider.Client.Interfaces.IAsociateProviderClientData DataFactory;

        #endregion

        #region Constructor

        public AsociateProviderClientController()
        {
            AsociateProviderClientDataFactory factory = new AsociateProviderClientDataFactory();
            DataFactory = factory.GetAsociateProviderClientInstance();
        }

        #endregion

        #region Asociate Provider

        public int DMProviderUpsert(string CustomerPublicId, string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber)
        {
            return DataFactory.DMProviderUpsert(CustomerPublicId, ProviderPublicId, ProviderName, IdentificationType, IdentificationNumber);
        }

        public int BOProviderUpsert(string ProviderPublicId, string ProviderName, string IdentificationType, string IdentificationNumber)
        {
            return DataFactory.BOProviderUpsert(ProviderPublicId, ProviderName, IdentificationType, IdentificationNumber);
        }

        public void AP_AsociateProviderUpsert(string BOProviderPublicId, string DMProviderPublicId, string Email)
        {
            DataFactory.AP_AsociateProviderUpsert(BOProviderPublicId, DMProviderPublicId, Email);
        }

        public ProveedoresOnLine.AsociateProvider.Client.Models.HomologateModel GetHomologateItemBySourceID(Int32 SourceCode)
        {
            return DataFactory.GetHomologateItemBySourceID(SourceCode);
        }

        #endregion
    }
}
