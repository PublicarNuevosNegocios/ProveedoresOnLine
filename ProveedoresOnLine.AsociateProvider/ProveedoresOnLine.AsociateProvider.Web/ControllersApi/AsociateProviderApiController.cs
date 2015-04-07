using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProveedoresOnLine.AsociateProvider.Web.ControllersApi
{
    public class AsociateProviderApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<ProveedoresOnLine.AsociateProvider.Interfaces.ViewModel.AsociateProviderViewModel> GetAllAsociateProvider
            (string GetAllAsociateProvider,
            string SearchParam,
            string PageNumber,
            string RowCount)
        {
            List<ProveedoresOnLine.AsociateProvider.Interfaces.ViewModel.AsociateProviderViewModel> oReturn = new List<Interfaces.ViewModel.AsociateProviderViewModel>();

            if (GetAllAsociateProvider == "true")
            {
                int oPageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber.Trim());

                int oRowCount = Convert.ToInt32(string.IsNullOrEmpty(RowCount) ? 10 : Convert.ToInt32(RowCount.Trim()));

                int TotalRows;

                List<ProveedoresOnLine.AsociateProvider.Interfaces.Models.AsociateProvider.AsociateProviderModel> oSearchResult = ProveedoresOnLine.AsociateProvider.DAL.Controller.AsociateProviderDataController.Instance.GetAllAsociateProvider
                    (SearchParam, oPageNumber, oRowCount, out TotalRows);

                if (oSearchResult != null && oSearchResult.Count > 0)
                {
                    oSearchResult.All(x =>
                    {
                        oReturn.Add(new Interfaces.ViewModel.AsociateProviderViewModel(x, TotalRows));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public void AsociateProviderUpsert
            (string AsociateProviderUpsert)
        {
            if (AsociateProviderUpsert == "true" &&
                !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DataToUpsert"]))
            {
                ProveedoresOnLine.AsociateProvider.Interfaces.ViewModel.AsociateProviderViewModel oDataToUpsert =
                    (ProveedoresOnLine.AsociateProvider.Interfaces.ViewModel.AsociateProviderViewModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(System.Web.HttpContext.Current.Request["DataToUpsert"],
                                typeof(ProveedoresOnLine.AsociateProvider.Interfaces.ViewModel.AsociateProviderViewModel));

                ProveedoresOnLine.AsociateProvider.DAL.Controller.AsociateProviderDataController.Instance.AsociateProviderUpsertEmail(Convert.ToInt32(oDataToUpsert.AP_AsociateProviderId.Trim()), oDataToUpsert.AP_Email);
            }
        }
    }
}