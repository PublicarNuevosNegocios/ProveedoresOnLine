using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.Controller
{
    public class ProjectModule
    {
        #region Project Config

        public static ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel ProjectConfigUpsert(ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel ProjectConfigToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {
                //upsert survey
                ProjectConfigToUpsert.ItemId =
                    DAL.Controller.ProjectDataController.Instance.ProjectConfigUpsert
                    (ProjectConfigToUpsert.RelatedCustomer.RelatedCompany.CompanyPublicId,
                    ProjectConfigToUpsert.ItemId > 0 ? (int?)ProjectConfigToUpsert.ItemId : null,
                    ProjectConfigToUpsert.ItemName,
                    ProjectConfigToUpsert.Enable);

                //upsert survey items
                ProjectConfigToUpsert = EvaluationItemUpsert(ProjectConfigToUpsert);

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
                oLog.LogObject = ProjectConfigToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }

            return ProjectConfigToUpsert;
        }

        public static ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel EvaluationItemUpsert(ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel ProjectConfigToUpsert)
        {
            if (ProjectConfigToUpsert.ItemId > 0 &&
                ProjectConfigToUpsert.RelatedEvaluationItem != null &&
                ProjectConfigToUpsert.RelatedEvaluationItem.Count > 0)
            {
                ProjectConfigToUpsert.RelatedEvaluationItem.All(rei =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        //upsert survey config item
                        rei.ItemId = DAL.Controller.ProjectDataController.Instance.EvaluationItemUpsert
                            (rei.ItemId > 0 ? (int?)rei.ItemId : null,
                            ProjectConfigToUpsert.ItemId,
                            rei.ItemName,
                            rei.ItemType.ItemId,
                            rei.ParentItem != null && rei.ParentItem.ItemId > 0 ? (int?)rei.ParentItem.ItemId : null,
                            rei.Enable);

                        //upsert survey config item info
                        rei = EvaluationItemInfoUpsert(rei);

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
                        oLog.LogObject = rei;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ProjectConfigId",
                            Value = ProjectConfigToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProjectConfigToUpsert;
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel EvaluationItemInfoUpsert(ProveedoresOnLine.Company.Models.Util.GenericItemModel EvaluationItemInfoToUpsert)
        {
            if (EvaluationItemInfoToUpsert.ItemId > 0 &&
                EvaluationItemInfoToUpsert.ItemInfo != null &&
                EvaluationItemInfoToUpsert.ItemInfo.Count > 0)
            {
                EvaluationItemInfoToUpsert.ItemInfo.All(eiinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        //upsert survey config item
                        eiinf.ItemInfoId = DAL.Controller.ProjectDataController.Instance.EvaluationItemInfoUpsert
                            (eiinf.ItemInfoId > 0 ? (int?)eiinf.ItemInfoId : null,
                            EvaluationItemInfoToUpsert.ItemId,
                            eiinf.ItemInfoType.ItemId,
                            eiinf.Value,
                            eiinf.LargeValue,
                            eiinf.Enable);

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
                        oLog.LogObject = eiinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "EvaluationItemId",
                            Value = EvaluationItemInfoToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return EvaluationItemInfoToUpsert;
        }

        public static List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetAllEvaluationItemByProjectConfig(string ProjectConfigId, bool ViewEnable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.ProjectDataController.Instance.GetAllEvaluationItemByProjectConfig(ProjectConfigId, ViewEnable, PageNumber, RowCount, out TotalRows);
        }

        public static List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> GetAllProjectConfigByCustomerPublicId(string CustomerPublicId, bool ViewEnable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.ProjectDataController.Instance.GetAllProjectConfigByCustomerPublicId(CustomerPublicId, ViewEnable, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Project

        public static ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectUpsert(ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                //upsert project
                ProjectToUpsert.ProjectPublicId =
                    DAL.Controller.ProjectDataController.Instance.ProjectUpsert
                    (ProjectToUpsert.ProjectPublicId,
                    ProjectToUpsert.ProjectName,
                    ProjectToUpsert.RelatedProjectConfig.ItemId,
                    ProjectToUpsert.ProjectStatus.ItemId,
                    ProjectToUpsert.Enable);

                //upsert project info
                ProjectToUpsert = ProjectInfoUpsert(ProjectToUpsert);

                //upsert project company
                ProjectToUpsert = ProjectCompanyUpsert(ProjectToUpsert);

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
                oLog.LogObject = ProjectToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }

            return ProjectToUpsert;
        }

        public static ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectInfoUpsert(ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectToUpsert)
        {
            if (!string.IsNullOrEmpty(ProjectToUpsert.ProjectPublicId) &&
                ProjectToUpsert.ProjectInfo != null &&
                ProjectToUpsert.ProjectInfo.Count > 0)
            {
                ProjectToUpsert.ProjectInfo.All(pjnf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pjnf.ItemInfoId = DAL.Controller.ProjectDataController.Instance.ProjectInfoUpsert
                            (pjnf.ItemInfoId > 0 ? (int?)pjnf.ItemInfoId : null,
                            ProjectToUpsert.ProjectPublicId,
                            pjnf.ItemInfoType.ItemId,
                            pjnf.Value,
                            pjnf.LargeValue,
                            pjnf.Enable);

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
                        oLog.LogObject = pjnf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ProjectPublicId",
                            Value = ProjectToUpsert.ProjectPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProjectToUpsert;
        }

        public static ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectCompanyUpsert(ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectToUpsert)
        {
            if (!string.IsNullOrEmpty(ProjectToUpsert.ProjectPublicId) &&
                ProjectToUpsert.RelatedProjectProvider != null &&
                ProjectToUpsert.RelatedProjectProvider.Count > 0)
            {
                ProjectToUpsert.RelatedProjectProvider.All(pjp =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pjp.ProjectCompanyId = DAL.Controller.ProjectDataController.Instance.ProjectCompanyUpsert
                            (ProjectToUpsert.ProjectPublicId,
                            pjp.RelatedProvider.RelatedCompany.CompanyPublicId,
                            pjp.Enable);

                        //upsert info
                        pjp = ProjectCompanyInfoUpsert(pjp);

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
                        oLog.LogObject = pjp;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ProjectPublicId",
                            Value = ProjectToUpsert.ProjectPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProjectToUpsert;
        }

        public static ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel ProjectCompanyInfoUpsert(ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel ProjectProviderToUpsert)
        {
            if (ProjectProviderToUpsert.ProjectCompanyId > 0 &&
                ProjectProviderToUpsert.ItemInfo != null &&
                ProjectProviderToUpsert.ItemInfo.Count > 0)
            {
                ProjectProviderToUpsert.ItemInfo.All(pjpvinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pjpvinf.ItemInfoId = DAL.Controller.ProjectDataController.Instance.ProjectCompanyInfoUpsert
                            (pjpvinf.ItemInfoId > 0 ? (int?)pjpvinf.ItemInfoId : null,
                            ProjectProviderToUpsert.ProjectCompanyId,
                            pjpvinf.RelatedEvaluationItem != null ? (int?)pjpvinf.RelatedEvaluationItem.ItemId : null,
                            pjpvinf.ItemInfoType.ItemId,
                            pjpvinf.Value,
                            pjpvinf.LargeValue,
                            pjpvinf.Enable);

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
                        oLog.LogObject = pjpvinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ProjectCompanyId",
                            Value = ProjectProviderToUpsert.ProjectCompanyId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProjectProviderToUpsert;
        }

        public static List<ProveedoresOnLine.ProjectModule.Models.ProjectConfigModel> MPProjectConfigGetByCustomer(string CustomerPublicId)
        {
            return DAL.Controller.ProjectDataController.Instance.MPProjectConfigGetByCustomer(CustomerPublicId);
        }

        public static List<ProveedoresOnLine.ProjectModule.Models.ProjectModel> ProjectSearch(string CustomerPublicId, string SearchParam, int ProjectStatus, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.ProjectDataController.Instance.ProjectSearch(CustomerPublicId, SearchParam, ProjectStatus, PageNumber, RowCount, out  TotalRows);
        }

        public static ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetById(string ProjectPublicId, string CustomerPublicId)
        {
            return DAL.Controller.ProjectDataController.Instance.ProjectGetById(ProjectPublicId, CustomerPublicId);
        }

        public static ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetByIdLite(string ProjectPublicId, string CustomerPublicId)
        {
            return DAL.Controller.ProjectDataController.Instance.ProjectGetByIdLite(ProjectPublicId, CustomerPublicId);
        }

        public static ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetByIdCalculate(string ProjectPublicId, string CustomerPublicId)
        {
            return DAL.Controller.ProjectDataController.Instance.ProjectGetByIdCalculate(ProjectPublicId, CustomerPublicId);
        }

        public static void ProjectCalculate(string ProjectPublicId, string CustomerPublicId)
        {
            ProjectEvaluator pjeval = new ProjectEvaluator(ProjectPublicId, CustomerPublicId);
            pjeval.InitEval();
        }

        #endregion

    }
}
