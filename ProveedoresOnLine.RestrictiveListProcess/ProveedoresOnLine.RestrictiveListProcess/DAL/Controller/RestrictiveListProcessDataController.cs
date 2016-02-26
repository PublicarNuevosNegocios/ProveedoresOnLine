using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.DAL.Controller
{
    internal class RestrictiveListProcessDataController : ProveedoresOnLine.RestrictiveListProcess.Interfaces.IRestrictiveListProcess
    {

        #region singleton instance

        private static Interfaces.IRestrictiveListProcess oInstance;
            internal static Interfaces.IRestrictiveListProcess Instance
            {
                get
                {
                    if (oInstance == null)
                        oInstance = new RestrictiveListProcessDataController();
                    return oInstance;
                }
            }
            private Interfaces.IRestrictiveListProcess DataFactory;
        #endregion

        #region Constructor
            public RestrictiveListProcessDataController()
                {
                    RestrictiveListProcessDataFactory factory = new RestrictiveListProcessDataFactory();
                    DataFactory = factory.GetRestrictiveListProcessInstance();
                }
        #endregion

        #region Provider Functions
            public List<CompanyModel> GetProviderByStatus(int Status, string CustomerPublicId)
            {
                return DataFactory.GetProviderByStatus(Status, CustomerPublicId);
            }
            public List<RestrictiveListProcessModel> GetAllProvidersInProcess() {
                return DataFactory.GetAllProvidersInProcess();            
            }
        #endregion

    }
}
