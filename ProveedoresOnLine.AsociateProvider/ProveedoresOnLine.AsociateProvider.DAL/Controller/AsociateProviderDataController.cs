using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.DAL.Controller
{
    public class AsociateProviderDataController : AsociateProvider.Interfaces.IAsociateProviderData
    {
        #region Singleton Instance

        private static AsociateProvider.Interfaces.IAsociateProviderData oInstance;
        public static AsociateProvider.Interfaces.IAsociateProviderData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new AsociateProviderDataController();
                return oInstance;
            }
        }

        private AsociateProvider.Interfaces.IAsociateProviderData DataFactory;

        #endregion

        #region Constructor

        public AsociateProviderDataController()
        {
            AsociateProviderDataFactory factory = new AsociateProviderDataFactory();
            DataFactory = factory.GetDataInstance();
        }

        #endregion

        #region Asociate Provider

        public List<Interfaces.Models.AsociateProvider.AsociateProviderModel> GetAllAsociateProvider(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.GetAllAsociateProvider(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public List<Interfaces.Models.AsociateProvider.AsociateProviderModel> GetAsociateProviderByProviderPublicId(string vProviderPublicIdDM, string vProviderPublicIdBO)
        {
            return DataFactory.GetAsociateProviderByProviderPublicId(vProviderPublicIdDM, vProviderPublicIdBO);
        }

        public int AsociateProviderUpsertEmail(int AsociateProviderId, string Email)
        {
            return DataFactory.AsociateProviderUpsertEmail(AsociateProviderId, Email);
        }

        #endregion
    }
}
