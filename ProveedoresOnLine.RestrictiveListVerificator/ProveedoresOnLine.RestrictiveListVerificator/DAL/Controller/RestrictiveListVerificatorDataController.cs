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

        #region Excel 

        public List<ProviderStatusModel> getProviderByStatus(int Status, string CustomerPublicId)
        {
             return DataFactory.getProviderByStatus(Status, CustomerPublicId);
        }

        #endregion

    }
}
