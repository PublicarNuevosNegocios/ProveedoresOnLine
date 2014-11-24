using ProveedoresOnLine.CompanyCustomer.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Controller
{
    public class Customer
    {
        #region Customer Info

        public static CustomerModel UpsertProvider(CustomerModel CustomerToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {

                CustomerToUpsert.RelatedCompany = Company.Controller.Company.UpsertCompany
                    (CustomerToUpsert.RelatedCompany);

                UpsertCustomerProvider(CustomerToUpsert);

                oLog.IsSuccess = true;

                return CustomerToUpsert;
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
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        #endregion

        #region Customer Provider

        public static CustomerModel UpsertCustomerProvider(CustomerModel CustomerToUpsert)
        {
            if (CustomerToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(CustomerToUpsert.RelatedCompany.CompanyPublicId) &&
                CustomerToUpsert.RelatedProvider != null &&
                CustomerToUpsert.RelatedProvider.Count > 0)
            {
                CustomerToUpsert.RelatedProvider.All(prov =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        prov.CustomerProviderId =
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.UpsertCustomerProvider
                            (CustomerToUpsert.RelatedCompany.CompanyPublicId,
                            prov.RelatedProvider.CompanyPublicId,
                            prov.Status.ItemId,
                            prov.Enable);

                        UpsertCustomerProviderInfo(prov);

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
                        oLog.LogObject = prov;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CustomerPublicId",
                            Value = CustomerToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ProviderPublicId",
                            Value = prov.RelatedProvider.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return CustomerToUpsert;
        }

        public static CustomerProviderModel UpsertCustomerProviderInfo
            (CustomerProviderModel CustomerProviderToUpsert)
        {
            if (CustomerProviderToUpsert.CustomerProviderId > 0 &&
                CustomerProviderToUpsert.CustomerProviderInfo != null &&
                CustomerProviderToUpsert.CustomerProviderInfo.Count > 0)
            {
                CustomerProviderToUpsert.CustomerProviderInfo.All(cpinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        cpinf.ItemInfoId =
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.UpsertCustomerProviderInfo
                            (CustomerProviderToUpsert.CustomerProviderId,
                            cpinf.ItemInfoId > 0 ? (int?)cpinf.ItemInfoId : null,
                            cpinf.ItemInfoType.ItemId,
                            cpinf.Value,
                            cpinf.LargeValue,
                            cpinf.Enable);

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
                        oLog.LogObject = cpinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CustomerProviderId",
                            Value = CustomerProviderToUpsert.CustomerProviderId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return CustomerProviderToUpsert;
        }

        #endregion

        #region Customer Survey

        public CustomerModel UpsertSurveyConfig(CustomerModel CustomerToUpsert)
        {
            if (CustomerToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(CustomerToUpsert.RelatedCompany.CompanyPublicId) &&
                CustomerToUpsert.RelatedSurveyConfig != null &&
                CustomerToUpsert.RelatedSurveyConfig.Count > 0)
            {
                CustomerToUpsert.RelatedSurveyConfig.All(srv =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        srv.ItemId =
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.UpsertSurveyConfig
                            (CustomerToUpsert.RelatedCompany.CompanyPublicId,
                            srv.ItemId > 0 ? (int?)srv.ItemId : null,
                            srv.ItemName,
                            srv.Enable);

                        UpsertSurveyItem(srv);

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
                        oLog.LogObject = srv;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CustomerPublicId",
                            Value = CustomerToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return CustomerToUpsert;
        }

        public SurveyConfigModel UpsertSurveyItem(SurveyConfigModel SurveyConfigToUpsert)
        {
            if (SurveyConfigToUpsert.ItemId > 0 &&
                SurveyConfigToUpsert.RelatedSurveyItem != null &&
                SurveyConfigToUpsert.RelatedSurveyItem.Count > 0)
            {
                SurveyConfigToUpsert.RelatedSurveyItem.All(srvitem =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        srvitem.ItemId =
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.UpsertSurveyItem
                            (SurveyConfigToUpsert.ItemId,
                            srvitem.ItemId > 0 ? (int?)srvitem.ItemId : null,
                            srvitem.ItemName,
                            srvitem.ItemType.ItemId,
                            srvitem.ParentItem != null && srvitem.ParentItem.ItemId > 0 ? (int?)srvitem.ParentItem.ItemId : null,
                            srvitem.Enable);

                        UpsertSurveyItemInfo(srvitem);

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
                        oLog.LogObject = srvitem;

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

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel UpsertSurveyItemInfo
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel SurveyItemToUpsert)
        {
            if (SurveyItemToUpsert.ItemId > 0 &&
                SurveyItemToUpsert.ItemInfo != null &&
                SurveyItemToUpsert.ItemInfo.Count > 0)
            {
                SurveyItemToUpsert.ItemInfo.All(srvitinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        srvitinf.ItemInfoId = DAL.Controller.CompanyCustomerDataController.Instance.UpsertSurveyItemInfo
                            (SurveyItemToUpsert.ItemId,
                            srvitinf.ItemInfoId > 0 ? (int?)srvitinf.ItemInfoId : null,
                            srvitinf.ItemInfoType.ItemId,
                            srvitinf.Value,
                            srvitinf.LargeValue,
                            srvitinf.Enable);

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
                        oLog.LogObject = srvitinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "SurveyItemId",
                            Value = SurveyItemToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return SurveyItemToUpsert;
        }

        #endregion

        #region Customer Project

        public CustomerModel UpsertProjectConfig(CustomerModel CustomerToUpsert)
        {
            if (CustomerToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(CustomerToUpsert.RelatedCompany.CompanyPublicId) &&
                CustomerToUpsert.RelatedProjectConfig != null &&
                CustomerToUpsert.RelatedProjectConfig.Count > 0)
            {
                CustomerToUpsert.RelatedProjectConfig.All(prcnf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        prcnf.ItemId =
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.UpsertProjectConfig
                            (CustomerToUpsert.RelatedCompany.CompanyPublicId,
                            prcnf.ItemId > 0 ? (int?)prcnf.ItemId : null,
                            prcnf.ItemName,
                            prcnf.Status.ItemId,
                            prcnf.Enable);

                        UpsertEvaluationItem(prcnf);

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
                        oLog.LogObject = prcnf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CustomerPublicId",
                            Value = CustomerToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return CustomerToUpsert;
        }

        public ProjectConfigModel UpsertEvaluationItem
            (ProjectConfigModel ProjectConfigToUpsert)
        {
            if (ProjectConfigToUpsert.ItemId > 0 &&
                ProjectConfigToUpsert.RelatedEvaluation != null &&
                ProjectConfigToUpsert.RelatedEvaluation.Count > 0)
            {
                ProjectConfigToUpsert.RelatedEvaluation.All(prjevit =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        prjevit.ItemId =
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.UpsertEvaluationItem
                            (ProjectConfigToUpsert.ItemId,
                            prjevit.ItemId > 0 ? (int?)prjevit.ItemId : null,
                            prjevit.ItemName,
                            prjevit.ItemType.ItemId,
                            prjevit.ParentItem != null && prjevit.ParentItem.ItemId > 0 ? (int?)prjevit.ParentItem.ItemId : null,
                            prjevit.Enable);

                        UpsertEvaluationItemInfo(prjevit);

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
                        oLog.LogObject = prjevit;

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

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel UpsertEvaluationItemInfo
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel EvaluationItemToUpsert)
        {
            if (EvaluationItemToUpsert.ItemId > 0 &&
                EvaluationItemToUpsert.ItemInfo != null &&
                EvaluationItemToUpsert.ItemInfo.Count > 0)
            {
                EvaluationItemToUpsert.ItemInfo.All(evitinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        evitinf.ItemInfoId = DAL.Controller.CompanyCustomerDataController.Instance.UpsertEvaluationItemInfo
                            (EvaluationItemToUpsert.ItemId,
                            evitinf.ItemInfoId > 0 ? (int?)evitinf.ItemInfoId : null,
                            evitinf.ItemInfoType.ItemId,
                            evitinf.Value,
                            evitinf.LargeValue,
                            evitinf.Enable);

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
                        oLog.LogObject = evitinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "EvaluationItemId",
                            Value = EvaluationItemToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return EvaluationItemToUpsert;
        }

        #endregion
    }
}
