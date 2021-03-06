﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;

namespace ProveedoresOnLine.CalificationBatch.Controller
{
    public class CalificationProjectBatch
    {
        #region Calification Batch

        public static List<CalificationProjectBatchModel> CalificationProject_GetByCustomer(string vCustomerPublicid, string vProviderPublicId, bool Enable)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProject_GetByCustomer(vCustomerPublicid,vProviderPublicId,Enable);
        }

        public static List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> CalificationProject_GetProviderByCustomer(string CustomerPublicId, string ProviderPublicId)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProject_GetProviderByCustomer(CustomerPublicId, ProviderPublicId);
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
                            oCalProject.CalificationProjectId,
                            oCalProject.CalificationProjectPublicId,
                            (int)oCalProject.ProjectConfigModel.CalificationProjectConfigId,
                            oCalProject.RelatedProvider.CompanyPublicId,
                            (int)oCalProject.TotalScore,
                            oCalProject.Enable
                        );

                    CalificatioProjectItemUpsert(oCalProject);

                    oLog.IsSuccess = true;
                }
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;
                //throw err;
            }
            finally
            {
                //oLog.LogObject = oCalProject;
                //oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                //{
                //    LogInfoType = "CalificationProjectId",
                //    Value = oCalProject.CalificationProjectId.ToString()
                //});
                //LogManager.ClientLog.AddLog(oLog);
            }
            return oCalProject;
        }

        public static CalificationProjectBatchModel CalificatioProjectItemUpsert(CalificationProjectBatchModel oCalProject) 
        {
            if (oCalProject != null &&
                oCalProject.CalificationProjectItemBatchModel != null &&
                oCalProject.CalificationProjectItemBatchModel.Count > 0)
            {
                oCalProject.CalificationProjectItemBatchModel.All(oCalItemProject =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

                    try
                    {
                        if (oCalItemProject != null)
                        {
                            oCalItemProject.CalificationProjectItemId = DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProjectItemUpsert
                            (
                                oCalItemProject.CalificationProjectItemId,
                                oCalProject.CalificationProjectId,
                                (int)oCalItemProject.CalificationProjectConfigItem.CalificationProjectConfigItemId,
                                (int)oCalItemProject.ItemScore,
                                oCalItemProject.Enable
                            );

                            CalificationProjectItemInfoUpsert(oCalItemProject);

                            oLog.IsSuccess = true;
                        }
                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;
                        //throw err;
                    }
                    finally
                    {
                        //oLog.LogObject = oCalItemProject;
                        //oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        //{
                        //    LogInfoType = "CalificationProjectItemId",
                        //    Value = oCalItemProject.CalificationProjectItemId.ToString()
                        //});
                        //LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }
            
            return oCalProject;
        }

        public static CalificationProjectItemBatchModel CalificationProjectItemInfoUpsert(CalificationProjectItemBatchModel oCalItemProject) 
        {
            if (oCalItemProject != null &&
                oCalItemProject.CalificatioProjectItemInfoModel != null &&
                oCalItemProject.CalificatioProjectItemInfoModel.Count > 0)
            {
                oCalItemProject.CalificatioProjectItemInfoModel.All(oCalInfoItemProject =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

                    try
                    {
                        if (oCalInfoItemProject != null)
                        {
                            oCalInfoItemProject.CalificationProjectItemInfoId = DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProjectItemInfoUpsert
                            (
                                oCalInfoItemProject.CalificationProjectItemInfoId,
                                oCalItemProject.CalificationProjectItemId,
                                (int)oCalInfoItemProject.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId,
                                (int)oCalInfoItemProject.ItemInfoScore,
                                oCalInfoItemProject.Enable
                            );
                            oLog.IsSuccess = true;
                        }
                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;
                        //throw err;
                    }
                    finally
                    {
                        //oLog.LogObject = oCalInfoItemProject;
                        //oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        //{
                        //    LogInfoType = "CalificationProjectItemInfoId",
                        //    Value = oCalInfoItemProject.CalificationProjectItemInfoId.ToString()
                        //});
                        //LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }
            
            return oCalItemProject;
        }

        #endregion

        #region MarketPlace

        public static List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> CalificationProjectConfig_GetByCustomerPublicId(string CustomerPublicId, bool Enable)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.CalificationProjectConfig_GetByCustomerPublicId(CustomerPublicId, Enable);
        }

        #endregion

        #region Calification Project Barch Util

        #region Legal Module

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> LegalModuleInfo(string CompanyPublicId, int LegalInfoType)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.LegalModuleInfo(CompanyPublicId, LegalInfoType);
        }

        #endregion

        #region Financial Module

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> FinancialModuleInfo(string CompanyPublicId, int FinancialInfoType)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.FinancialModuleInfo(CompanyPublicId, FinancialInfoType);
        }

        #endregion

        #region Commercial Module
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CommercialModuleInfo(string CompanyPublicId, int CommercialInfoType) 
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.CommercialModuleInfo(CompanyPublicId,CommercialInfoType);
        }

        #endregion

        #region HSEQ Module
        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CertificationModuleInfo(string CompanyPublicId, int CertificationInfoType) 
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.CertificationModuleInfo(CompanyPublicId, CertificationInfoType);
        }
        #endregion

        #region Balance Module

        public static List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> BalanceModuleInfo(string CompanyPublicId, int BalanceAccount)
        {
            return DAL.Controller.CalificationProjectBatchDataController.Instance.BalanceModuleInfo(CompanyPublicId, BalanceAccount);
        }

        #endregion

        #endregion
    }
}
