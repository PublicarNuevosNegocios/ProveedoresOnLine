using DocumentManagement.Models.Provider;
using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class ProviderController : BaseController
    {
        public virtual ActionResult Index()
        {
            ProviderSearchModel oModel = new ProviderSearchModel();
            int oTotalRows;
            oModel.Customers = DocumentManagement.Customer.Controller.Customer.CustomerSearch(null, 0, 20, out oTotalRows);
            //oModel.Forms = DocumentManagement.Customer.Controller.Customer.FormSearch(null, 0, 20, out oTotalRows);
            return View(oModel);
        }

        public virtual ActionResult DownloadFile()
        {
            ProviderSearchModel oReturn = new ProviderSearchModel();

            int oTotalRows;
            List<DocumentManagement.Provider.Models.Provider.ProviderModel> oProviderlst = DocumentManagement.Provider.Controller.Provider.ProviderSearch
            (Request["divGridProvider_txtSearch"], 0, 65000, out oTotalRows, Convert.ToBoolean(Request["chk_Unique"]));       
            
            if (!string.IsNullOrEmpty(Request["CustomerName"]) && !string.IsNullOrEmpty(Request["FormId"]))
            {
                oProviderlst = oProviderlst.Where(x => x.CustomerPublicId == Request["CustomerName"]
                                     && x.FormPublicId == Request["FormId"]).Select(x => x).ToList();          
            }
            if (!string.IsNullOrEmpty(Request["CustomerName"]) && string.IsNullOrEmpty(Request["FormId"]))
            {
                oProviderlst = oProviderlst.Where(x => x.CustomerPublicId == Request["CustomerName"]
                                     || x.FormPublicId == Request["FormId"]).Select(x => x).ToList();     
            }

            oReturn.RelatedProvider = new List<ProviderItemSearchModel>();
            oProviderlst.All(prv =>
            {
                oReturn.RelatedProvider.Add(new ProviderItemSearchModel()
                {
                    RelatedProvider = prv,
                });
                return true;
            });
            string strSep = ";";

            StringBuilder data = new StringBuilder();
            foreach (var item in oReturn.RelatedProvider)
            {

                data.Append(item.RelatedProvider.Name + "\"" + strSep);
                data.Append("\"" + item.RelatedProvider.IdentificationType.ItemId + "\"" + strSep);
                data.Append("\"" + item.RelatedProvider.IdentificationNumber + "\"" + strSep);
                data.Append("\"" + item.RelatedProvider.Email + "\"" + strSep);
                //data.Append(item.RelatedProvider.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType == new CatalogModel { ItemId = 401 }).Select(x => x).ToList());
                data.Append("\"" + item.FormUrl + "\"" + strSep + "\n");
                //data.Append(item.)
            }

            byte[] buffer = Encoding.ASCII.GetBytes(data.ToString().ToCharArray());

            string fileName = "ProviderFile.csv";
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}