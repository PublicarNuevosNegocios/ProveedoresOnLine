using ProveedoresOnLine.AsociateProvider.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Controller
{
    public class AsociateProviderClient
    {
        public static void AsociateProvider(AsociateProviderModel AsociateProviderToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();

            if (AsociateProviderToUpsert.RelatedProviderBO != null &&
                AsociateProviderToUpsert.RelatedProviderDM != null)
            {
                try
                {
                    int BOProviderUpsert = ProviderUpsertBO(AsociateProviderToUpsert);

                    int DMProviderUpsert = ProviderUpsertDM(AsociateProviderToUpsert);

                    DAL.Controller.AsociateProviderClientController.Instance.AP_AsociateProviderUpsert(
                        AsociateProviderToUpsert.RelatedProviderBO.ProviderPublicId,
                        AsociateProviderToUpsert.RelatedProviderDM.ProviderPublicId,
                        AsociateProviderToUpsert.Email);

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
                    oLog.LogObject = AsociateProviderToUpsert;
                    LogManager.ClientLog.AddLog(oLog);
                }
            }
        }
        
        public static int ProviderUpsertBO(AsociateProviderModel AsociateProviderToUpsert)
        {
            int oReturn = 0;
            
            if (AsociateProviderToUpsert != null &&
                AsociateProviderToUpsert.RelatedProviderBO != null)
            {
                LogManager.Models.LogModel oLog = GetGenericLogModel();
                try
                {
                    oReturn = DAL.Controller.AsociateProviderClientController.Instance.BOProviderUpsert(
                    AsociateProviderToUpsert.RelatedProviderBO.ProviderPublicId,
                    AsociateProviderToUpsert.RelatedProviderBO.ProviderName,
                    AsociateProviderToUpsert.RelatedProviderBO.IdentificationType,
                    AsociateProviderToUpsert.RelatedProviderBO.IdentificationNumber);

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
                    oLog.LogObject = AsociateProviderToUpsert;
                    LogManager.ClientLog.AddLog(oLog);
                }
            }

            return oReturn;
        }

        public static int ProviderUpsertDM(AsociateProviderModel AsociateProviderToUpsert)
        {
            int oReturn = 0;

            if (AsociateProviderToUpsert != null &&
                AsociateProviderToUpsert.RelatedProviderDM != null)
            {
                LogManager.Models.LogModel oLog = GetGenericLogModel();
                try
                {
                    oReturn = DAL.Controller.AsociateProviderClientController.Instance.DMProviderUpsert(
                    AsociateProviderToUpsert.RelatedProviderDM.ProviderPublicId,
                    AsociateProviderToUpsert.RelatedProviderDM.ProviderName,
                    AsociateProviderToUpsert.RelatedProviderDM.IdentificationType,
                    AsociateProviderToUpsert.RelatedProviderDM.IdentificationNumber);

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
                    oLog.LogObject = AsociateProviderToUpsert;
                    LogManager.ClientLog.AddLog(oLog);
                }
            }

            return oReturn;
        }

        #region Generic Log

        public static LogManager.Models.LogModel GetGenericLogModel()
        {
            LogManager.Models.LogModel oReturn = new LogManager.Models.LogModel()
            {
                RelatedLogInfo = new List<LogManager.Models.LogInfoModel>(),
            };

            try
            {
                System.Diagnostics.StackTrace oStack = new System.Diagnostics.StackTrace();

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

                    //get source invocation
                    oReturn.Source = System.Web.HttpContext.Current.Request.Url.ToString();
                }
                else if (oStack.FrameCount > 0)
                {
                    oReturn.Source = oStack.GetFrame(oStack.FrameCount - 1).GetMethod().Module.Assembly.Location;
                }

                //get appname
                if (oStack.FrameCount > 1)
                {
                    oReturn.Application = oStack.GetFrame(1).GetMethod().Module.Name + " - " + oStack.GetFrame(1).GetMethod().Name;
                }
                else
                {
                    oReturn.Application = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                }
            }
            catch { }

            return oReturn;
        }

        #endregion
    }
}
