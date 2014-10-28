using DocumentManagement.Customer.Models.Customer;
using DocumentManagement.Customer.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.Controller
{
    public class Customer
    {
        #region Customer

        public static string CustomerUpsert(CustomerModel CustomerToUpsert)
        {
            string oReturn = null;
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                oReturn = DAL.Controller.CustomerDataController.Instance.CustomerUpsert
                    (CustomerToUpsert.CustomerPublicId,
                    CustomerToUpsert.Name,
                    CustomerToUpsert.IdentificationType.ItemId,
                    CustomerToUpsert.IdentificationNumber);

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
                oLog.LogObject = CustomerToUpsert;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CustomerPublicId",
                    Value = oReturn,
                });

                LogManager.ClientLog.AddLog(oLog);
            }

            return oReturn;
        }

        public static List<Models.Customer.CustomerModel> CustomerSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerSearch
                (SearchParam,
                PageNumber,
                RowCount,
                out TotalRows);
        }

        public static Models.Customer.CustomerModel CustomerGetByFormId(string FormPublicId)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerGetByFormId(FormPublicId);
        }

        public static Models.Customer.CustomerModel CustomerGetById(string CustomerPublicId)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerGetById(CustomerPublicId);
        }

        #endregion

        #region Form

        public static string FormUpsert(string CustomerPublicId, FormModel FormToUpsert)
        {
            string oReturn = null;
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                oReturn = DAL.Controller.CustomerDataController.Instance.FormUpsert
                (FormToUpsert.FormPublicId,
                CustomerPublicId,
                FormToUpsert.Name);

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
                oLog.LogObject = FormToUpsert;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "FormPublicId",
                    Value = oReturn,
                });

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CustomerPublicId",
                    Value = CustomerPublicId,
                });

                LogManager.ClientLog.AddLog(oLog);
            }

            return oReturn;

        }

        public static void FormUpsertLogo(string FormPublicId, string Logo)
        {
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                DAL.Controller.CustomerDataController.Instance.FormUpsertLogo
                (FormPublicId,
                Logo);

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
                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "FormPublicId",
                        Value = FormPublicId,
                    });

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "Logo",
                        Value = Logo,
                    });

                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static int StepCreate(string FormPublicId, StepModel StepToUpsert)
        {
            int oReturn = 0;
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                oReturn = DAL.Controller.CustomerDataController.Instance.StepCreate
                (FormPublicId,
                StepToUpsert.Name,
                StepToUpsert.Position);

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
                oLog.LogObject = StepToUpsert;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "FormPublicId",
                    Value = FormPublicId,
                });

                LogManager.ClientLog.AddLog(oLog);
            }

            return oReturn;
        }

        public static void StepModify(StepModel StepToUpsert)
        {
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                DAL.Controller.CustomerDataController.Instance.StepModify
                (StepToUpsert.StepId,
                StepToUpsert.Name,
                StepToUpsert.Position);

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
                oLog.LogObject = StepToUpsert;

                LogManager.ClientLog.AddLog(oLog);
            }

        }

        public static void StepDelete(int StepId)
        {
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                DAL.Controller.CustomerDataController.Instance.StepDelete(StepId);

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
                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "StepId",
                    Value = StepId.ToString(),
                });

                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static int FieldCreate(int StepId, FieldModel FieldToUpsert)
        {
            int oReturn = 0;
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                oReturn = DAL.Controller.CustomerDataController.Instance.FieldCreate
                    (StepId,
                    FieldToUpsert.Name,
                    FieldToUpsert.ProviderInfoType.ItemId,
                    FieldToUpsert.IsRequired,
                    FieldToUpsert.Position);

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
                oLog.LogObject = FieldToUpsert;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "StepId",
                    Value = StepId.ToString(),
                });

                LogManager.ClientLog.AddLog(oLog);
            }

            return oReturn;
        }

        public static void FieldDelete(int FieldId)
        {
            LogManager.Models.LogModel oLog = GetLogModel();

            try
            {
                DAL.Controller.CustomerDataController.Instance.FieldDelete(FieldId);

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
                oLog.LogInfoObject.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "FieldId",
                    Value = FieldId.ToString(),
                });

                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static List<DocumentManagement.Customer.Models.Form.FormModel> FormSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CustomerDataController.Instance.FormSearch
                (CustomerPublicId,
                SearchParam,
                PageNumber,
                RowCount,
                out TotalRows);
        }

        public static List<Models.Form.StepModel> StepGetByFormId(string FormPublicId)
        {
            return DAL.Controller.CustomerDataController.Instance.StepGetByFormId(FormPublicId);
        }

        public static List<Models.Form.FieldModel> FieldGetByStepId(int StepId)
        {
            return DAL.Controller.CustomerDataController.Instance.FieldGetByStepId(StepId);
        }

        #endregion

        #region Util

        public static List<Models.Util.CatalogModel> CatalogGetCustomerOptions()
        {
            return DAL.Controller.CustomerDataController.Instance.CatalogGetCustomerOptions();
        }

        #endregion

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
                    oReturn.Application = System.AppDomain.CurrentDomain.FriendlyName;

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
