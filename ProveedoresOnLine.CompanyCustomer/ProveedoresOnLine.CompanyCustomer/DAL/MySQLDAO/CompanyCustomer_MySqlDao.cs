using ProveedoresOnLine.CompanyCustomer.Interfaces;
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

        #region Aditional Documents

        public int AditionalDocumentsUpsert(string CompanyPublicId, int? AditionalDocumentsId, string Title, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentsId", AditionalDocumentsId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentsName", Title));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_AditionalDocuments_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int AditionalDocumentsInfoUpsert(int AditionalDocumentsId, int? AditionalDocumentsInfoId, int AditionalDocumentsType, string Value, string LargeValue, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentsId", AditionalDocumentsId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentsInfoId", AditionalDocumentsInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentsType", AditionalDocumentsType));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentsValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vAditionalDocumentsLargeValue", LargeValue));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_AditionalDocumentsInfo_Upsert",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public Models.Customer.CustomerModel GetAditionalDocumentsByCompany(string CustomerPublicId, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CC_GetAditionalDocumentsByCompanyPublicId",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            Models.Customer.CustomerModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("oTotalRows");

                oReturn = new Models.Customer.CustomerModel()
                {
                    RelatedCompany = new Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = response.DataTableResult.Rows[0].Field<string>("CustomerPublicId"),
                    },
                    AditionalDocuments =
                        (from ad in response.DataTableResult.AsEnumerable()
                         where !ad.IsNull("AditionalDocumentId")
                         group ad by new
                         {
                             AditionalDocumentId = ad.Field<int>("AditionalDocumentId"),
                             AditionalDocumentName = ad.Field<string>("AditionalDocumentName"),
                             AditionalDocumnetEnable = ad.Field<UInt64>("AditionalDocumentEnable"),

                             AditionalDocumentInfoId = ad.Field<int>("AditionalDocumentInfoId"),
                             AditionalDocumentInfoTypeId = ad.Field<int>("AditionalDocumentInfoTypeId"),
                             AditionalDocumentInfoTypeName = ad.Field<string>("AditionalDocumentInfoTypeName"),
                             AditionalDocumentInfoValue = ad.Field<string>("AditionalDocumentInfoValue"),
                             AditionalDocumentInfoLargeValue = ad.Field<string>("AditionalDocumentInfoLargeValue"),
                             AditionalDocumentInfoEnable = ad.Field<UInt64>("AditionalDocumentInfoEnable"),
                         }
                         into adi
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                         {
                             ItemId = adi.Key.AditionalDocumentId,
                             ItemName = adi.Key.AditionalDocumentName,
                             Enable = adi.Key.AditionalDocumnetEnable == 1 ? true : false,
                             ItemInfo =
                              (from adinf in response.DataTableResult.AsEnumerable()
                               where !adinf.IsNull("AditionalDocumentInfoId") &&
                                     adinf.Field<int>("AditionalDocumentId") == adi.Key.AditionalDocumentId
                               select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                               {
                                   ItemInfoId = adi.Key.AditionalDocumentInfoId,
                                   ItemInfoType = new Company.Models.Util.CatalogModel()
                                   {
                                       ItemId = adi.Key.AditionalDocumentInfoTypeId,
                                       ItemName = adi.Key.AditionalDocumentInfoTypeName,
                                   },
                                   Value = adi.Key.AditionalDocumentInfoValue,
                                   LargeValue = adi.Key.AditionalDocumentInfoLargeValue,
                                   Enable = adi.Key.AditionalDocumentInfoEnable == 1 ? true : false,
                               }).ToList(),
                         }).ToList(),
                };
            }

            return oReturn;
        }

        #endregion

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

        public CompanyCustomer.Models.Customer.CustomerModel GetCustomerInfoByProvider(int CustomerProviderId, bool Enable, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerProviderId", CustomerProviderId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

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
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

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
                                     CreateDate = cpi.Field<DateTime>("CreateDate"),
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
                                     CreateDate = cpinf.Key.CreateDate,
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
