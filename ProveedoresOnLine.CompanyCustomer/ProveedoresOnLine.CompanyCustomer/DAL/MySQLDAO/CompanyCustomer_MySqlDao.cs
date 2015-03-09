﻿using ProveedoresOnLine.CompanyCustomer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyCustomer.DAL.MySQLDAO
{
    internal class CompanyCustomer_MySqlDao : ICompanyCustomerData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CompanyCustomer_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CompanyCustomer.Models.Constants.C_POL_CompanyCustomerConnectionName);
        }

        #region Customer Provider

        public int CustomerProviderUpsert(string CustomerPublicId, string ProviderPublicId, int StatusId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatusId", StatusId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_CustomerProvider_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int CustomerProviderInfoUpsert(int CustomerProviderId, int? CustomerProviderInfoId, int CustomerProviderInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderId", CustomerProviderId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderInfoId", CustomerProviderInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderInfoTypeId", CustomerProviderInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_CustomerProviderInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<CompanyCustomer.Models.Customer.CustomerModel> GetCustomerByProvider(string ProviderPublicId, string CustomerSearch)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerSearch", CustomerSearch));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                    CommandText = "CC_GetCustomerByProvider",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams,
                });

            List<CompanyCustomer.Models.Customer.CustomerModel> oReturn = new List<Models.Customer.CustomerModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                if (CustomerSearch != null)
                {

                    oReturn =
                        (from cp in response.DataTableResult.AsEnumerable()
                         where !cp.IsNull("CustomerProviderId")
                         group cp by new
                         {
                             CustomerProviderId = cp.Field<int>("CustomerProviderId"),
                             CompanyPublicId = cp.Field<string>("CompanyPublicId"),
                             CompanyName = cp.Field<string>("CompanyName"),
                             Status = cp.Field<int>("Status"),
                             StatusName = cp.Field<string>("StatusName"),
                             Enable = cp.Field<UInt64>("Enable") == 1 ? true : false,
                         }
                             into cpi
                             select new CompanyCustomer.Models.Customer.CustomerModel()
                             {
                                 RelatedProvider = new List<Models.Customer.CustomerProviderModel>()
                                 {
                                     new Models.Customer.CustomerProviderModel()
                                     {
                                         CustomerProviderId = cpi.Key.CustomerProviderId,
                                         RelatedProvider = new Company.Models.Company.CompanyModel()
                                         {
                                             CompanyPublicId = cpi.Key.CompanyPublicId,
                                             CompanyName = cpi.Key.CompanyName,
                                         },
                                         Status = new Company.Models.Util.CatalogModel()
                                         {
                                             ItemId = cpi.Key.Status,
                                             ItemName = cpi.Key.StatusName,
                                         },
                                         Enable = cpi.Key.Enable,
                                     }
                                 }
                             }).ToList();
                }
                else
                {
                    oReturn =
                        (from p in response.DataTableResult.AsEnumerable()
                         where !p.IsNull("CompanyId")
                         group p by new
                         {
                             CustomerProviderId = p.Field<int>("CompanyId"),
                         }
                             into pp
                             select new CompanyCustomer.Models.Customer.CustomerModel()
                             {
                                 RelatedProvider =
                                 (from c in response.DataTableResult.AsEnumerable()
                                  where !c.IsNull("CompanyId")
                                  group c by new
                                  {
                                      CompanyId = c.Field<int>("CompanyId"),
                                      Customer = c.Field<string>("CompanyName"),
                                      CustomerPublicId = c.Field<string>("CompanyPublicId"),
                                      Enable = c.Field<int>("IsRelatedCustomer") == 1 ? true : false,
                                  }
                                      into cc
                                      select new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel()
                                      {

                                          RelatedProvider = new Company.Models.Company.CompanyModel()
                                          {
                                              CompanyName = cc.Key.Customer,
                                              Enable = cc.Key.Enable,
                                              CompanyPublicId = cc.Key.CustomerPublicId,
                                          },
                                      }).ToList(),
                             }).ToList();
                }
            }

            return oReturn;
        }

        public List<CompanyCustomer.Models.Customer.CustomerModel> GetCustomerInfoByProvider(int CustomerProviderId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderId", CustomerProviderId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                    CommandText = "CC_GetCustomerInfoByProvider",
                    CommandType = CommandType.StoredProcedure,
                    Parameters = lstParams,
                });

            List<CompanyCustomer.Models.Customer.CustomerModel> oReturn = new List<Models.Customer.CustomerModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cpi in response.DataTableResult.AsEnumerable()
                     where !cpi.IsNull("CustomerProviderInfoId")
                     group cpi by new
                     {
                         CustomerProviderInfoId = cpi.Field<int>("CustomerProviderInfoId"),
                         CustomerProviderId = cpi.Field<int>("CustomerProviderId"),
                         TrackingId = cpi.Field<int>("TrackingId"),
                         TrackingName = cpi.Field<string>("TrackingName"),
                         TrackingValue = cpi.Field<string>("TrackingValue"),
                         Enable = cpi.Field<UInt64>("Enable") == 1 ? true : false,
                         LastModify = cpi.Field<DateTime>("LastModify"),
                     }
                         into cpinf
                         select new CompanyCustomer.Models.Customer.CustomerModel()
                         {
                             RelatedProvider = new List<Models.Customer.CustomerProviderModel>()
                             {
                                 new Models.Customer.CustomerProviderModel()
                                 {
                                     CustomerProviderId = cpinf.Key.CustomerProviderId,
                                     CustomerProviderInfo = new List<Company.Models.Util.GenericItemInfoModel>()
                                     {
                                         new Company.Models.Util.GenericItemInfoModel()
                                         {
                                             ItemInfoId = cpinf.Key.CustomerProviderInfoId,
                                             ItemInfoType = new Company.Models.Util.CatalogModel()
                                             {
                                                 ItemId = cpinf.Key.TrackingId,
                                                 ItemName = cpinf.Key.TrackingName,
                                             },
                                             Value = cpinf.Key.TrackingValue,
                                             Enable = cpinf.Key.Enable,
                                             LastModify = cpinf.Key.LastModify,
                                         },
                                     },
                                 },
                             },
                         }).ToList();
            }
            return oReturn;
        }
        #endregion

        #region Survey

        public int SurveyConfigUpsert(string CompanyPublicId, int? SurveyConfigId, string SurveyName, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyName", SurveyName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_SurveyConfig_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyItemUpsert(int SurveyConfigId, int? SurveyItemId, string SurveyItemName, int SurveyItemTypeId, int? ParentSurveyItemId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyConfigId", SurveyConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemName", SurveyItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemTypeId", SurveyItemTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentSurveyItemId", ParentSurveyItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_SurveyItem_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int SurveyItemInfoUpsert(int SurveyItemId, int? SurveyItemInfoId, int SurveyItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemId", SurveyItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemInfoId", SurveyItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyItemInfoTypeId", SurveyItemInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_SurveyItemInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Project

        public int ProjectConfigUpsert(string CompanyPublicId, int? ProjectConfigId, string ProjectConfigName, int StatusId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigName", ProjectConfigName));
            lstParams.Add(DataInstance.CreateTypedParameter("vStatusId", StatusId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_ProjectConfig_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int EvaluationItemUpsert(int ProjectConfigId, int? EvaluationItemId, string EvaluationItemName, int EvaluationItemTypeId, int? ParentEvaluationItemId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectConfigId", ProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemName", EvaluationItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemTypeId", EvaluationItemTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentEvaluationItemId", ParentEvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_EvaluationItem_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int EvaluationItemInfoUpsert(int EvaluationItemId, int? EvaluationItemInfoId, int EvaluationItemInfoTypeId, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemId", EvaluationItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemInfoId", EvaluationItemInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEvaluationItemInfoTypeId", EvaluationItemInfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "CC_EvaluationItemInfo_Upsert",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion

        #region Util

        public List<Company.Models.Util.CatalogModel> CatalogGetCustomerOptions()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "U_Catalog_GetCustomerOptions",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = null
            });

            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<ProveedoresOnLine.Company.Models.Util.CatalogModel>();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("ItemId")
                     select new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                     {
                         CatalogId = c.Field<int>("CatalogId"),
                         CatalogName = c.Field<string>("CatalogName"),
                         ItemId = c.Field<int>("ItemId"),
                         ItemName = c.Field<string>("ItemName"),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion
    }
}
