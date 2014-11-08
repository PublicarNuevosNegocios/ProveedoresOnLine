using DocumentManagement.Models.Provider;
using DocumentManagement.Provider.Models.Provider;
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
            (Request["divGridProvider_txtSearch"], Request["CustomerName"], Request["FormId"], 0, 65000, out oTotalRows, Convert.ToBoolean(Request["chk_Unique"]));

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
                string Campana = "";

                ProviderInfoModel info = item.RelatedProvider.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x).FirstOrDefault() == null ? null : item.RelatedProvider.RelatedProviderCustomerInfo.Where(x => x.ProviderInfoType.ItemId == 403).Select(x => x).FirstOrDefault();
                if (info != null && info.ProviderInfoType.ItemId == 403)
                    Campana = info.Value;
                else
                    Campana = "N/A";

                if (oReturn.RelatedProvider.IndexOf(item) == 0)
                {
                    data.AppendLine
                        ("\"" + "RAZON SOCIAL" + "\"" + strSep +
                        "\"" + "TIPO IDENTIFICACION" + "\"" + strSep +
                        "\"" + "NUMERO IDENTIFICACION" + "\"" + strSep +
                        "\"" + "EMAIL" + "\"" + strSep +
                        "\"" + "CAMPANA" + "\"" + strSep +
                        "\"" + "URL" + "\"");
                }
                else
                {
                    data.AppendLine
                        ("\"" + item.RelatedProvider.Name + "\"" + "" + strSep +
                        "\"" + item.RelatedProvider.IdentificationType.ItemName + "\"" + strSep +
                        "\"" + item.RelatedProvider.IdentificationNumber + "\"" + strSep +
                        "\"" + item.RelatedProvider.Email + "\"" + strSep +
                        "\"" + Campana + "\"" + strSep +
                        "\"" + Url.Action(MVC.ProviderForm.ActionNames.Index,
                                    MVC.ProviderForm.Name,
                                    new
                                    {
                                        ProviderPublicId = item.RelatedProvider.ProviderPublicId,
                                        FormPublicId = item.RelatedProvider.FormPublicId
                                    }) + "\"");
                }
            }

            byte[] buffer = Encoding.ASCII.GetBytes(data.ToString().ToCharArray());

            return File(buffer, "application/csv", "Proveedores_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv");
        }

        public virtual ActionResult UpdateProvider()
        {
            ProviderModel oModel = new ProviderModel();
            oModel = GetRequestProvider();

            DocumentManagement.Provider.Controller.Provider.ProviderUpsert(oModel);

            return RedirectToAction(MVC.Provider.ActionNames.Index, MVC.Provider.Name);
        }
        
        #region Private Functions

        private ProviderModel GetRequestProvider()
        {
            ProviderModel oreturn = new ProviderModel();

            oreturn.ProviderPublicId = Request["ProviderPublicIdEdit"];
            oreturn.Name = Request["RazonSocial"];
            oreturn.IdentificationType = new CatalogModel();
            oreturn.IdentificationType.ItemId = int.Parse(Request["TipoIdentificacion"]);
            oreturn.IdentificationNumber = Request["NumeroIdentificacion"];
            oreturn.Email = Request["Email"];
            oreturn.CustomerPublicId = Request["ProviderCustomerIdEdit"];
            oreturn.RelatedProviderCustomerInfo = new List<ProviderInfoModel>();
            ProviderInfoModel info = new ProviderInfoModel();
            info.ProviderInfoType = new CatalogModel();
            info.ProviderInfoType.ItemId = 403;
            info.ProviderInfoId = int.Parse(Request["ProviderInfoIdEdit"]);
            info.Value = Request["SalesForceCode"];
            oreturn.RelatedProviderCustomerInfo.Add(info);            

            return oreturn;
        }

        #endregion
    }
}