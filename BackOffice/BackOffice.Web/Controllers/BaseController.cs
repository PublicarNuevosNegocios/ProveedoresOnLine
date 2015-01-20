using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class BaseController : Controller
    {
        #region public static properties

        public static string CurrentControllerName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public static string CurrentActionName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }

        #endregion

        #region generic file actions

        public virtual FileResult GetPdfFileBytes(string FilePath)
        {
            byte[] bytes = new byte[] { };
            if (!string.IsNullOrEmpty(FilePath) && FilePath.IndexOf(".pdf") > 0)
            {
                bytes = (new System.Net.WebClient()).DownloadData(FilePath);
            }
            return File(bytes, "application/pdf");
        }

        #endregion

        #region generic Currency methods

        public static List<Tuple<decimal, decimal>> Currency_ConvertToStandar
            (int MoneyFrom,
            int MoneyTo,
            int Year,
            List<decimal> OriginalValues)
        {
            List<Tuple<decimal, decimal>> oReturn = new List<Tuple<decimal, decimal>>();

            if (OriginalValues != null && OriginalValues.Count > 0)
            {
                //get rate
                List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> olstCurrency =
                    ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetByMoneyType
                        (MoneyFrom, MoneyTo, null);

                ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel oCurrency = null;

                if (olstCurrency != null && olstCurrency.Count > 0)
                {
                    //get rate for year or current year
                    oCurrency = olstCurrency.Any(x => x.IssueDate.Year == Year) ?
                        olstCurrency.Where(x => x.IssueDate.Year == Year).FirstOrDefault() :
                        olstCurrency.Where(x => x.IssueDate.Year == DateTime.Now.Year).FirstOrDefault();
                }

                if (oCurrency == null)
                {
                    //rate not found
                    oCurrency = new ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel()
                    {
                        Rate = 0,
                    };
                }

                OriginalValues.All(ov =>
                {
                    oReturn.Add(new Tuple<decimal, decimal>(ov, (decimal)(ov * oCurrency.Rate)));
                    return true;
                });
            }
            return oReturn;
        }

        #endregion
    }
}