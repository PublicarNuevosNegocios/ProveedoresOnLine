using ProveedoresOnLine.CompanyCustomer.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.Controller
{
    public class CompanyCustomer
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

        #region Aditional Documents

        public static CustomerModel AditionalDocumentsUpsert(CustomerModel AditionalDocumentsToUpsert)
        {
            if (AditionalDocumentsToUpsert != null)
            {
                AditionalDocumentsToUpsert.AditionalDocuments.All(ad =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

                    try
                    {
                        ad.ItemId = ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.AditionalDocumentsUpsert(
                        AditionalDocumentsToUpsert.RelatedCompany.CompanyPublicId,
                        ad.ItemId > 0 ? (int?)ad.ItemId : null,
                        ad.ItemName,
                        ad.Enable);

                        ad = AditionalDocumentsInfoUpsert(ad);
                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = AditionalDocumentsToUpsert;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = AditionalDocumentsToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return AditionalDocumentsToUpsert;
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel AditionalDocumentsInfoUpsert(ProveedoresOnLine.Company.Models.Util.GenericItemModel AditionalDocuementsInfoToUpsert)
        {
            if (AditionalDocuementsInfoToUpsert != null &&
                AditionalDocuementsInfoToUpsert.ItemId > 0 &&
                AditionalDocuementsInfoToUpsert.ItemInfo != null)
            {
                AditionalDocuementsInfoToUpsert.ItemInfo.All(adinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

                    try
                    {
                        adinf.ItemInfoId = ProveedoresOnLine.CompanyCustomer.DAL.Controller.CompanyCustomerDataController.Instance.AditionalDocumentsInfoUpsert(
                            AditionalDocuementsInfoToUpsert.ItemId,
                            adinf.ItemInfoId > 0 ? (int?)adinf.ItemInfoId : null,
                            adinf.ItemInfoType.ItemId,
                            adinf.Value,
                            adinf.LargeValue,
                            adinf.Enable);

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
                        oLog.LogObject = adinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "AditionalDocumentsInfo",
                            Value = adinf.ItemInfoId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return AditionalDocuementsInfoToUpsert;
        }

        public static ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerModel GetAditionalDocumentsByCompany(string CustomerPublicId, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyCustomerDataController.Instance.GetAditionalDocumentsByCompany(CustomerPublicId, Enable, PageNumber, RowCount, out TotalRows);
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

        public static CustomerModel GetCustomerByProvider(string ProviderPublicId, string vCustomerRelated)
        {
            return DAL.Controller.CompanyCustomerDataController.Instance.GetCustomerByProvider(ProviderPublicId, vCustomerRelated);
        }

        public static CustomerModel GetCustomerInfoByProvider(int CustomerProviderId, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyCustomerDataController.Instance.GetCustomerInfoByProvider(CustomerProviderId, Enable, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Integration

        public static List<ProveedoresOnLine.Company.Models.Company.CompanyModel> GetCustomerProviderByCustomData(string ProviderPublicId)
        {
            return DAL.Controller.CompanyCustomerDataController.Instance.GetCustomerProviderByCustomData(ProviderPublicId);
        }

        #endregion

        #region Util

        public static List<Company.Models.Util.CatalogModel> CatalogGetCustomerOptions()
        {
            return DAL.Controller.CompanyCustomerDataController.Instance.CatalogGetCustomerOptions();
        }

        #endregion
    }
}
