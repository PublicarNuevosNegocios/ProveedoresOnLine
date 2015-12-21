using DocumentManagement.Provider.Models;
using DocumentManagement.Provider.Models.Provider;
using ProveedoresOnLine.AsociateProvider.Client.Models;
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

                ProviderToUpsert.ProviderPublicId = oResult;
                ProviderInfoUpsert(ProviderToUpsert);
                ProviderCustomerInfoUpsert(ProviderToUpsert);

                AsociateProviderModel AsociateProviderToUpsert = new AsociateProviderModel()
                {
                    RelatedProviderDM = new RelatedProviderModel()
                    {
                        ProviderPublicId = oResult,
                        ProviderName = ProviderToUpsert.Name,
                        IdentificationType = ProviderToUpsert.IdentificationType.ItemId.ToString(),
                        IdentificationNumber = ProviderToUpsert.IdentificationNumber,
                    },
                };

                ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.ProviderUpsertDM(AsociateProviderToUpsert);

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
                    item.ProviderInfoId = DAL.Controller.ProviderDataController.Instance.ProviderInfoUpsert
                        (ProviderToUpsert.ProviderPublicId,
                        item.ProviderInfoId,
                        item.ProviderInfoType.ItemId,
                        item.Value,
                        item.LargeValue);

                    LogManager.Models.LogModel oItemLog = GetLogModel();
                    oItemLog.IsSuccess = true;
                    oItemLog.LogObject = item;
                    oItemLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "ProviderInfoType.ItemId",
                        Value = item.ProviderInfoType.ItemId.ToString(),
                    });
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
                    item.ProviderInfoId = DAL.Controller.ProviderDataController.Instance.ProviderCustomerInfoUpsert
                         (ProviderToUpsert.ProviderPublicId,
                         ProviderToUpsert.CustomerPublicId,
                         item.ProviderInfoId,
                         item.ProviderInfoType.ItemId,
                         item.Value,
                         item.LargeValue);

                    LogManager.Models.LogModel oItemLog = GetLogModel();
                    oItemLog.IsSuccess = true;
                    oItemLog.LogObject = item;
                    oItemLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "ProviderCustomerInfoType.ItemId",
                        Value = item.ProviderInfoType.ItemId.ToString(),
                    });
                    LogManager.ClientLog.AddLog(oItemLog);
                }
            }
        }

        static public List<ProviderModel> ProviderSearch(string SearchParam, string CustomerPublicId, string FormPublicId, int PageNumber, int RowCount, out int TotalRows, bool isUnique)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderSearch(SearchParam, CustomerPublicId, FormPublicId, PageNumber, RowCount, out TotalRows, isUnique);
        }

        public static List<LogManager.Models.LogModel> ProviderLog(string ProviderPublicId)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderLog(ProviderPublicId);
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

        public static ProviderModel ProviderGetByInfoType(string IdentificationNumber, int IdentificationTypeId, int ProviderInfoTypeId)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderGetByInfoType(IdentificationNumber, IdentificationTypeId, ProviderInfoTypeId);
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

        #region ChangesControl

        public static string ChangesControlUpsert(ChangesControlModel ChangesControlToUpsert)
        {

            string oResult = null;
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                oResult = DAL.Controller.ProviderDataController.Instance.ChangesControlUpsert
                    (ChangesControlToUpsert.ChangesPublicId
                    , ChangesControlToUpsert.ProviderInfoId
                    , ChangesControlToUpsert.FormUrl
                    , ChangesControlToUpsert.Status.ItemId
                    , ChangesControlToUpsert.Enable);

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
                oLog.LogObject = ChangesControlToUpsert;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "ChangesPublicId",
                    Value = oResult,
                });

                LogManager.ClientLog.AddLog(oLog);
            }
            return oResult;
        }

        public static List<ChangesControlModel> ChangesControlSearch(string SearchParam, string ProviderPublicId, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.ProviderDataController.Instance.ChangesControlSearch(SearchParam, ProviderPublicId, PageNumber, RowCount, out TotalRows);
        }
        #endregion
    }
}
