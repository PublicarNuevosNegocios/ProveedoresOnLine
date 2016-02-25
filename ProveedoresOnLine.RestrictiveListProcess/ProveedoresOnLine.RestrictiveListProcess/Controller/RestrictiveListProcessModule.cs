using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Controller
{
    public class RestrictiveListProcessModule
    {
        #region Provider Functions
            public static List<ProviderModel> GetProviderByStatus(int Status, string CustomerPublicId)
            {
                List<ProviderModel> objProviderModel = DAL.Controller.RestrictiveListProcessDataController.Instance.GetProviderByStatus(Status, CustomerPublicId);
                return objProviderModel;
            }
        #endregion
    }
}
