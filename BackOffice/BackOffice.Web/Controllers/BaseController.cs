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

        //public static List<Tuple<decimal, decimal>> Currency_ConvertToStandar
        //    (int MoneyFrom,
        //    int MoneyTo,
        //    int Year,
        //    List<decimal> OriginalValues)
        //{
        //    List<Tuple<decimal, decimal>> oReturn = new List<Tuple<decimal, decimal>>();

        //    if (OriginalValues != null && OriginalValues.Count > 0)
        //    {
        //        //get rate
        //        List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> olstCurrency =
        //            ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetByMoneyType
        //                (MoneyFrom, MoneyTo, null);

        //        ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel oCurrency = null;

        //        if (olstCurrency != null && olstCurrency.Count > 0)
        //        {
        //            //get rate for year or current year
        //            oCurrency = olstCurrency.Any(x => x.IssueDate.Year == Year) ?
        //                olstCurrency.Where(x => x.IssueDate.Year == Year).FirstOrDefault() :
        //                olstCurrency.Where(x => x.IssueDate.Year == DateTime.Now.Year).FirstOrDefault();
        //        }

        //        if (oCurrency == null)
        //        {
        //            //rate not found
        //            oCurrency = new ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel()
        //            {
        //                Rate = 0,
        //            };
        //        }

        //        OriginalValues.All(ov =>
        //        {
        //            oReturn.Add(new Tuple<decimal, decimal>(ov, (decimal)(ov * oCurrency.Rate)));
        //            return true;
        //        });
        //    }
        //    return oReturn;
        //}

        public static decimal Currency_GetRate
                    (int MoneyFrom,
                    int MoneyTo,
                    int Year)
        {
            ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel oCurrency = null;

            if (MoneyFrom != MoneyTo)
            {
                //get rate
                List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> olstCurrency =
                    ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetByMoneyType
                        (MoneyFrom, MoneyTo, null);

                if (olstCurrency != null && olstCurrency.Count > 0)
                {
                    //get rate for year or current year
                    oCurrency = olstCurrency.Any(x => x.IssueDate.Year == Year) ?
                        olstCurrency.Where(x => x.IssueDate.Year == Year).FirstOrDefault() :
                        olstCurrency.OrderByDescending(x => x.IssueDate.Year).FirstOrDefault();
                }
            }

            if (oCurrency == null)
            {
                //rate not found
                oCurrency = new ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel()
                {
                    Rate = 1,
                };
            }

            return oCurrency.Rate;
        }

        #endregion

        #region generic Math operations

        /// <summary>
        /// evaluate comparison expressions only constants
        /// </summary>
        /// <param name="Expression">5==(4+1)==(5*1)</param>
        /// <returns></returns>
        public static bool MathComparison(string Expression)
        {
            bool oReturn = true;

            decimal? oTmpResult = null;

            Expression.Split(new string[] { "==" }, StringSplitOptions.RemoveEmptyEntries).All(exp =>
            {
                decimal oCurrentResult = MathEval(exp);
                if (oTmpResult != null && oCurrentResult != oTmpResult.Value)
                {
                    oReturn = false;
                }
                oTmpResult = oCurrentResult;
                return true;
            });

            return oReturn;
        }

        /// <summary>
        /// evaluate math expression only constants
        /// </summary>
        /// <param name="Expression">3+5*(1+2)</param>
        /// <returns></returns>
        public static decimal MathEval(string Expression)
        {
            try
            {
                var oDataTable = new System.Data.DataTable();
                var oDataColumn = new System.Data.DataColumn("Eval", typeof(decimal), Expression);
                oDataTable.Columns.Add(oDataColumn);
                oDataTable.Rows.Add(0);
                return (decimal)(oDataTable.Rows[0]["Eval"]);
            }
            catch { }

            return 0;
        }

        #endregion
    }
}