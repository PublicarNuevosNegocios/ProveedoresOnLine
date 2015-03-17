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

        public CompanyCustomer.Models.Customer.CustomerModel GetCustomerByProvider(string ProviderPublicId, string vCustomerRelated)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerRelated", vCustomerRelated));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                    CommandText = "CC_GetCustomerByProvider",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams,
                });

            CompanyCustomer.Models.Customer.CustomerModel oReturn = new Models.Customer.CustomerModel();

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new Models.Customer.CustomerModel()
                {
                    RelatedProvider =
                     (from cpri in response.DataTableResult.AsEnumerable()
                      where !cpri.IsNull("CustomerPublicId")
                      group cpri by new
                      {
                          CustomerProviderId = cpri.Field<UInt64>("CustomerProviderId"),
                          CustomerPublicId = cpri.Field<string>("CustomerPublicId"),
                          CustomerName = cpri.Field<string>("CustomerName"),
                          StatusId = cpri.Field<UInt64>("StatusId"),
                          StatusName = cpri.Field<string>("StatusName"),
                          IsRelatedCustomer = cpri.Field<UInt64>("IsRelatedCustomer") == 1 ? true : false,
                      } into cprinf
                      select new ProveedoresOnLine.CompanyCustomer.Models.Customer.CustomerProviderModel()
                      {
                          CustomerProviderId = Convert.ToInt32(cprinf.Key.CustomerProviderId),
                          RelatedProvider = new Company.Models.Company.CompanyModel()
                          {
                              CompanyPublicId = cprinf.Key.CustomerPublicId,
                              CompanyName = cprinf.Key.CustomerName,
                          },
                          Status = new Company.Models.Util.CatalogModel()
                          {
                              ItemId = Convert.ToInt32(cprinf.Key.StatusId),
                              ItemName = cprinf.Key.StatusName,
                          },
                          Enable = cprinf.Key.IsRelatedCustomer,
                      }).ToList(),
                };
            }

            return oReturn;
        }

        public CompanyCustomer.Models.Customer.CustomerModel GetCustomerInfoByProvider(int CustomerProviderId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderId", CustomerProviderId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_GetCustomerInfoByProvider",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            CompanyCustomer.Models.Customer.CustomerModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new Models.Customer.CustomerModel()
                {
                    RelatedProvider = new List<Models.Customer.CustomerProviderModel>()
                    {
                        new Models.Customer.CustomerProviderModel(){
                            CustomerProviderId = response.DataTableResult.Rows[0].Field<int>("CustomerProviderId"),
                            CustomerProviderInfo = 
                            (from cpi in response.DataTableResult.AsEnumerable()
                                 where !cpi.IsNull("CustomerProviderInfoId")
                                 group cpi by new
                                 {
                                     CustomerProviderInfoId = cpi.Field<int>("CustomerProviderInfoId"),
                                     TrackingId = cpi.Field<int>("TrackingId"),
                                     TrackingName = cpi.Field<string>("TrackingName"),
                                     TrackingValue = cpi.Field<string>("TrackingValue"),
                                     Enable = cpi.Field<UInt64>("Enable") == 1 ? true : false,
                                     LastModify = cpi.Field<DateTime>("LastModify"),
                                 } into cpinf
                                 select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                 {

                                     ItemInfoId = cpinf.Key.CustomerProviderInfoId,
                                     ItemInfoType = new Company.Models.Util.CatalogModel()
                                     {
                                         ItemId = cpinf.Key.TrackingId,
                                         ItemName = cpinf.Key.TrackingName,
                                     },
                                     LargeValue = cpinf.Key.TrackingValue,
                                     Enable = cpinf.Key.Enable,
                                     LastModify = cpinf.Key.LastModify,
                                 }).ToList()
                        }
                    }
                };
            }
            return oReturn;
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
