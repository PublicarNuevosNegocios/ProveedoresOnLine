using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.Controller
{
    public class SurveyModule
    {
        #region Survey Config

        public static ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel SurveyConfigUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel SurveyConfigToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {
                //upsert survey
                SurveyConfigToUpsert.ItemId =
                    DAL.Controller.SurveyDataController.Instance.SurveyConfigUpsert
                    (SurveyConfigToUpsert.RelatedCustomer.RelatedCompany.CompanyPublicId,
                    SurveyConfigToUpsert.ItemId > 0 ? (int?)SurveyConfigToUpsert.ItemId : null,
                    SurveyConfigToUpsert.ItemName,
                    SurveyConfigToUpsert.Enable);

                //upsert survey info
                SurveyConfigToUpsert = SurveyConfigInfoUpsert(SurveyConfigToUpsert);
                //upsert survey items
                SurveyConfigToUpsert = SurveyConfigItemUpsert(SurveyConfigToUpsert);

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
                oLog.LogObject = SurveyConfigToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }

            return SurveyConfigToUpsert;
        }

        public static ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel SurveyConfigInfoUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel SurveyConfigToUpsert)
        {
            if (SurveyConfigToUpsert.ItemId > 0 &&
                SurveyConfigToUpsert.ItemInfo != null &&
                SurveyConfigToUpsert.ItemInfo.Count > 0)
            {
                SurveyConfigToUpsert.ItemInfo.All(scinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        scinf.ItemInfoId = DAL.Controller.SurveyDataController.Instance.SurveyConfigInfoUpsert
                            (scinf.ItemInfoId > 0 ? (int?)scinf.ItemInfoId : null,
                            SurveyConfigToUpsert.ItemId,
                            scinf.ItemInfoType.ItemId,
                            scinf.Value,
                            scinf.LargeValue,
                            scinf.Enable);

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
                        oLog.LogObject = scinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "SurveyConfigId",
                            Value = SurveyConfigToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return SurveyConfigToUpsert;
        }

        public static ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel SurveyConfigItemUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel SurveyConfigToUpsert)
        {
            if (SurveyConfigToUpsert.ItemId > 0 &&
                SurveyConfigToUpsert.RelatedSurveyConfigItem != null &&
                SurveyConfigToUpsert.RelatedSurveyConfigItem.Count > 0)
            {
                SurveyConfigToUpsert.RelatedSurveyConfigItem.All(rscit =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        //upsert survey config item
                        rscit.ItemId = DAL.Controller.SurveyDataController.Instance.SurveyConfigItemUpsert
                            (rscit.ItemId > 0 ? (int?)rscit.ItemId : null,
                            SurveyConfigToUpsert.ItemId,
                            rscit.ItemName,
                            rscit.ItemType.ItemId,
                            rscit.ParentItem != null && rscit.ParentItem.ItemId > 0 ? (int?)rscit.ParentItem.ItemId : null,
                            rscit.Enable);

                        //upsert survey config item info
                        rscit = SurveyConfigItemInfoUpsert(rscit);

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
                        oLog.LogObject = rscit;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "SurveyConfigId",
                            Value = SurveyConfigToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return SurveyConfigToUpsert;
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel SurveyConfigItemInfoUpsert(ProveedoresOnLine.Company.Models.Util.GenericItemModel SurveyConfigItemToUpsert)
        {
            if (SurveyConfigItemToUpsert.ItemId > 0 &&
                SurveyConfigItemToUpsert.ItemInfo != null &&
                SurveyConfigItemToUpsert.ItemInfo.Count > 0)
            {
                SurveyConfigItemToUpsert.ItemInfo.All(rscitinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        //upsert survey config item
                        rscitinf.ItemInfoId = DAL.Controller.SurveyDataController.Instance.SurveyConfigItemInfoUpsert
                            (rscitinf.ItemInfoId > 0 ? (int?)rscitinf.ItemInfoId : null,
                            SurveyConfigItemToUpsert.ItemId,
                            rscitinf.ItemInfoType.ItemId,
                            rscitinf.Value,
                            rscitinf.LargeValue,
                            rscitinf.Enable);

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
                        oLog.LogObject = rscitinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "SurveyConfigItemId",
                            Value = SurveyConfigItemToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return SurveyConfigItemToUpsert;
        }

        #endregion
    }
}
