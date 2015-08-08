using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.Reports.Interfaces;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.DAL.MySQLDAO
{
    internal class Reports_MySqlDao : IReportData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Reports_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.Reports.Models.Constants.R_POL_ReportsConnectionName);
        }

        #region report

        public List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));


            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Report_Survey_GetAllByCustomer",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<SurveyModule.Models.SurveyModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sv in response.DataTableResult.AsEnumerable()
                     where !sv.IsNull("SurveyPublicId")
                     group sv by new
                     {
                         SurveyPublicId = sv.Field<string>("SurveyPublicId"),
                         LastModify = sv.Field<DateTime>("LastModify"),
                         SurveyConfigId = sv.Field<int>("SurveyConfigId"),
                         SurveyName = sv.Field<string>("SurveyName"),
                     } into svg
                     select new SurveyModel()
                     {
                         SurveyPublicId = svg.Key.SurveyPublicId,
                         LastModify = svg.Key.LastModify,

                         RelatedSurveyConfig = new SurveyConfigModel()
                         {
                             ItemId = svg.Key.SurveyConfigId,
                             ItemName = svg.Key.SurveyName,

                             ItemInfo =
                                (from scinf in response.DataTableResult.AsEnumerable()
                                 where !scinf.IsNull("SurveyConfigInfoId") &&
                                        scinf.Field<int>("SurveyConfigId") == svg.Key.SurveyConfigId &&
                                        scinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                                 group scinf by new
                                 {
                                     SurveyConfigInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                     SurveyConfigInfoTypeId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                     SurveyConfigInfoTypeName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                     SurveyConfigInfoValue = scinf.Field<string>("SurveyConfigInfoValue"),
                                     SurveyConfigInfoLargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                     SurveyConfigInfoValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                                 } into scinfg
                                 select new GenericItemInfoModel()
                                 {
                                     ItemInfoId = scinfg.Key.SurveyConfigInfoId,
                                     ItemInfoType = new CatalogModel()
                                     {
                                         ItemId = scinfg.Key.SurveyConfigInfoTypeId,
                                         ItemName = scinfg.Key.SurveyConfigInfoTypeName,
                                     },
                                     Value = scinfg.Key.SurveyConfigInfoValue,
                                     LargeValue = scinfg.Key.SurveyConfigInfoLargeValue,
                                     ValueName = scinfg.Key.SurveyConfigInfoValueName,
                                 }).ToList(),
                         },
                         SurveyInfo =
                            (from svinf in response.DataTableResult.AsEnumerable()
                             where !svinf.IsNull("SurveyInfoId") &&
                                    svinf.Field<string>("SurveyPublicId") == svg.Key.SurveyPublicId
                             group svinf by new
                             {
                                 SurveyInfoId = svinf.Field<int>("SurveyInfoId"),
                                 SurveyInfoTypeId = svinf.Field<int>("SurveyInfoTypeId"),
                                 SurveyInfoTypeName = svinf.Field<string>("SurveyInfoTypeName"),
                                 SurveyInfoValue = svinf.Field<string>("SurveyInfoValue"),
                                 SurveyInfoLargeValue = svinf.Field<string>("SurveyInfoLargeValue"),
                                 SurveyInfoValueName = svinf.Field<string>("SurveyInfoValueName"),

                             } into svinfg
                             select new GenericItemInfoModel()
                             {
                                 ItemInfoId = svinfg.Key.SurveyInfoId,
                                 ItemInfoType = new CatalogModel()
                                 {
                                     ItemId = svinfg.Key.SurveyInfoTypeId,
                                     ItemName = svinfg.Key.SurveyInfoTypeName,
                                 },
                                 Value = svinfg.Key.SurveyInfoValue,
                                 LargeValue = svinfg.Key.SurveyInfoLargeValue,
                                 ValueName = svinfg.Key.SurveyInfoValueName,
                             }).ToList(),
                     }).ToList();

            }
            return oReturn;
        }

        public SurveyModel SurveyGetById(string SurveyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vSurveyPublicId", SurveyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CP_Survey_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            SurveyModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 1 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[1].Rows.Count > 0)
            {
                oReturn = new SurveyModel()
                {
                    SurveyPublicId = response.DataSetResult.Tables[1].Rows[0].Field<string>("SurveyPublicId"),
                    LastModify = response.DataSetResult.Tables[1].Rows[0].Field<DateTime>("SurveyLastModify"),
                    ParentSurveyPublicId = response.DataSetResult.Tables[1].Rows[0].Field<string>("ParentSurveyPublicId"),
                    User = response.DataSetResult.Tables[1].Rows[0].Field<string>("User"),
                    RelatedProvider = new CompanyProvider.Models.Provider.ProviderModel()
                    {
                        RelatedCompany = new Company.Models.Company.CompanyModel()
                        {
                            CompanyPublicId = response.DataSetResult.Tables[1].Rows[0].Field<string>("ProviderPublicId"),
                        },
                    },
                    #region SurveyInfo
                    SurveyInfo =
                                    (from svinf in response.DataSetResult.Tables[1].AsEnumerable()
                                     where !svinf.IsNull("SurveyInfoId") &&
                                            svinf.Field<string>("SurveyPublicId") == response.DataSetResult.Tables[1].Rows[0].Field<string>("SurveyPublicId")
                                     group svinf by new
                                     {
                                         SurveyInfoId = svinf.Field<int>("SurveyInfoId"),
                                         SurveyInfoTypeId = svinf.Field<int>("SurveyInfoTypeId"),
                                         SurveyInfoTypeName = svinf.Field<string>("SurveyInfoTypeName"),
                                         SurveyInfoValue = svinf.Field<string>("SurveyInfoValue"),
                                         SurveyInfoLargeValue = svinf.Field<string>("SurveyInfoLargeValue"),
                                         SurveyInfoValueName = svinf.Field<string>("SurveyInfoValueName"),
                                     } into svinfg
                                     select new GenericItemInfoModel()
                                     {
                                         ItemInfoId = svinfg.Key.SurveyInfoId,
                                         ItemInfoType = new CatalogModel()
                                         {
                                             ItemId = svinfg.Key.SurveyInfoTypeId,
                                             ItemName = svinfg.Key.SurveyInfoTypeName,
                                         },
                                         Value = svinfg.Key.SurveyInfoValue,
                                         LargeValue = svinfg.Key.SurveyInfoLargeValue,
                                         ValueName = svinfg.Key.SurveyInfoValueName,
                                     }).ToList(),
                    #endregion

                    #region RelatedSurveyItem
                    RelatedSurveyItem =
                                    (from svit in response.DataSetResult.Tables[1].AsEnumerable()
                                     where !svit.IsNull("SurveyItemId") &&
                                            svit.Field<string>("SurveyPublicId") == response.DataSetResult.Tables[1].Rows[0].Field<string>("SurveyPublicId")
                                     group svit by new
                                     {
                                         SurveyItemId = svit.Field<int>("SurveyItemId"),
                                         SurveyItemSurveyConfigItemId = svit.Field<int>("SurveyItemSurveyConfigItemId"),
                                         EvaluatorRolId = svit.Field<int>("EvaluatorRolId"),
                                         SurveyItemLastModify = svit.Field<DateTime>("SurveyItemLastModify"),
                                     } into svitg
                                     select new SurveyItemModel()
                                     {
                                         ItemId = svitg.Key.SurveyItemId,
                                         RelatedSurveyConfigItem = new GenericItemModel()
                                         {
                                             ItemId = svitg.Key.SurveyItemSurveyConfigItemId,
                                         },
                                         EvaluatorRoleId = svitg.Key.EvaluatorRolId,
                                         LastModify = svitg.Key.SurveyItemLastModify,

                                         ItemInfo =
                                            (from svitinf in response.DataSetResult.Tables[1].AsEnumerable()
                                             where !svitinf.IsNull("SurveyItemInfoId") &&
                                                    svitinf.Field<int>("SurveyItemId") == svitg.Key.SurveyItemId
                                             group svitinf by new
                                             {
                                                 SurveyItemInfoId = svitinf.Field<int>("SurveyItemInfoId"),
                                                 SurveyItemInfoTypeId = svitinf.Field<int>("SurveyItemInfoTypeId"),
                                                 SurveyItemInfoTypeName = svitinf.Field<string>("SurveyItemInfoTypeName"),
                                                 SurveyItemInfoValue = svitinf.Field<string>("SurveyItemInfoValue"),
                                                 SurveyItemInfoLargeValue = svitinf.Field<string>("SurveyItemInfoLargeValue"),
                                             } into svitinfg
                                             select new GenericItemInfoModel()
                                             {
                                                 ItemInfoId = svitinfg.Key.SurveyItemInfoId,
                                                 ItemInfoType = new CatalogModel()
                                                 {
                                                     ItemId = svitinfg.Key.SurveyItemInfoTypeId,
                                                     ItemName = svitinfg.Key.SurveyItemInfoTypeName,
                                                 },
                                                 Value = svitinfg.Key.SurveyItemInfoValue,
                                                 LargeValue = svitinfg.Key.SurveyItemInfoLargeValue,
                                             }).ToList(),
                                     }).ToList(),
                    #endregion

                    RelatedSurveyConfig = new SurveyConfigModel()
                    {
                        ItemId = response.DataSetResult.Tables[0].Rows[0].Field<int>("SurveyConfigId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("SurveyName"),

                        RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                        {
                            RelatedCompany = new Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerPublicId"),
                            },
                        },

                        ItemInfo =
                           (from scinf in response.DataSetResult.Tables[0].AsEnumerable()
                            where !scinf.IsNull("SurveyConfigInfoId") &&
                                   scinf.Field<int>("SurveyConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<int>("SurveyConfigId")
                            group scinf by new
                            {
                                SurveyConfigInfoId = scinf.Field<int>("SurveyConfigInfoId"),
                                SurveyConfigInfoTypeId = scinf.Field<int>("SurveyConfigInfoTypeId"),
                                SurveyConfigInfoTypeName = scinf.Field<string>("SurveyConfigInfoTypeName"),
                                SurveyConfigInfoValue = scinf.Field<string>("SurveyConfigInfoValue"),
                                SurveyConfigInfoLargeValue = scinf.Field<string>("SurveyConfigInfoLargeValue"),
                                SurveyConfigInfoValueName = scinf.Field<string>("SurveyConfigInfoValueName"),
                            } into scinfg
                            select new GenericItemInfoModel()
                            {
                                ItemInfoId = scinfg.Key.SurveyConfigInfoId,
                                ItemInfoType = new CatalogModel()
                                {
                                    ItemId = scinfg.Key.SurveyConfigInfoTypeId,
                                    ItemName = scinfg.Key.SurveyConfigInfoTypeName,
                                },
                                Value = scinfg.Key.SurveyConfigInfoValue,
                                LargeValue = scinfg.Key.SurveyConfigInfoLargeValue,
                                ValueName = scinfg.Key.SurveyConfigInfoValueName,
                            }).ToList(),

                        RelatedSurveyConfigItem =
                            (from scit in response.DataSetResult.Tables[0].AsEnumerable()
                             where !scit.IsNull("SurveyConfigItemId") &&
                                    scit.Field<int>("SurveyConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<int>("SurveyConfigId")
                             group scit by new
                             {
                                 SurveyConfigItemId = scit.Field<int>("SurveyConfigItemId"),
                                 SurveyConfigItemName = scit.Field<string>("SurveyConfigItemName"),
                                 SurveyConfigItemTypeId = scit.Field<int>("SurveyConfigItemTypeId"),
                                 SurveyConfigItemTypeName = scit.Field<string>("SurveyConfigItemTypeName"),
                                 ParentSurveyConfigItem = scit.Field<int?>("ParentSurveyConfigItem"),
                             } into scitg
                             select new GenericItemModel()
                             {
                                 ItemId = scitg.Key.SurveyConfigItemId,
                                 ItemName = scitg.Key.SurveyConfigItemName,
                                 ItemType = new CatalogModel()
                                 {
                                     ItemId = scitg.Key.SurveyConfigItemTypeId,
                                     ItemName = scitg.Key.SurveyConfigItemTypeName,
                                 },
                                 ParentItem = scitg.Key.ParentSurveyConfigItem == null ? null :
                                    new GenericItemModel()
                                    {
                                        ItemId = scitg.Key.ParentSurveyConfigItem.Value,
                                    },

                                 ItemInfo =
                                     (from scitinf in response.DataSetResult.Tables[0].AsEnumerable()
                                      where !scitinf.IsNull("SurveyConfigItemInfoId") &&
                                             scitinf.Field<int>("SurveyConfigItemId") == scitg.Key.SurveyConfigItemId
                                      group scitinf by new
                                      {
                                          SurveyConfigItemInfoId = scitinf.Field<int>("SurveyConfigItemInfoId"),
                                          SurveyConfigItemInfoTypeId = scitinf.Field<int>("SurveyConfigItemInfoTypeId"),
                                          SurveyConfigItemInfoTypeName = scitinf.Field<string>("SurveyConfigItemInfoTypeName"),
                                          SurveyConfigItemInfoValue = scitinf.Field<string>("SurveyConfigItemInfoValue"),
                                          SurveyConfigItemInfoLargeValue = scitinf.Field<string>("SurveyConfigItemInfoLargeValue"),
                                      } into scitinfg
                                      select new GenericItemInfoModel()
                                      {
                                          ItemInfoId = scitinfg.Key.SurveyConfigItemInfoId,
                                          ItemInfoType = new CatalogModel()
                                          {
                                              ItemId = scitinfg.Key.SurveyConfigItemInfoTypeId,
                                              ItemName = scitinfg.Key.SurveyConfigItemInfoTypeName,
                                          },
                                          Value = scitinfg.Key.SurveyConfigItemInfoValue,
                                          LargeValue = scitinfg.Key.SurveyConfigItemInfoLargeValue,
                                      }).ToList(),
                             }).ToList(),
                    },
                };
            }
            return oReturn;
        }

        #endregion

        #region Gerencial Report

        public List<GenericItemModel> BlackListGetByCompanyPublicId(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_BlackListInfo_GetInfoByCompanyPublicId",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bl in response.DataTableResult.AsEnumerable()
                     where !bl.IsNull("BlackListId")
                     group bl by new
                     {
                         BlackListId = bl.Field<int>("BlackListId"),
                         BlackListInfoId = bl.Field<int>("BlackListInfoId"),
                         BlackListInfoType = bl.Field<string>("BlackListInfoType"),
                         Value = bl.Field<string>("Value")
                     } into blg
                     select new GenericItemModel()
                     {
                         ItemId = blg.Key.BlackListId,
                         ItemType = new CatalogModel()
                         {
                             ItemName = blg.Key.BlackListInfoType,
                         },
                         ItemName = blg.Key.Value,
                     }).ToList();
            }
            return oReturn;
        }

        public Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_C_Company_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            Company.Models.Company.CompanyModel oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = new Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = response.DataTableResult.Rows[0].Field<string>("CompanyPublicId"),
                    CompanyName = response.DataTableResult.Rows[0].Field<string>("CompanyName"),
                    IdentificationType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("IdentificationTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("IdentificationTypeName"),
                    },
                    IdentificationNumber = response.DataTableResult.Rows[0].Field<string>("IdentificationNumber"),
                    CompanyType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = response.DataTableResult.Rows[0].Field<int>("CompanyTypeId"),
                        ItemName = response.DataTableResult.Rows[0].Field<string>("CompanyTypeName"),
                    },
                    Enable = response.DataTableResult.Rows[0].Field<UInt64>("CompanyEnable") == 1 ? true : false,
                    LastModify = response.DataTableResult.Rows[0].Field<DateTime>("CompanyLastModify"),
                    CreateDate = response.DataTableResult.Rows[0].Field<DateTime>("CompanyCreateDate"),

                    CompanyInfo =
                        (from ci in response.DataTableResult.AsEnumerable()
                         where !ci.IsNull("CompanyInfoId")
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = ci.Field<int>("CompanyInfoId"),
                             ItemInfoType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = ci.Field<int>("CompanyInfoTypeId"),
                                 ItemName = ci.Field<string>("CompanyInfoTypeName"),
                             },
                             Value = ci.Field<string>("Value"),
                             LargeValue = ci.Field<string>("LargeValue"),
                             Enable = ci.Field<UInt64>("CompanyInfoEnable") == 1 ? true : false,
                             LastModify = ci.Field<DateTime>("CompanyInfoLastModify"),
                             CreateDate = ci.Field<DateTime>("CompanyInfoCreateDate"),
                         }).ToList(),
                };
            }
            return oReturn;
        }

        #endregion
    }
}
