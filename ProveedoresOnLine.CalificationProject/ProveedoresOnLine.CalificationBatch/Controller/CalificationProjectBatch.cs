using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;

namespace ProveedoresOnLine.CalificationBatch.Controller
{
    public class CalificationProjectBatch
    {
        #region CalificationBatch

        public static List<Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool Enable)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProject_GetByCustomer(vCustomerPublicid,vProviderPublicId,Enable);
        }

        public static CalificationProjectBatchModel CalificationProjectUpsert(CalificationProjectBatchModel oCalProject) 
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                if (oCalProject != null)
                {
                    oCalProject.CalificationProjectId = DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProjectUpsert
                        (
                            oCalProject.CalificationProjectPublicId,
                            oCalProject.ProjectConfigModel.CalificationProjectConfigId,
                            oCalProject.Company.CompanyPublicId,
                            oCalProject.TotalScore,
                            oCalProject.Enable
                        );
                    oLog.IsSuccess = true;
                }
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;
                throw err;
            }
            finally
            {
                oLog.LogObject = oCalProject;
                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CalificationProjectId",
                    Value = oCalProject.CalificationProjectId.ToString()
                });
                LogManager.ClientLog.AddLog(oLog);
            }
            return oCalProject;
        }

        public static CalificationProjectItemBatchModel CalificatioProjectItemUpsert(CalificationProjectItemBatchModel oCalItemProject) 
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                if (oCalItemProject != null)
                {
                    oCalItemProject.CalificationProjectItemId = DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProjectItemUpsert
                    (
                        oCalItemProject.CalificationProjectItemId,
                        oCalItemProject.CalificationProjectId,
                        oCalItemProject.CalificationProjectConfigItem.CalificationProjectConfigItemId,
                        oCalItemProject.ItemScore,
                        oCalItemProject.Enable
                    );
                    oLog.IsSuccess = true;
                }
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;
                throw err;
            }
            finally
            {
                oLog.LogObject = oCalItemProject;
                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CalificationProjectItemId",
                    Value = oCalItemProject.CalificationProjectItemId.ToString()
                });
                LogManager.ClientLog.AddLog(oLog);
            }
            return oCalItemProject;
        }

        public static CalificationProjectItemInfoBatchModel CalificationProjectItemInfoUpsert(CalificationProjectItemInfoBatchModel oCalInfoItemProject) 
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                if (oCalInfoItemProject != null)
                {
                    oCalInfoItemProject.CalificationProjectItemId = DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProjectItemInfoUpsert
                    (
                        oCalInfoItemProject.CalificationProjectItemInfoId,
                        oCalInfoItemProject.CalificationProjectItemId,
                        oCalInfoItemProject.CalificationProjectConfigItemInfo.CalificationProjectConfigItemInfoId,
                        oCalInfoItemProject.ItemInfoScore,
                        oCalInfoItemProject.Enable
                    );
                    oLog.IsSuccess = true;
                }
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;
                throw err;
            }
            finally
            {
                oLog.LogObject = oCalInfoItemProject;
                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CalificationProjectItemInfoId",
                    Value = oCalInfoItemProject.CalificationProjectItemInfoId.ToString()
                });
                LogManager.ClientLog.AddLog(oLog);
            }
            return oCalInfoItemProject;
        }
        #endregion

        #region CalificationProjectBarchUtil

        #region LegalModule

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel LegalModuleInfo(string CompanyPublicId, int LegalInfoType)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.LegalModuleInfo(CompanyPublicId, LegalInfoType);
        }

        #endregion

        #endregion
    }
}
