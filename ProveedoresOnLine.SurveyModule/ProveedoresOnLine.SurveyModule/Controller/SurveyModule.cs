﻿using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.SurveyModule.Models;
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

        public static List<Company.Models.Util.GenericItemModel> SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? ParentSurveyConfigItem, int? SurveyItemType, bool Enable)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveyConfigItemGetBySurveyConfigId(SurveyConfigId, ParentSurveyConfigItem, SurveyItemType, Enable);
        }

        public static List<Models.SurveyConfigModel> MP_SurveyConfigSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.SurveyDataController.Instance.MP_SurveyConfigSearch(CustomerPublicId, SearchParam, PageNumber, RowCount);
        }

        public static List<Company.Models.Util.GenericItemModel> MP_SurveyConfigItemGetBySurveyConfigId(int SurveyConfigId, int? SurveyItemType)
        {
            return DAL.Controller.SurveyDataController.Instance.MP_SurveyConfigItemGetBySurveyConfigId(SurveyConfigId, SurveyItemType);
        }

        #endregion

        #region Survey

        public static ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyUpsert(ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                //upsert survey
                //Upsert Parent Survey
                SurveyToUpsert.SurveyPublicId =
                    DAL.Controller.SurveyDataController.Instance.SurveyUpsert
                    (SurveyToUpsert.SurveyPublicId,
                    SurveyToUpsert.RelatedProvider.RelatedCompany.CompanyPublicId,
                    SurveyToUpsert.RelatedSurveyConfig.ItemId,
                    null,
                    SurveyToUpsert.User,
                    SurveyToUpsert.Enable);

                //upsert survey info
                SurveyToUpsert = SurveyInfoUpsert(SurveyToUpsert);

                //upsert survey item 
                SurveyToUpsert = SurveyItemUpsert(SurveyToUpsert);

                //Upsert Child
                if (SurveyToUpsert.ChildSurvey != null && SurveyToUpsert.ChildSurvey.Count > 0)
                {
                    SurveyToUpsert.ChildSurvey.All(x =>
                    {
                        //Upsert Parent Survey                    
                        x.SurveyPublicId = DAL.Controller.SurveyDataController.Instance.SurveyUpsert
                        (x.SurveyPublicId,
                        SurveyToUpsert.RelatedProvider.RelatedCompany.CompanyPublicId,
                        SurveyToUpsert.RelatedSurveyConfig.ItemId,
                        SurveyToUpsert.SurveyPublicId,
                        x.User,
                        SurveyToUpsert.Enable);

                        //upsert ChildSurvey info
                        x = SurveyInfoUpsert(x);

                        //upsert ChildSurvey item 
                        x = SurveyItemUpsert(x);
                        return true;
                    });
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
                            rsi.EvaluatorRoleId,
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
                            SurveyItemToUpsert.EvaluatorRoleId.ToString(),
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

        public static ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyGetById(string SurveyPublicId)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveyGetById(SurveyPublicId);
        }

        public static ProveedoresOnLine.SurveyModule.Models.SurveyModel SurveyGetByUser(string ParentSurveyPublicId, string User)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveyGetByUser(ParentSurveyPublicId, User);
        }

        public static List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> SurveyGetByCustomerProvider(string CustomerPublicId, string ProviderPublicId)
        {
            return DAL.Controller.SurveyDataController.Instance.SurveyGetByCustomerProvider(CustomerPublicId, ProviderPublicId);
        }

        /// <summary>
        /// Recalculate survey values and upsert into a database
        /// </summary>
        /// <param name="SurveyPublicId">Survey to recalculate</param>
        /// <returns></returns>
        public static void SurveyRecalculate(string SurveyPublicId, int EvaluatorRolId, string UserEmail)
        {
            ProveedoresOnLine.SurveyModule.Models.SurveyModel oCurrentSurvey = DAL.Controller.SurveyDataController.Instance.SurveyGetById(SurveyPublicId);

            ProveedoresOnLine.SurveyModule.Models.SurveyModel oSurveyToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                SurveyPublicId = oCurrentSurvey.SurveyPublicId,
                SurveyInfo = new List<Company.Models.Util.GenericItemInfoModel>(),
                RelatedSurveyItem = new List<ProveedoresOnLine.SurveyModule.Models.SurveyItemModel>(),
            };

            List<GenericItemInfoModel> oAssignedAreas = oCurrentSurvey.SurveyInfo.Where(inf => inf.ItemInfoType.ItemId == 1204014).Select(inf => inf).ToList();

            //Get Only Rol Area's
            List<GenericItemModel> Areas = new List<GenericItemModel>();
            List<GenericItemModel> Answers = new List<GenericItemModel>();

            if (oAssignedAreas.Count > 0)
            {
                oAssignedAreas.All(ev =>
                {
                    //Get Areas
                    Areas.AddRange(oCurrentSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.Where(x => x.ItemId == Convert.ToInt32(ev.Value.Split('_')[0])).Select(x => x).ToList());
                    //Get Areas Iteminfo
                    Areas.AddRange(oCurrentSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.Where(x => x.ParentItem != null && x.ParentItem.ItemId == Convert.ToInt32(ev.Value.Split('_')[0])).Select(x => x).ToList());
                    return true;
                });
                if (Areas.Count > 0)
                {
                    Areas.All(qs =>
                    {
                        Answers.AddRange(oCurrentSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.Where(x => x.ItemType.ItemId == 1202003 && x.ParentItem.ItemId == qs.ItemId).Select(x => x).ToList());
                        return true;
                    });
                    if (Answers.Count > 0)
                    {
                        Areas.AddRange(Answers);
                    }
                }
                oCurrentSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem = Areas;
            }

            #region Survey Child progress
            //get mandatory survey answers
            decimal? oProgress = oCurrentSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.
                Where(scit => scit.ItemType.ItemId == 1202002 && //filter only question configuration values
                            scit.ItemInfo.Any(scitinf => scitinf.ItemInfoType.ItemId == 1203004 && //get only mandatory item info
                                            scitinf.Value == "true" //get only mandatory fields to progress                                            
                    )).
                //validate if has answer (1) or not (0)
                Select(scit => Convert.ToDecimal(oCurrentSurvey.RelatedSurveyItem.Any(svit => svit.RelatedSurveyConfigItem.ItemId == scit.ItemId))).Average();
            //get percent value            
            oProgress = oProgress == null ? 0 : oProgress * 100;

            #endregion

            #region Survey Child Ratting

            var EvaluationAreaResults = oCurrentSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.
                //filter only question configuration values
                Where(scit => scit.ItemType.ItemId == 1202002).
                Select(scit => new
                    {
                        EvaluationAreaId = scit.ParentItem.ItemId,

                        EvaluationAreaWeight = oCurrentSurvey.RelatedSurveyConfig.RelatedSurveyConfigItem.
                            Where(sciteaw => sciteaw.ItemId == scit.ParentItem.ItemId).
                            Select(sciteaw => sciteaw.ItemInfo.
                                    Where(sciteawinf => sciteawinf.ItemInfoType.ItemId == 1203002).
                                    Select(sciteawinf => string.IsNullOrEmpty(sciteawinf.Value) ? 0 : Convert.ToDecimal(sciteawinf.Value.Replace(" ", ""))).
                                    DefaultIfEmpty(0).
                                    FirstOrDefault()).
                            DefaultIfEmpty(0).
                            FirstOrDefault(),

                        QuestionWeight = scit.ItemInfo.
                            Where(scitinf => scitinf.ItemInfoType.ItemId == 1203002).
                            Select(scitinf => string.IsNullOrEmpty(scitinf.Value) ? 0 : Convert.ToDecimal(scitinf.Value.Replace(" ", ""))).
                            DefaultIfEmpty(0).
                            FirstOrDefault(),

                        QuestionRatting = oCurrentSurvey.RelatedSurveyItem.
                            Where(svit => svit.RelatedSurveyConfigItem.ItemId == scit.ItemId).
                            Select(svit => svit.ItemInfo.
                                            Where(svitinf => svitinf.ItemInfoType.ItemId == 1205001).
                                            Select(svitinf => string.IsNullOrEmpty(svitinf.Value) ? 0 : Convert.ToDecimal(svitinf.Value.Replace(" ", ""))).
                                            DefaultIfEmpty(0).
                                            FirstOrDefault()).
                        DefaultIfEmpty(0).
                        FirstOrDefault()
                    }).
                GroupBy(svg => new
                {
                    EvaluationAreaId = svg.EvaluationAreaId,
                    EvaluationAreaWeight = svg.EvaluationAreaWeight,
                }).
                Select(svgea => new
                {
                    EvaluationAreaId = svgea.Key.EvaluationAreaId,
                    EvaluationAreaWeight = svgea.Key.EvaluationAreaWeight,
                    EvaluationAreaRatting = svgea.Sum(svgeas => (svgeas.QuestionRatting * svgeas.QuestionWeight) / 100)
                }).
                ToList();

            ProveedoresOnLine.SurveyModule.Models.SurveyModel oParentSurvey = DAL.Controller.SurveyDataController.Instance.SurveyGetById(oCurrentSurvey.ParentSurveyPublicId);

            decimal? oTotalRatting = EvaluationAreaResults.Sum(ear => ((ear.EvaluationAreaRatting * ear.EvaluationAreaWeight) / 100)
                                                                * oCurrentSurvey.SurveyInfo.Where(x => x.ItemInfoType.ItemId == 1204014
                                                                && Convert.ToInt32(x.Value.Split('_')[0]) == ear.EvaluationAreaId).
                                                                Select(x => Convert.ToDecimal(x.Value.Split('_')[1])).FirstOrDefault() / 100);
            #endregion

            #region Upsert Survey Child calculate values

            //add survey progress to upsert model
            oSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = oCurrentSurvey.SurveyInfo.
                    Where(svinf => svinf.ItemInfoType.ItemId == 1204005).
                    Select(svinf => svinf.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault(),

                ItemInfoType = new Company.Models.Util.CatalogModel()
                {
                    ItemId = 1204005,
                },
                Value = oProgress.Value.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")),
                Enable = true,
            });

            //add survey ratting to upsert model
            oSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = oCurrentSurvey.SurveyInfo.
                    Where(svinf => svinf.ItemInfoType.ItemId == 1204006).
                    Select(svinf => svinf.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault(),

                ItemInfoType = new Company.Models.Util.CatalogModel()
                {
                    ItemId = 1204006,
                },
                Value = oTotalRatting.Value.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")),
                Enable = true,
            });

            //add survey item evaluation area info
            EvaluationAreaResults.All(ear =>
            {
                if (oCurrentSurvey.RelatedSurveyItem != null && oCurrentSurvey.RelatedSurveyItem.Count > 0)
                {
                    oSurveyToUpsert.RelatedSurveyItem.Add(new ProveedoresOnLine.SurveyModule.Models.SurveyItemModel()
                    {
                        ItemId = oCurrentSurvey.RelatedSurveyItem.
                            Where(svit => svit.RelatedSurveyConfigItem.ItemId == ear.EvaluationAreaId).
                            Select(svit => svit.ItemId).
                            DefaultIfEmpty(0).
                            FirstOrDefault(),
                        RelatedSurveyConfigItem = new Company.Models.Util.GenericItemModel()
                        {
                            ItemId = ear.EvaluationAreaId,
                        },
                        Enable = true,
                        EvaluatorRoleId = oCurrentSurvey.RelatedSurveyItem.FirstOrDefault().EvaluatorRoleId,
                        ItemInfo = new List<Company.Models.Util.GenericItemInfoModel>() 
                    { 
                        new Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = oCurrentSurvey.RelatedSurveyItem.
                                Where(svit => svit.RelatedSurveyConfigItem.ItemId == ear.EvaluationAreaId).
                                Select(svit => svit.ItemInfo.
                                            Where(svitinf=>svitinf.ItemInfoType.ItemId == 1205001).
                                            Select(svitinf=>svitinf.ItemInfoId).
                                            DefaultIfEmpty(0).
                                            FirstOrDefault()).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),
                            ItemInfoType = new Company.Models.Util.CatalogModel()
                            {
                                ItemId = 1205001,
                            },
                            Value = ear.EvaluationAreaRatting.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")),
                            Enable = true,
                        },                    
                    }
                    });
                }
                return true;
            });

            //upsert survey info
            oSurveyToUpsert = SurveyInfoUpsert(oSurveyToUpsert);

            //upsert survey item
            oSurveyToUpsert = SurveyItemUpsert(oSurveyToUpsert);

            #endregion

            #region Calculate Parent Survey

            ProveedoresOnLine.SurveyModule.Models.SurveyModel oParentSurveyToUpsert = new ProveedoresOnLine.SurveyModule.Models.SurveyModel()
            {
                SurveyPublicId = oParentSurvey.SurveyPublicId,
                SurveyInfo = new List<Company.Models.Util.GenericItemInfoModel>(),
                RelatedSurveyItem = new List<ProveedoresOnLine.SurveyModule.Models.SurveyItemModel>(),
            };
            List<SurveyModel> ChildsSurvey = new List<SurveyModel>();

            List<string> Evaluators = oParentSurvey.SurveyInfo.Where(x => x.ItemInfoType.ItemId == 1204003).Select(x => x.Value).ToList();
            Evaluators = Evaluators.GroupBy(x => x).Select(grp => grp.First()).ToList();

            //Get Evaluators Parent Survey|
            if (Evaluators != null)
            {
                Evaluators.All(y =>
                    {
                        ChildsSurvey.Add(SurveyGetByUser(oParentSurvey.SurveyPublicId, y));
                        return true;
                    });
            }

            #region Parent Status
            if (ChildsSurvey.Any(z => z.SurveyInfo.Where(x => x.ItemInfoType.ItemId == 1204004).Select(x => x.Value).FirstOrDefault() != "1206004"))
            {
                oParentSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                {
                    ItemInfoId = oParentSurvey.SurveyInfo.
                        Where(svinf => svinf.ItemInfoType.ItemId == 1204004).
                        Select(svinf => svinf.ItemInfoId).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),

                    ItemInfoType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = 1204004,
                    },
                    Value = "1206003",
                    Enable = true,
                });
            }
            else
            {
                oParentSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                {
                    ItemInfoId = oParentSurvey.SurveyInfo.
                        Where(svinf => svinf.ItemInfoType.ItemId == 1204004).
                        Select(svinf => svinf.ItemInfoId).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),

                    ItemInfoType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = 1204004,
                    },
                    Value = "1206004",
                    Enable = true,
                });
            }
            #endregion

            decimal? ChildCurrentProgress = 0;
            decimal? ParentProgress = 0;
            if (ChildsSurvey != null && ChildsSurvey.Count > 0)
            {
                decimal? ChilSurveyCount = ChildsSurvey.Count;
                //get ProgressBar
                if (ChildsSurvey.Any(z => z.SurveyInfo.Where(x => x.ItemInfoType.ItemId == 1204004).Select(x => x.Value).FirstOrDefault() == "1206004"))
                {
                    #region Child Progress
                    //Get Only Closed Survey
                    ChildCurrentProgress = (100 / ChilSurveyCount) * ChildsSurvey.Where(ch => ch.SurveyInfo.Where(x => x.ItemInfoType.ItemId == 1204004).
                                                Select(x => x.Value).FirstOrDefault() == "1206004").Select(ch => ch).ToList().Count;

                    #endregion

                    #region Parent Ratting
                    //Get EvaluetorRole
                    List<GenericItemModel> CurrentAreaModel = oParentSurvey.RelatedSurveyConfig.
                                                            RelatedSurveyConfigItem.Where(x => x.ItemType.ItemId == 1202004).ToList();
                    decimal? TotalRate = 0;
                    ChildsSurvey.All(a =>
                    {
                        decimal? CurrentChildRate = a.SurveyInfo.Where(x => x.ItemInfoType.ItemId == 1204006).Select(x => Convert.ToDecimal(x.Value)).DefaultIfEmpty(0).FirstOrDefault();

                        TotalRate += CurrentChildRate;
                        return true;
                    });
                    ParentProgress = ParentProgress + TotalRate;
                    #endregion
                }
                //add survey progress to upsert model
                oParentSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                {
                    ItemInfoId = oParentSurvey.SurveyInfo.
                        Where(svinf => svinf.ItemInfoType.ItemId == 1204005).
                        Select(svinf => svinf.ItemInfoId).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),

                    ItemInfoType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = 1204005,
                    },
                    Value = ChildCurrentProgress.Value.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")),
                    Enable = true,
                });

                //add survey progress to upsert model
                oParentSurveyToUpsert.SurveyInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                {
                    ItemInfoId = oParentSurvey.SurveyInfo.
                        Where(svinf => svinf.ItemInfoType.ItemId == 1204006).
                        Select(svinf => svinf.ItemInfoId).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),

                    ItemInfoType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = 1204006,
                    },
                    Value = ParentProgress.Value.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")),
                    Enable = true,
                });

                //upsert survey info
                oSurveyToUpsert = SurveyInfoUpsert(oParentSurveyToUpsert);

                //upsert survey item
                oSurveyToUpsert = SurveyItemUpsert(oParentSurveyToUpsert);
            }

            #endregion

            #region Total provider ratting

            List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> lstProviderSurvey = DAL.Controller.SurveyDataController.Instance.SurveyGetByCustomerProvider
                (oParentSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyPublicId,
                oParentSurvey.RelatedProvider.RelatedCompany.CompanyPublicId);

            decimal? oTotalProviderRatting = 0;
            int oTotalProviderRattingCount = 0;

            if (lstProviderSurvey != null && lstProviderSurvey.Count > 0)
            {
                //get ratting count
                oTotalProviderRattingCount = lstProviderSurvey.
                    Where(sv => sv.SurveyInfo.Any(svinf =>
                                    svinf.ItemInfoType.ItemId == 1204004 &&
                                    !string.IsNullOrEmpty(svinf.Value) &&
                                    svinf.Value.Replace(" ", "") == "1206004")).
                    Count();

                //get ratting
                if (oTotalProviderRattingCount > 0)
                {
                    oTotalProviderRatting = lstProviderSurvey.
                        Where(sv => sv.SurveyInfo.Any(svinf =>
                                        svinf.ItemInfoType.ItemId == 1204004 &&
                                        !string.IsNullOrEmpty(svinf.Value) &&
                                        svinf.Value.Replace(" ", "") == "1206004")).
                        Average(sv => sv.SurveyInfo.
                            Where(svinf => svinf.ItemInfoType.ItemId == 1204006).
                            Select(svinf => string.IsNullOrEmpty(svinf.Value) ? 0 : Convert.ToDecimal(svinf.Value.Replace(" ", ""))).
                            DefaultIfEmpty(0).
                            FirstOrDefault());
                }
                oTotalProviderRatting = oTotalProviderRatting == null ? 0 : oTotalProviderRatting.Value;
            }

            #endregion

            #region Upsert total provider ratting

            //get basic provider info
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> olstProvider = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.MPProviderSearchById
                (oCurrentSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyPublicId,
                oCurrentSurvey.RelatedProvider.RelatedCompany.CompanyPublicId);

            if (olstProvider != null &&
                olstProvider.Count > 0 &&
                olstProvider.Any(cp => cp.RelatedCustomerInfo.ContainsKey(oCurrentSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyPublicId)))
            {
                ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel CustomerProviderToUpsert = new CompanyCustomer.Models.Customer.CustomerProviderModel()
                {
                    CustomerProviderId = olstProvider.
                        Select(cp => cp.RelatedCustomerInfo[oCurrentSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyPublicId].ItemId).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),

                    CustomerProviderInfo = new List<Company.Models.Util.GenericItemInfoModel>() 
                    { 
                        new Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = olstProvider.
                                Select(cp=> cp.RelatedCustomerInfo[oCurrentSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyPublicId].ItemInfo.
                                            Where(cpinf=>cpinf.ItemInfoType.ItemId == 901003).
                                            Select(cpinf=>cpinf.ItemInfoId).
                                            DefaultIfEmpty(0).
                                            FirstOrDefault()).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),

                            ItemInfoType = new Company.Models.Util.CatalogModel()
                            {
                                ItemId = 901003,
                            },
                            Value = oTotalProviderRatting.Value.ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")),
                            Enable = true,
                        },
                        new Company.Models.Util.GenericItemInfoModel()
                        {
                            ItemInfoId = olstProvider.
                                Select(cp=> cp.RelatedCustomerInfo[oCurrentSurvey.RelatedSurveyConfig.RelatedCustomer.RelatedCompany.CompanyPublicId].ItemInfo.
                                            Where(cpinf=>cpinf.ItemInfoType.ItemId == 901004).
                                            Select(cpinf=>cpinf.ItemInfoId).
                                            DefaultIfEmpty(0).
                                            FirstOrDefault()).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),

                            ItemInfoType = new Company.Models.Util.CatalogModel()
                            {
                                ItemId = 901004,
                            },
                            Value = oTotalProviderRattingCount.ToString(),
                            Enable = true,
                        },
                    },
                };

                CustomerProviderToUpsert = ProveedoresOnLine.CompanyCustomer.Controller.CompanyCustomer.CustomerProviderInfoUpsert(CustomerProviderToUpsert);
            }

            #endregion
        }

        #endregion

        #region SurveyBatch

        public static List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> BPSurveyGetNotification()
        {
            return DAL.Controller.SurveyDataController.Instance.BPSurveyGetNotification();
        }

        public static List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> BPSurveyGetReminders()
        {
            return DAL.Controller.SurveyDataController.Instance.BPSurveyGetReminders();
        }

        #endregion

        #region SurveyCharts

        public static List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByResponsable(string CustomerPublicId, string ResponsableEmail, DateTime Year)
        {
            return DAL.Controller.SurveyDataController.Instance.GetSurveyByResponsable(CustomerPublicId, ResponsableEmail, Year);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByName(string CustomerPublicId, string ResponsableEmail)
        {
            return DAL.Controller.SurveyDataController.Instance.GetSurveyByName(CustomerPublicId, ResponsableEmail);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByMonth(string CustomerPublicId, string ResponsableEmail)
        {
            return DAL.Controller.SurveyDataController.Instance.GetSurveyByMonth(CustomerPublicId, ResponsableEmail);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericChartsModelInfo> GetSurveyByEvaluator(string CustomerPublicId, string ResponsableEmail)
        {
            return DAL.Controller.SurveyDataController.Instance.GetSurveyByEvaluator(CustomerPublicId, ResponsableEmail);
        }
        #endregion

        #region Index

        public static void SurveyIndex()
        {
            DAL.Controller.SurveyDataController.Instance.SurveyIndex();
        }

        #endregion
    }
}
