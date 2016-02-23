using System;
using ProveedoresOnLine.RestrictiveListVerificator.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListVerificator.Models;

namespace ProveedoresOnLine.RestrictiveListVerificator.DAL.Controller
{
    internal class RestrictiveListVerificatorDataController : IRestrictiveListVerificatorData
    {
        #region singleton instance

        private static ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData oInstance;
        internal static ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new RestrictiveListVerificatorDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.RestrictiveListVerificator.Interfaces.IRestrictiveListVerificatorData DataFactory;

        #endregion

        #region Provider Functions

        public List<ProviderModel> GetProviderByStatus(int Status, string CustomerPublicId)
        {
             return DataFactory.GetProviderByStatus(Status, CustomerPublicId);
        }

        #endregion

        public RestrictiveListVerificatorDataController()
        {
            ProveedoresOnLine.RestrictiveListVerificator.DAL.Controller.RestrictiveListVerificatorDataFactory factory = new ProveedoresOnLine.RestrictiveListVerificator.DAL.Controller.RestrictiveListVerificatorDataFactory();
            DataFactory = factory.GetRestrictiveListVerificatorInstance();
        }
    }
}
