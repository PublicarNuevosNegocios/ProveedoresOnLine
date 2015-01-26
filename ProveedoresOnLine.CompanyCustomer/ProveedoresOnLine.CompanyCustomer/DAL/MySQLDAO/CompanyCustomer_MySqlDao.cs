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
                        (from p in response.DataTableResult.AsEnumerable()
                         where !p.IsNull("CustomerProviderId")
                         group p by new
                         {
                             CustomerProviderId = p.Field<int>("CustomerProviderId"),
                         }
                             into pp
                             select new CompanyCustomer.Models.Customer.CustomerModel()
                             {
                                 RelatedProvider =
                                 (from c in response.DataTableResult.AsEnumerable()
                                  where !c.IsNull("CustomerProviderId")
                                  group c by new
                                  {
                                      CustomerProviderId = c.Field<int>("CustomerProviderId"),
                                      CustomerPublicId = c.Field<string>("CompanyPublicId"),
                                      Customer = c.Field<string>("Customer"),
                                      Status = c.Field<int>("Status"),
                                      StatusName = c.Field<string>("StatusName"),
                                      Enable = c.Field<UInt64>("Enable") == 1 ? true : false,
                                  }
                                      into cc
                                      select new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel()
                                      {
                                          CustomerProviderId = cc.Key.CustomerProviderId,
                                          RelatedProvider = new Company.Models.Company.CompanyModel()
                                          {
                                              CompanyPublicId = cc.Key.CustomerPublicId,
                                              CompanyName = cc.Key.Customer,
                                          },
                                          Status = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = cc.Key.Status,
                                              ItemName = cc.Key.StatusName,
                                          },
                                          Enable = cc.Key.Enable,
                                      }).ToList(),
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
                    CommandText = "GetCustomerInfoByProvider",
                    CommandType = CommandType.StoredProcedure,
                    Parameters = lstParams,
                });

            List<CompanyCustomer.Models.Customer.CustomerModel> oReturn = new List<Models.Customer.CustomerModel>();

            if (response.DataTableResult != null && 
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from ci in response.DataTableResult.AsEnumerable()
                     where !ci.IsNull("CustomerProviderInfoId")
                     group ci by new
                     {
                         CustomerProviderInfoId = ci.Field<int>("CustomerProviderInfoId"),
                     }
                         into cii
                         select new CompanyCustomer.Models.Customer.CustomerModel()
                         {
                             RelatedProvider =
                             (from rp in response.DataTableResult.AsEnumerable()
                              where !rp.IsNull("CustomerProviderId")
                              group rp by new
                              {
                                  CustomerProviderId = rp.Field<int>("CustomerProviderId"),
                              }
                                  into rpi
                                  select new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel()
                                  {
                                      CustomerProviderId = rpi.Key.CustomerProviderId,
                                      CustomerProviderInfo =
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
                                       into cpii
                                           select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                           {
                                               ItemInfoId = cpii.Key.CustomerProviderInfoId,
                                               ItemInfoType = new Company.Models.Util.CatalogModel()
                                               {
                                                   ItemId = cpii.Key.TrackingId,
                                                   ItemName = cpii.Key.TrackingName
                                               },
                                               Value = cpii.Key.TrackingValue,
                                               Enable = cpii.Key.Enable,
                                               LastModify = cpii.Key.LastModify,
                                           }).ToList(),
                                  }).ToList(),
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
    }
}
