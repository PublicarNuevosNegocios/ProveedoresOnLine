using ProveedoresOnLine.CompareModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ProveedoresOnLine.CompareModule.DAL.MySQLDAO
{
    internal class Compare_MySqlDao : ProveedoresOnLine.CompareModule.Interfaces.ICompareData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Compare_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CompareModule.Models.Constants.C_POL_CompareConnectionName);
        }

        #region Implemented methods

        public int CompareUpsert(int? CompareId, string CompareName, string User, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompareId", CompareId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompareName", CompareName));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CM_Compare_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public int CompareCompanyUpsert(int CompareId, string CompanyPublicId, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompareId", CompareId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "MP_CM_CompareCompany_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        public List<CompareModel> CompareSearch(string SearchParam, string User, int PageNumber, int RowCount, out int TotalRows)
        {
            TotalRows = 0;

            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSearchParam", SearchParam));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vPageNumber", PageNumber));
            lstParams.Add(DataInstance.CreateTypedParameter("vRowCount", RowCount));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CM_Compare_Search",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<CompareModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                TotalRows = response.DataTableResult.Rows[0].Field<int>("TotalRows");

                oReturn =
                    (from cmp in response.DataTableResult.AsEnumerable()
                     where !cmp.IsNull("CompareId")
                     select new CompareModel()
                     {
                         CompareId = cmp.Field<int>("CompareId"),
                         CompareName = cmp.Field<string>("CompareName"),
                         User = cmp.Field<string>("User"),
                         LastModify = cmp.Field<DateTime>("LastModify"),
                         CreateDate = cmp.Field<DateTime>("CreateDate"),
                     }).ToList();
            }
            return oReturn;
        }

        public CompareModel CompareGetCompanyBasicInfo(int CompareId, string User, string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompareId", CompareId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CM_Compare_GetCompanyBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            CompareModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new CompareModel()
                {
                    CompareId = response.DataTableResult.Rows[0].Field<int>("CompareId"),
                    CompareName = response.DataTableResult.Rows[0].Field<string>("CompareName"),
                    User = response.DataTableResult.Rows[0].Field<string>("User"),

                    RelatedProvider =
                        (from p in response.DataTableResult.AsEnumerable()
                         where !p.IsNull("CompanyPublicId")
                         group p by new
                         {
                             CompareCompanyId = p.Field<int>("CompareCompanyId"),

                             CompanyPublicId = p.Field<string>("CompanyPublicId"),
                             CompanyName = p.Field<string>("CompanyName"),
                             IdentificationTypeId = p.Field<int>("IdentificationTypeId"),
                             IdentificationTypeName = p.Field<string>("IdentificationTypeName"),
                             IdentificationNumber = p.Field<string>("IdentificationNumber"),
                             CompanyTypeId = p.Field<int>("CompanyTypeId"),
                             CompanyTypeName = p.Field<string>("CompanyTypeName"),
                         } into pg
                         select new CompareCompanyModel()
                         {
                             CompareCompanyId = pg.Key.CompareCompanyId,

                             RelatedCompany = new Company.Models.Company.CompanyModel()
                             {
                                 CompanyPublicId = pg.Key.CompanyPublicId,
                                 CompanyName = pg.Key.CompanyName,
                                 IdentificationType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = pg.Key.IdentificationTypeId,
                                     ItemName = pg.Key.IdentificationTypeName,
                                 },
                                 IdentificationNumber = pg.Key.IdentificationNumber,
                                 CompanyType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = pg.Key.CompanyTypeId,
                                     ItemName = pg.Key.CompanyTypeName
                                 },

                                 CompanyInfo =
                                     (from pi in response.DataTableResult.AsEnumerable()
                                      where !pi.IsNull("CompanyInfoId") && pi.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                                      group pi by new
                                      {
                                          CompanyInfoId = pi.Field<int>("CompanyInfoId"),
                                          CompanyInfoTypeId = pi.Field<int>("CompanyInfoTypeId"),
                                          CompanyInfoTypeName = pi.Field<string>("CompanyInfoTypeName"),
                                          CompanyInfoValue = pi.Field<string>("CompanyInfoValue"),
                                          CompanyInfoLargeValue = pi.Field<string>("CompanyInfoLargeValue"),
                                      } into pig
                                      select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                      {
                                          ItemInfoId = pig.Key.CompanyInfoId,
                                          ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = pig.Key.CompanyInfoTypeId,
                                              ItemName = pig.Key.CompanyInfoTypeName,
                                          },
                                          Value = pig.Key.CompanyInfoValue,
                                          LargeValue = pig.Key.CompanyInfoLargeValue,
                                      }).ToList(),
                             },
                             RelatedCustomerInfo =
                                (from rc in response.DataTableResult.AsEnumerable()
                                 where !rc.IsNull("CustomerProviderId") && rc.Field<string>("CompanyPublicId") == pg.Key.CompanyPublicId
                                 group rc by new
                                 {
                                     CustomerPublicId = rc.Field<string>("CustomerPublicId"),
                                     CustomerProviderId = rc.Field<int>("CustomerProviderId"),
                                     CustomerProviderStatusId = rc.Field<int>("CustomerProviderStatusId"),
                                     CustomerProviderStatusName = rc.Field<string>("CustomerProviderStatusName"),
                                 } into rcg
                                 select new
                                 {
                                     oKey = rcg.Key.CustomerPublicId,
                                     oValue = new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                     {
                                         ItemId = rcg.Key.CustomerProviderId,
                                         ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                         {
                                             ItemId = rcg.Key.CustomerProviderStatusId,
                                             ItemName = rcg.Key.CustomerProviderStatusName,
                                         },
                                         ItemInfo =
                                            (from rci in response.DataTableResult.AsEnumerable()
                                             where !rci.IsNull("CustomerProviderInfoId") && rci.Field<int>("CustomerProviderId") == rcg.Key.CustomerProviderId
                                             group rci by new
                                             {
                                                 CustomerProviderInfoId = rci.Field<int>("CustomerProviderInfoId"),
                                                 CustomerProviderInfoTypeId = rci.Field<int>("CustomerProviderInfoTypeId"),
                                                 CustomerProviderInfoTypeName = rci.Field<string>("CustomerProviderInfoTypeName"),
                                                 CustomerProviderInfoValue = rci.Field<string>("CustomerProviderInfoValue"),
                                                 CustomerProviderInfoLargeValue = rci.Field<string>("CustomerProviderInfoLargeValue"),
                                             } into rcig
                                             select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                             {
                                                 ItemInfoId = rcig.Key.CustomerProviderInfoId,
                                                 ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                 {
                                                     ItemId = rcig.Key.CustomerProviderInfoTypeId,
                                                     ItemName = rcig.Key.CustomerProviderInfoTypeName,
                                                 },
                                                 Value = rcig.Key.CustomerProviderInfoValue,
                                                 LargeValue = rcig.Key.CustomerProviderInfoLargeValue,
                                             }).ToList()
                                     }
                                 }).ToDictionary(k => k.oKey, v => v.oValue),
                         }).ToList(),
                };
            }
            return oReturn;
        }

        #endregion
    }
}
