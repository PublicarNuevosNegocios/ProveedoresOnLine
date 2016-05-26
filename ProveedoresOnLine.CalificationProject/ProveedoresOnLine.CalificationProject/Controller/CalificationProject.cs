using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Controller
{
    public class CalificationProject
    {
        #region CalificationProjectConfigModule

        #region ProjectConfig

        public static CalificationProjectConfigModel CalificationProjectConfigUpsert(CalificationProjectConfigModel oConfigProject)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                if (oConfigProject != null)
                {
                    oConfigProject.CalificationProjectConfigId = DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigUpsert
                        (
                            oConfigProject.CalificationProjectConfigId,
                            oConfigProject.CalificationProjectConfigName,
                            oConfigProject.Company.CompanyPublicId,
                            oConfigProject.Enable
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
                oLog.LogObject = oConfigProject;
                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CalificationProjectConfigId",
                    Value = oConfigProject.CalificationProjectConfigId.ToString()
                });
                LogManager.ClientLog.AddLog(oLog);
            }
            return oConfigProject;
        }

        public static List<CalificationProjectConfigModel> CalificationProjectConfigGetByCompanyId(string Company, bool Enable)
        {
            return DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfig_GetByCompanyId(Company, Enable);
        }

        #endregion

        #region ConfigItem

        public static ConfigItemModel CalificationProjectConfigItemUpsert(ConfigItemModel oConfigItemModel)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                if (oConfigItemModel != null)
                {
                    oConfigItemModel.CalificationProjectConfigItemId = DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigItemUpsert
                        (oConfigItemModel.CalificationProjectConfigId,
                        oConfigItemModel.CalificationProjectConfigItemId,
                        oConfigItemModel.CalificationProjectConfigItemName,
                        oConfigItemModel.CalificationProjectConfigItemType.ItemId,
                        oConfigItemModel.Enable);

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
                oLog.LogObject = oConfigItemModel;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CalificationProjectConfigItemId",
                    Value = oConfigItemModel.CalificationProjectConfigItemId.ToString(),
                });

                LogManager.ClientLog.AddLog(oLog);
            }

            return oConfigItemModel;
        }

        public static List<ConfigItemModel> CalificationProjectConfigItem_GetByCalificationProjectConfigId(int CalificationProjectConfigId, bool Enable)
        {
            return DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigItem_GetByCalificationProjectConfigId(CalificationProjectConfigId, Enable);
        }

        #endregion

        #region ConfigItemInfo

        public static ConfigItemModel CalificationProjectConfigItemInfoUpsert(ConfigItemModel oConfigItemModel)
        {
            if (oConfigItemModel != null &&
                oConfigItemModel.CalificationProjectConfigItemInfoModel != null &&
                oConfigItemModel.CalificationProjectConfigItemInfoModel.Count > 0)
            {
                oConfigItemModel.CalificationProjectConfigItemInfoModel.All(cinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        cinf.CalificationProjectConfigItemInfoId = DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigItemInfoUpsert
                        (cinf.CalificationProjectConfigItemId,
                        cinf.CalificationProjectConfigItemInfoId,
                        cinf.Question.ItemId,
                        cinf.Rule.ItemId,
                        cinf.ValueType.ItemId,
                        cinf.Value,
                        cinf.Score,
                        cinf.Enable);

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
                        oLog.LogObject = cinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "SurveyConfigId",
                            Value = cinf.CalificationProjectConfigItemInfoId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return oConfigItemModel;
        }

        public static List<ConfigItemInfoModel> CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(int CalificationProjectConfigItemId, bool Enable)
        {
            return DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(CalificationProjectConfigItemId, Enable);
        }

        #endregion

        #region ConfigValidate

        public static ConfigValidateModel CalificationProjectConfigValidate_Upsert(ConfigValidateModel oConfigValidateModel)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {
                if (oConfigValidateModel != null)
                {
                    oConfigValidateModel.CalificationProjectConfigValidateId = DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigValidateUpsert
                        (
                        oConfigValidateModel.CalificationProjectConfigValidateId,
                            oConfigValidateModel.CalificationProjectConfigId,
                            oConfigValidateModel.Operator.ItemId,
                            oConfigValidateModel.Value,
                            oConfigValidateModel.Result,
                            oConfigValidateModel.Enable
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
                oLog.LogObject = oConfigValidateModel;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CalificationProjectValidateId",
                    Value = oConfigValidateModel.CalificationProjectConfigValidateId.ToString(),
                });

                LogManager.ClientLog.AddLog(oLog);
            }
            return oConfigValidateModel;
        }

        public static List<ConfigValidateModel> CalificationProjectValidate_GetByProjectConfigId(int ConfigProjectId, bool Enable)
        {
            return DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigValidate_GetByProjectConfigId(ConfigProjectId, Enable);
        }

        #endregion

        #endregion
    }
}
