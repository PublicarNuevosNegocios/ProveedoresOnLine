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

        public static CustomerModel ProviderUpsert(CustomerModel CustomerToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {

                CustomerToUpsert.RelatedCompany = Company.Controller.Company.CompanyUpsert
                    (CustomerToUpsert.RelatedCompany);

                CustomerProviderUpsert(CustomerToUpsert);

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

        public static CustomerModel CustomerProviderUpsert(CustomerModel CustomerToUpsert)
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
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.CustomerProviderUpsert
                            (CustomerToUpsert.RelatedCompany.CompanyPublicId,
                            prov.RelatedProvider.CompanyPublicId,
                            prov.Status.ItemId,
                            prov.Enable);

                        CustomerProviderInfoUpsert(prov);

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

        public static CustomerProviderModel CustomerProviderInfoUpsert
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
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.CustomerProviderInfoUpsert
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

        public static List<CompanyCustomer.Models.Customer.CustomerModel> GetCustomerByProvider(string ProviderPublicId, string CustomerSearch)
        {
            return DAL.Controller.CompanyCustomerDataController.Instance.GetCustomerByProvider(ProviderPublicId, CustomerSearch);
        }

        public static List<CompanyCustomer.Models.Customer.CustomerModel> GetCustomerInfoByProvider(int CustomerProviderId)
        {
            return DAL.Controller.CompanyCustomerDataController.Instance.GetCustomerInfoByProvider(CustomerProviderId);
        }

        #endregion

        #region Customer Survey

        public CustomerModel SurveyConfigUpsert(CustomerModel CustomerToUpsert)
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
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.SurveyConfigUpsert
                            (CustomerToUpsert.RelatedCompany.CompanyPublicId,
                            srv.ItemId > 0 ? (int?)srv.ItemId : null,
                            srv.ItemName,
                            srv.Enable);

                        SurveyItemUpsert(srv);

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

        public SurveyConfigModel SurveyItemUpsert(SurveyConfigModel SurveyConfigToUpsert)
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
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.SurveyItemUpsert
                            (SurveyConfigToUpsert.ItemId,
                            srvitem.ItemId > 0 ? (int?)srvitem.ItemId : null,
                            srvitem.ItemName,
                            srvitem.ItemType.ItemId,
                            srvitem.ParentItem != null && srvitem.ParentItem.ItemId > 0 ? (int?)srvitem.ParentItem.ItemId : null,
                            srvitem.Enable);

                        SurveyItemInfoUpsert(srvitem);

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

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel SurveyItemInfoUpsert
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
                        srvitinf.ItemInfoId = DAL.Controller.CompanyCustomerDataController.Instance.SurveyItemInfoUpsert
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

        public CustomerModel ProjectConfigUpsert(CustomerModel CustomerToUpsert)
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
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.ProjectConfigUpsert
                            (CustomerToUpsert.RelatedCompany.CompanyPublicId,
                            prcnf.ItemId > 0 ? (int?)prcnf.ItemId : null,
                            prcnf.ItemName,
                            prcnf.Status.ItemId,
                            prcnf.Enable);

                        EvaluationItemUpsert(prcnf);

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

        public ProjectConfigModel EvaluationItemUpsert
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
                            ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.EvaluationItemUpsert
                            (ProjectConfigToUpsert.ItemId,
                            prjevit.ItemId > 0 ? (int?)prjevit.ItemId : null,
                            prjevit.ItemName,
                            prjevit.ItemType.ItemId,
                            prjevit.ParentItem != null && prjevit.ParentItem.ItemId > 0 ? (int?)prjevit.ParentItem.ItemId : null,
                            prjevit.Enable);

                        EvaluationItemInfoUpsert(prjevit);

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

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel EvaluationItemInfoUpsert
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
                        evitinf.ItemInfoId = DAL.Controller.CompanyCustomerDataController.Instance.EvaluationItemInfoUpsert
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
