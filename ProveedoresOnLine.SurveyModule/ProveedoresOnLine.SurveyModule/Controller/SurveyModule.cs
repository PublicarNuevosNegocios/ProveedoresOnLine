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


        public static List<ProveedoresOnLine.SurveyModule.Models.SurveyConfigModel> SurveyConfigSearch(string CustomerPublicId, string SearchParam, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveyConfigSearch(CustomerPublicId, SearchParam, Enable, PageNumber, RowCount, out  TotalRows);
        }

        public static Models.SurveyConfigModel SurveyConfigGetById(int SurveyConfigId)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveyConfigGetById(SurveyConfigId);
        }

        public static List<Company.Models.Util.GenericItemModel> SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? ParentSurveyConfigItem, bool Enable)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveyConfigItemGetBySurveyConfigId(SurveyConfigId, ParentSurveyConfigItem, Enable);
        }

        public static List<Models.SurveyConfigModel> MP_SurveyConfigSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.SurveyDataController.Instance.MP_SurveyConfigSearch(CustomerPublicId, SearchParam, PageNumber, RowCount);
        }

        #endregion

        #region Survey

        public static ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                //upsert survey
                SurveyToUpsert.SurveyPublicId =
                    DAL.Controller.SurveyDataController.Instance.SurveyUpsert
                    (SurveyToUpsert.SurveyPublicId,
                    SurveyToUpsert.RelatedProvider.RelatedCompany.CompanyPublicId,
                    SurveyToUpsert.RelatedSurveyConfig.ItemId,
                    SurveyToUpsert.Enable);

                //upsert survey info
                SurveyToUpsert = SurveyInfoUpsert(SurveyToUpsert);

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
                oLog.LogObject = SurveyToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }

            return SurveyToUpsert;
        }

        public static ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyInfoUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyToUpsert)
        {
            if (!string.IsNullOrEmpty(SurveyToUpsert.SurveyPublicId) &&
                SurveyToUpsert.SurveyInfo != null &&
                SurveyToUpsert.SurveyInfo.Count > 0)
            {
                SurveyToUpsert.SurveyInfo.All(sinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        sinf.ItemInfoId = DAL.Controller.SurveyDataController.Instance.SurveyInfoUpsert
                            (sinf.ItemInfoId > 0 ? (int?)sinf.ItemInfoId : null,
                            SurveyToUpsert.SurveyPublicId,
                            sinf.ItemInfoType.ItemId,
                            sinf.Value,
                            sinf.LargeValue,
                            sinf.Enable);

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
                        oLog.LogObject = sinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "SurveyPublicId",
                            Value = SurveyToUpsert.SurveyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return SurveyToUpsert;
        }

        public static ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyItemUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyToUpsert)
        {
            if (!string.IsNullOrEmpty(SurveyToUpsert.SurveyPublicId) &&
                SurveyToUpsert.RelatedSurveyItem != null &&
                SurveyToUpsert.RelatedSurveyItem.Count > 0)
            {
                SurveyToUpsert.RelatedSurveyItem.All(rsi =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        rsi.ItemId = DAL.Controller.SurveyDataController.Instance.SurveyItemUpsert
                            (rsi.ItemId > 0 ? (int?)rsi.ItemId : null,
                            SurveyToUpsert.SurveyPublicId,
                            rsi.RelatedSurveyConfigItem.ItemId,
                            rsi.Enable);

                        //update survey item info
                        rsi = SurveyItemInfoUpsert(rsi);

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
                        oLog.LogObject = rsi;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "SurveyPublicId",
                            Value = SurveyToUpsert.SurveyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return SurveyToUpsert;
        }

        public static ProveedoresOnLine.SurveyModule.Models.SurveyItemModel SurveyItemInfoUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyItemModel SurveyItemToUpsert)
        {
            if (SurveyItemToUpsert.ItemId > 0 &&
                SurveyItemToUpsert.ItemInfo != null &&
                SurveyItemToUpsert.ItemInfo.Count > 0)
            {
                SurveyItemToUpsert.ItemInfo.All(siinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        siinf.ItemInfoId = DAL.Controller.SurveyDataController.Instance.SurveyItemInfoUpsert
                            (siinf.ItemInfoId > 0 ? (int?)siinf.ItemInfoId : null,
                            SurveyItemToUpsert.ItemId,
                            siinf.ItemInfoType.ItemId,
                            siinf.Value,
                            siinf.LargeValue,
                            siinf.Enable);

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
                        oLog.LogObject = siinf;
                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return SurveyItemToUpsert;
        }

        public static List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> SurveySearch(string CustomerPublicId, string ProviderPublicId, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveySearch(CustomerPublicId, ProviderPublicId, SearchOrderType, OrderOrientation, PageNumber, RowCount, out  TotalRows);
        }

        #endregion
    }
}
