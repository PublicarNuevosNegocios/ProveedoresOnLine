using DocumentManagement.Provider.Models;
using DocumentManagement.Provider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Controller
{
    public class Provider
    {
        static public string ProviderUpsert(ProviderModel ProviderToUpsert)
        {

            string oResult = null;
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                oResult = DAL.Controller.ProviderDataController.Instance.ProviderUpsert
                    (ProviderToUpsert.ProviderPublicId
                    , ProviderToUpsert.Name
                    , ProviderToUpsert.IdentificationType.ItemId
                    , ProviderToUpsert.IdentificationNumber
                    , ProviderToUpsert.Email);

                if (ProviderToUpsert.RelatedProviderInfo != null && ProviderToUpsert.RelatedProviderInfo.Count > 0)
                {
                    foreach (var item in ProviderToUpsert.RelatedProviderInfo)
                    {
                        DAL.Controller.ProviderDataController.Instance.ProviderInfoUpsert
                            (oResult,
                            item.ProviderInfoId,
                            item.ProviderInfoType.ItemId,
                            item.Value,
                            item.LargeValue);

                        LogManager.Models.LogModel oItemLog = GetLogModel();
                        oItemLog.IsSuccess = true;
                        oItemLog.LogObject = item;
                        LogManager.ClientLog.AddLog(oItemLog);
                    }
                }
                if (ProviderToUpsert.RelatedProviderCustomerInfo != null && ProviderToUpsert.RelatedProviderCustomerInfo.Count > 0)
                {
                    foreach (var item in ProviderToUpsert.RelatedProviderCustomerInfo)
                    {
                        DAL.Controller.ProviderDataController.Instance.ProviderCustomerInfoUpsert
                            (oResult,
                            ProviderToUpsert.CustomerPublicId,
                            item.ProviderInfoId,
                            item.ProviderInfoType.ItemId,
                            item.Value,
                            item.LargeValue);

                        LogManager.Models.LogModel oItemLog = GetLogModel();
                        oItemLog.IsSuccess = true;
                        oItemLog.LogObject = item;
                        LogManager.ClientLog.AddLog(oItemLog);
                    }
                }

                oLog.IsSuccess = true;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = ProviderToUpsert;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "ProviderPublicId",
                    Value = oResult,
                });

                LogManager.ClientLog.AddLog(oLog);
            }
            return oResult;
        }

        static public void ProviderInfoUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedProviderInfo != null && ProviderToUpsert.RelatedProviderInfo.Count > 0)
            {
                foreach (var item in ProviderToUpsert.RelatedProviderInfo)
                {
                    DAL.Controller.ProviderDataController.Instance.ProviderInfoUpsert
                        (ProviderToUpsert.ProviderPublicId,
                        item.ProviderInfoId,
                        item.ProviderInfoType.ItemId,
                        item.Value, item.LargeValue);

                    LogManager.Models.LogModel oItemLog = GetLogModel();
                    oItemLog.IsSuccess = true;
                    oItemLog.LogObject = item;
                    LogManager.ClientLog.AddLog(oItemLog);
                }
            }
        }

        public static void ProviderCustomerInfoUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedProviderCustomerInfo != null && ProviderToUpsert.RelatedProviderCustomerInfo.Count > 0)
            {
                foreach (var item in ProviderToUpsert.RelatedProviderCustomerInfo)
                {
                    DAL.Controller.ProviderDataController.Instance.ProviderCustomerInfoUpsert
                         (ProviderToUpsert.ProviderPublicId,
                         ProviderToUpsert.CustomerPublicId,
                         item.ProviderInfoId,
                         item.ProviderInfoType.ItemId,
                         item.Value,
                         item.LargeValue);

                    LogManager.Models.LogModel oItemLog = GetLogModel();
                    oItemLog.IsSuccess = true;
                    oItemLog.LogObject = item;
                    LogManager.ClientLog.AddLog(oItemLog);
                }
            }
        }

        static public List<ProviderModel> ProviderSearch(string SearchParam, string CustomerPublicId, string FormPublicId, int PageNumber, int RowCount, out int TotalRows, bool isUnique)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderSearch(SearchParam, CustomerPublicId, FormPublicId, PageNumber, RowCount, out TotalRows, isUnique);
        }

        public static ProviderModel ProviderGetByIdentification(string IdentificationNumber, int IdenificationTypeId, string CustomerPublicId)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderGetByIdentification(IdentificationNumber, IdenificationTypeId, CustomerPublicId);
        }

        public static ProviderModel ProviderGetById(string ProviderPublicId, int? StepId)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderGetById(ProviderPublicId, StepId);
        }

        public static Dictionary<int, List<Models.Util.CatalogModel>> CatalogGetProviderOptions()
        {
            return DAL.Controller.ProviderDataController.Instance.CatalogGetProviderOptions();
        }

        #region Log

        private static LogManager.Models.LogModel GetLogModel()
        {

            LogManager.Models.LogModel oReturn = new LogManager.Models.LogModel()
            {
                RelatedLogInfo = new List<LogManager.Models.LogInfoModel>(),
            };

            try
            {

                if (System.Web.HttpContext.Current != null)
                {
                    //get user info
                    if (SessionManager.SessionController.Auth_UserLogin != null)
                    {
                        oReturn.User = SessionManager.SessionController.Auth_UserLogin.Email;
                    }
                    else
                    {
                        oReturn.User = System.Web.HttpContext.Current.Request.UserHostAddress;
                    }

                    //get appname
                    oReturn.Application = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

                    //get source invocation
                    oReturn.Source = System.Web.HttpContext.Current.Request.Url.ToString();
                }
            }
            catch { }

            return oReturn;
        }

        #endregion
    }
}
