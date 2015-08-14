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
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.ProjectModule.Models;

namespace ProveedoresOnLine.Reports.DAL.MySQLDAO
{
    internal class Reports_MySqlDao : IReportData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Reports_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.Reports.Models.Constants.R_POL_ReportsConnectionName);
        }

        #region ReportSurveyGetAllByCustomer

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

        public SurveyModel SurveyGetByParentUser(string ParentSurveyPublicId, string User)
        {

            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUser", User));
            lstParams.Add(DataInstance.CreateTypedParameter("vParentSurveyPublicId", ParentSurveyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CP_Survey_GetByUser",
                CommandType = CommandType.StoredProcedure,
                Parameters = lstParams,
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
                    CreateDate = response.DataSetResult.Tables[1].Rows[0].Field<DateTime>("SurveyCreateDate"),
                    User = response.DataSetResult.Tables[1].Rows[0].Field<string>("User"),
                    ParentSurveyPublicId = response.DataSetResult.Tables[1].Rows[0].Field<string>("ParentSurveyId"),

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
        
        public List<ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel> C_Report_BlackListGetBasicInfo(string CompanyPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_BlackList_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from l in response.DataTableResult.AsEnumerable()
                     where !l.IsNull("BlackListId")
                     group l by new
                     {
                         BlackListId = l.Field<int>("BlackListId"),
                         BlackListSatusTypeId = l.Field<int>("BlackListSatusTypeId"),
                         BlackListSatusTypeName = l.Field<string>("BlackListSatusTypeName"),
                         User = l.Field<string>("User"),
                         FileUrl = l.Field<string>("FileUrl"),

                         LegalCreateDate = l.Field<DateTime>("LegalCreateDate"),
                     }
                         into cog
                         select new BlackListModel()
                         {
                             BlackListId = cog.Key.BlackListId,
                             BlackListStatus = new CatalogModel()
                             {
                                 ItemId = cog.Key.BlackListSatusTypeId,
                                 ItemName = cog.Key.BlackListSatusTypeName,
                             },
                             User = cog.Key.User,
                             FileUrl = cog.Key.FileUrl,
                             BlackListInfo =
                                 (from coinf in response.DataTableResult.AsEnumerable()
                                  where !coinf.IsNull("BlackListInfoId") &&
                                          coinf.Field<int>("BlackListId") == cog.Key.BlackListId
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = coinf.Field<int>("BlackListinfoId"),
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemName = coinf.Field<string>("BlacklistInfoType"),
                                      },
                                      Value = coinf.Field<string>("Value"),
                                      CreateDate = coinf.Field<DateTime>("CreateDate"),
                                  }).ToList(),

                         }).ToList();
            }
            return oReturn;
        }

        public Company.Models.Company.CompanyModel C_Report_MPCompanyGetBasicInfo(string CompanyPublicId)
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

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vContactType", ContactType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_C_Contact_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from co in response.DataTableResult.AsEnumerable()
                     where !co.IsNull("ContactId")
                     group co by new
                     {
                         ContactId = co.Field<int>("ContactId"),
                         ContactTypeId = co.Field<int>("ContactTypeId"),
                         ContactTypeName = co.Field<string>("ContactTypeName"),
                         ContactName = co.Field<string>("ContactName"),
                         ContactEnable = co.Field<UInt64>("ContactEnable") == 1 ? true : false,
                         ContactLastModify = co.Field<DateTime>("ContactLastModify"),
                         ContactCreateDate = co.Field<DateTime>("ContactCreateDate"),
                     } into cog
                     select new GenericItemModel()
                     {
                         ItemId = cog.Key.ContactId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = cog.Key.ContactTypeId,
                             ItemName = cog.Key.ContactTypeName
                         },
                         ItemName = cog.Key.ContactName,
                         Enable = cog.Key.ContactEnable,
                         LastModify = cog.Key.ContactLastModify,
                         CreateDate = cog.Key.ContactCreateDate,
                         ItemInfo =
                             (from coinf in response.DataTableResult.AsEnumerable()
                              where !coinf.IsNull("ContactInfoId") &&
                                      coinf.Field<int>("ContactId") == cog.Key.ContactId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = coinf.Field<int>("ContactInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = coinf.Field<int>("ContactInfoTypeId"),
                                      ItemName = coinf.Field<string>("ContactInfoTypeName"),
                                  },
                                  Value = coinf.Field<string>("Value"),
                                  LargeValue = coinf.Field<string>("LargeValue"),
                                  Enable = coinf.Field<UInt64>("ContactInfoEnable") == 1 ? true : false,
                                  LastModify = coinf.Field<DateTime>("ContactInfoLastModify"),
                                  CreateDate = coinf.Field<DateTime>("ContactInfoCreateDate"),
                              }).ToList(),

                     }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vLegalType", LegalType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Legal_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from l in response.DataTableResult.AsEnumerable()
                     where !l.IsNull("LegalId")
                     group l by new
                     {
                         LegalId = l.Field<int>("LegalId"),
                         LegalTypeId = l.Field<int>("LegalTypeId"),
                         LegalTypeName = l.Field<string>("LegalTypeName"),
                         LegalName = l.Field<string>("LegalName"),
                         LegalEnable = l.Field<UInt64>("LegalEnable") == 1 ? true : false,
                         LegalLastModify = l.Field<DateTime>("LegalLastModify"),
                         LegalCreateDate = l.Field<DateTime>("LegalCreateDate"),
                     }
                         into cog
                         select new GenericItemModel()
                         {
                             ItemId = cog.Key.LegalId,
                             ItemType = new CatalogModel()
                             {
                                 ItemId = cog.Key.LegalTypeId,
                                 ItemName = cog.Key.LegalTypeName
                             },
                             ItemName = cog.Key.LegalName,
                             Enable = cog.Key.LegalEnable,
                             LastModify = cog.Key.LegalLastModify,
                             CreateDate = cog.Key.LegalCreateDate,
                             ItemInfo =
                                 (from coinf in response.DataTableResult.AsEnumerable()
                                  where !coinf.IsNull("LegalInfoId") &&
                                          coinf.Field<int>("LegalId") == cog.Key.LegalId
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = coinf.Field<int>("LegalInfoId"),
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = coinf.Field<int>("LegalInfoTypeId"),
                                          ItemName = coinf.Field<string>("LegalInfoTypeName"),
                                      },
                                      Value = coinf.Field<string>("Value"),
                                      LargeValue = coinf.Field<string>("LargeValue"),
                                      Enable = coinf.Field<UInt64>("LegalInfoEnable") == 1 ? true : false,
                                      LastModify = coinf.Field<DateTime>("LegalInfoLastModify"),
                                      CreateDate = coinf.Field<DateTime>("LegalInfoCreateDate"),
                                  }).ToList(),

                         }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CustomerProvider_GetTracking",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bl in response.DataTableResult.AsEnumerable()
                     where !bl.IsNull("CustomerProviderInfoId")
                     group bl by new
                     {
                         CustomerProviderInfoId = bl.Field<int>("CustomerProviderInfoId"),
                         Seguimiento = bl.Field<string>("Seguimiento"),
                         CreateDate = bl.Field<DateTime>("CreateDate"),
                         Status = bl.Field<string>("Status"),
                     } into blg
                     select new GenericItemModel()
                     {
                         ItemId = blg.Key.CustomerProviderInfoId,
                         ItemType = new CatalogModel()
                         {
                             ItemName = blg.Key.Status
                         },
                         CreateDate = blg.Key.CreateDate,
                         ItemName = blg.Key.Seguimiento,
                     }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetLastyearInfoDeta(string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Financial_GetLastyearInfoDeta",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from bl in response.DataTableResult.AsEnumerable()
                     where !bl.IsNull("CompanyId")
                     group bl by new
                     {
                         CompanyId = bl.Field<int>("CompanyId"),
                         Year = bl.Field<string>("Year"),
                     } into blg
                     select new GenericItemModel()
                     {
                         ItemId = blg.Key.CompanyId,
                         ItemName = blg.Key.Year,
                         ItemInfo =
                              (from blinf in response.DataTableResult.AsEnumerable()
                               where !blinf.IsNull("FinancialInfoId") &&
                                       blinf.Field<int>("CompanyId") == blg.Key.CompanyId
                               select new GenericItemInfoModel()
                               {
                                   ItemInfoId = blinf.Field<int>("FinancialInfoId"),
                                   ItemInfoType = new CatalogModel()
                                   {
                                       ItemId = blinf.Field<int>("Account"),
                                   },
                                   Value = blinf.Field<decimal>("Value").ToString(),
                                   ValueName = blinf.Field<string>("Currency"),
                               }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialType", FinancialType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "CP_Financial_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from fi in response.DataTableResult.AsEnumerable()
                     where !fi.IsNull("FinancialId")
                     group fi by new
                     {
                         FinancialId = fi.Field<int>("FinancialId"),
                         FinancialTypeId = fi.Field<int>("FinancialTypeId"),
                         FinancialTypeName = fi.Field<string>("FinancialTypeName"),
                         FinancialName = fi.Field<string>("FinancialName"),
                         FinancialEnable = fi.Field<UInt64>("FinancialEnable") == 1 ? true : false,
                         FinancialLastModify = fi.Field<DateTime>("FinancialLastModify"),
                         FinancialCreateDate = fi.Field<DateTime>("FinancialCreateDate"),
                     } into fig
                     select new GenericItemModel()
                     {
                         ItemId = fig.Key.FinancialId,
                         ItemType = new CatalogModel()
                         {
                             ItemId = fig.Key.FinancialTypeId,
                             ItemName = fig.Key.FinancialTypeName
                         },
                         ItemName = fig.Key.FinancialName,
                         Enable = fig.Key.FinancialEnable,
                         LastModify = fig.Key.FinancialLastModify,
                         CreateDate = fig.Key.FinancialCreateDate,
                         ItemInfo =
                             (from fiinf in response.DataTableResult.AsEnumerable()
                              where !fiinf.IsNull("FinancialInfoId") &&
                                      fiinf.Field<int>("FinancialId") == fig.Key.FinancialId
                              select new GenericItemInfoModel()
                              {
                                  ItemInfoId = fiinf.Field<int>("FinancialInfoId"),
                                  ItemInfoType = new CatalogModel()
                                  {
                                      ItemId = fiinf.Field<int>("FinancialInfoTypeId"),
                                      ItemName = fiinf.Field<string>("FinancialInfoTypeName"),
                                  },
                                  Value = fiinf.Field<string>("Value"),
                                  LargeValue = fiinf.Field<string>("LargeValue"),
                                  Enable = fiinf.Field<UInt64>("FinancialInfoEnable") == 1 ? true : false,
                                  LastModify = fiinf.Field<DateTime>("FinancialInfoLastModify"),
                                  CreateDate = fiinf.Field<DateTime>("FinancialInfoCreateDate"),
                              }).ToList(),
                     }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCertificationType", CertificationType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Certification_GetBasicInfo",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from c in response.DataTableResult.AsEnumerable()
                     where !c.IsNull("CertificationId")
                     group c by new
                     {
                         CertificationId = c.Field<int>("CertificationId"),
                         CertificationTypeId = c.Field<int>("CertificationTypeId"),
                         CertificationTypeName = c.Field<string>("CertificationTypeName"),
                         CertificationName = c.Field<string>("CertificationName"),
                         CertificationEnable = c.Field<UInt64>("CertificationEnable") == 1 ? true : false,
                         CertificationLastModify = c.Field<DateTime>("CertificationLastModify"),
                         CertificationCreateDate = c.Field<DateTime>("CertificationCreateDate"),
                     }
                         into cog
                         select new GenericItemModel()
                         {
                             ItemId = cog.Key.CertificationId,
                             ItemType = new CatalogModel()
                             {
                                 ItemId = cog.Key.CertificationTypeId,
                                 ItemName = cog.Key.CertificationTypeName
                             },
                             ItemName = cog.Key.CertificationName,
                             Enable = cog.Key.CertificationEnable,
                             LastModify = cog.Key.CertificationLastModify,
                             CreateDate = cog.Key.CertificationCreateDate,
                             ItemInfo =
                                 (from coinf in response.DataTableResult.AsEnumerable()
                                  where !coinf.IsNull("CertificationInfoId") &&
                                          coinf.Field<int>("CertificationId") == cog.Key.CertificationId
                                  select new GenericItemInfoModel()
                                  {
                                      ItemInfoId = coinf.Field<int>("CertificationInfoId"),
                                      ItemInfoType = new CatalogModel()
                                      {
                                          ItemId = coinf.Field<int>("CertificationInfoTypeId"),
                                          ItemName = coinf.Field<string>("CertificationInfoTypeName"),
                                      },
                                      Value = coinf.Field<string>("Value"),
                                      LargeValue = coinf.Field<string>("LargeValue"),
                                      ValueName = coinf.Field<string>("ValueName"),
                                      Enable = coinf.Field<UInt64>("CertificationInfoEnable") == 1 ? true : false,
                                      LastModify = coinf.Field<DateTime>("CertificationInfoLastModify"),
                                      CreateDate = coinf.Field<DateTime>("CertificationInfoCreateDate"),
                                  }).ToList(),

                         }).ToList();
            }
            return oReturn;
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> C_Report_MPCertificationGetSpecificCert(string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_CertificationGetSpecificCert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            List<GenericItemInfoModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from cerinf in response.DataTableResult.AsEnumerable()
                     where !cerinf.IsNull("CompanyId")
                     select new GenericItemInfoModel()
                     {
                         ItemInfoId = cerinf.Field<int>("CompanyId"),
                         ItemInfoType = new CatalogModel()
                         {
                             ItemId = cerinf.Field<int>("CategoryId"),
                             ItemName = cerinf.Field<string>("CategoryName"),
                         },
                         Value = cerinf.Field<string>("CCS"),
                         LargeValue = cerinf.Field<string>("IsCertified"),
                     }).ToList();
            }
            return oReturn;
        }

        #endregion

        #region SelectionProcess Report
        
        public ProjectModel ProjectGetByIdProviderDetail(string ProjectPublicId, string CustomerPublicId, string ProviderPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vProjectPublicId", ProjectPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCustomerPublicId", CustomerPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderPublicId", ProviderPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataSet,
                CommandText = "MP_CC_Project_GetById_ProviderDetail",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            ProjectModel oReturn = null;

            if (response.DataSetResult != null &&
                response.DataSetResult.Tables.Count > 8 &&
                response.DataSetResult.Tables[0] != null &&
                response.DataSetResult.Tables[0].Rows.Count > 0 &&
                response.DataSetResult.Tables[1] != null &&
                response.DataSetResult.Tables[2] != null &&
                response.DataSetResult.Tables[3] != null &&
                response.DataSetResult.Tables[4] != null &&
                response.DataSetResult.Tables[5] != null &&
                response.DataSetResult.Tables[6] != null &&
                response.DataSetResult.Tables[7] != null &&
                response.DataSetResult.Tables[8] != null)
            {
                oReturn = new ProjectModel()
                {
                    ProjectPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId"),
                    ProjectName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectName"),
                    ProjectStatus = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectStatusId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectStatusName"),
                    },
                    LastModify = response.DataSetResult.Tables[0].Rows[0].Field<DateTime>("ProjectLastModify"),

                    #region ProjectInfo

                    ProjectInfo =
                        (from pjinf in response.DataSetResult.Tables[0].AsEnumerable()
                         where !pjinf.IsNull("ProjectInfoId") &&
                                pjinf.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group pjinf by new
                         {
                             ProjectInfoId = pjinf.Field<int>("ProjectInfoId"),
                             ProjectInfoTypeId = pjinf.Field<int>("ProjectInfoTypeId"),
                             ProjectInfoTypeName = pjinf.Field<string>("ProjectInfoTypeName"),
                             ProjectInfoValue = pjinf.Field<string>("ProjectInfoValue"),
                             ProjectInfoLargeValue = pjinf.Field<string>("ProjectInfoLargeValue"),
                             ProjectInfoValueName = pjinf.Field<string>("ProjectInfoValueName"),
                         } into pjinfg
                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                         {
                             ItemInfoId = pjinfg.Key.ProjectInfoId,
                             ItemInfoType = new Company.Models.Util.CatalogModel()
                             {
                                 ItemId = pjinfg.Key.ProjectInfoTypeId,
                                 ItemName = pjinfg.Key.ProjectInfoTypeName,
                             },
                             Value = pjinfg.Key.ProjectInfoValue,
                             LargeValue = pjinfg.Key.ProjectInfoLargeValue,
                             ValueName = pjinfg.Key.ProjectInfoValueName,
                         }).ToList(),

                    #endregion

                    #region Project Config

                    RelatedProjectConfig = new ProjectConfigModel()
                    {
                        ItemId = (int)response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectConfigId"),
                        ItemName = response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectConfigName"),

                        RelatedCustomer = new CompanyCustomer.Models.Customer.CustomerModel()
                        {
                            RelatedCompany = new Company.Models.Company.CompanyModel()
                            {
                                CompanyPublicId = response.DataSetResult.Tables[0].Rows[0].Field<string>("CustomerPublicId"),
                            },
                        },

                        RelatedEvaluationItem =
                            (from pjei in response.DataSetResult.Tables[2].AsEnumerable()
                             where !pjei.IsNull("EvaluationItemId") &&
                                    pjei.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                    pjei.Field<Int64>("ProjectConfigId") == response.DataSetResult.Tables[0].Rows[0].Field<Int64>("ProjectConfigId")
                             group pjei by new
                             {
                                 EvaluationItemId = pjei.Field<int>("EvaluationItemId"),
                                 EvaluationItemName = pjei.Field<string>("EvaluationItemName"),
                                 EvaluationItemTypeId = pjei.Field<int>("EvaluationItemTypeId"),
                                 EvaluationItemTypeName = pjei.Field<string>("EvaluationItemTypeName"),
                                 ParentEvaluationItem = pjei.Field<int?>("ParentEvaluationItem"),
                             } into pjeig
                             select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                             {
                                 ItemId = pjeig.Key.EvaluationItemId,
                                 ItemName = pjeig.Key.EvaluationItemName,
                                 ItemType = new Company.Models.Util.CatalogModel()
                                 {
                                     ItemId = pjeig.Key.EvaluationItemTypeId,
                                     ItemName = pjeig.Key.EvaluationItemTypeName,
                                 },
                                 ParentItem = pjeig.Key.ParentEvaluationItem == null ? null :
                                    new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = pjeig.Key.ParentEvaluationItem.Value,
                                    },

                                 ItemInfo =
                                     (from pjeiinf in response.DataSetResult.Tables[2].AsEnumerable()
                                      where !pjeiinf.IsNull("EvaluationItemInfoId") &&
                                             pjeiinf.Field<int>("EvaluationItemId") == pjeig.Key.EvaluationItemId
                                      group pjeiinf by new
                                      {
                                          EvaluationItemInfoId = pjeiinf.Field<int>("EvaluationItemInfoId"),
                                          EvaluationItemInfoTypeId = pjeiinf.Field<int>("EvaluationItemInfoTypeId"),
                                          EvaluationItemInfoTypeName = pjeiinf.Field<string>("EvaluationItemInfoTypeName"),
                                          EvaluationItemInfoValue = pjeiinf.Field<string>("EvaluationItemInfoValue"),
                                          EvaluationItemInfoLargeValue = pjeiinf.Field<string>("EvaluationItemInfoLargeValue"),
                                      } into pjeiinfg
                                      select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                      {
                                          ItemInfoId = pjeiinfg.Key.EvaluationItemInfoId,
                                          ItemInfoType = new Company.Models.Util.CatalogModel()
                                          {
                                              ItemId = pjeiinfg.Key.EvaluationItemInfoTypeId,
                                              ItemName = pjeiinfg.Key.EvaluationItemInfoTypeName,
                                          },
                                          Value = pjeiinfg.Key.EvaluationItemInfoValue,
                                          LargeValue = pjeiinfg.Key.EvaluationItemInfoLargeValue,
                                      }).ToList(),
                             }).ToList(),
                    },

                    #endregion

                    #region Project Provider

                    RelatedProjectProvider =
                        (from rpp in response.DataSetResult.Tables[1].AsEnumerable()
                         where !rpp.IsNull("ProjectCompanyId") &&
                                 rpp.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                         group rpp by new
                         {
                             ProjectCompanyId = (int)rpp.Field<Int64>("ProjectCompanyId"),
                             ProjectCompanyLastModify = rpp.Field<DateTime>("ProjectCompanyLastModify"),

                             ProviderPublicId = rpp.Field<string>("ProviderPublicId"),
                             CompanyName = rpp.Field<string>("CompanyName"),
                             IdentificationTypeId = (int)rpp.Field<Int64>("IdentificationTypeId"),
                             IdentificationTypeName = rpp.Field<string>("IdentificationTypeName"),
                             IdentificationNumber = rpp.Field<string>("IdentificationNumber"),
                             CompanyTypeId = (int)rpp.Field<Int64>("CompanyTypeId"),
                             CompanyTypeName = rpp.Field<string>("CompanyTypeName"),

                         } into rppg
                         select new ProjectProviderModel()
                         {
                             ProjectCompanyId = rppg.Key.ProjectCompanyId,
                             LastModify = rppg.Key.ProjectCompanyLastModify,
                             ItemInfo =
                              (from rppinf in response.DataSetResult.Tables[3].AsEnumerable()
                               where !rppinf.IsNull("ProjectCompanyInfoId") &&
                                       rppinf.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId
                               group rppinf by new
                               {
                                   ProjectCompanyInfoId = rppinf.Field<int>("ProjectCompanyInfoId"),
                                   EvaluationItemId = rppinf.Field<int?>("EvaluationItemId"),
                                   ProjectCompanyInfoTypeId = rppinf.Field<int>("ProjectCompanyInfoTypeId"),
                                   ProjectCompanyInfoTypeName = rppinf.Field<string>("ProjectCompanyInfoTypeName"),
                                   ProjectCompanyInfoValue = rppinf.Field<string>("ProjectCompanyInfoValue"),
                                   ProjectCompanyInfoLargeValue = rppinf.Field<string>("ProjectCompanyInfoLargeValue"),
                               } into rppinfg
                               select new ProjectProviderInfoModel()
                               {
                                   ItemInfoId = rppinfg.Key.ProjectCompanyInfoId,
                                   RelatedEvaluationItem = rppinfg.Key.EvaluationItemId == null ? null :
                                        new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                        {
                                            ItemId = rppinfg.Key.EvaluationItemId.Value,
                                        },
                                   ItemInfoType = new Company.Models.Util.CatalogModel()
                                   {
                                       ItemId = rppinfg.Key.ProjectCompanyInfoTypeId,
                                       ItemName = rppinfg.Key.ProjectCompanyInfoTypeName,
                                   },
                                   Value = rppinfg.Key.ProjectCompanyInfoValue,
                                   LargeValue = rppinfg.Key.ProjectCompanyInfoLargeValue,
                               }).ToList(),

                             #region ProviderInfo

                             RelatedProvider = new CompanyProvider.Models.Provider.ProviderModel()
                             {
                                 RelatedCompany = new Company.Models.Company.CompanyModel()
                                 {
                                     CompanyPublicId = rppg.Key.ProviderPublicId,
                                     CompanyName = rppg.Key.CompanyName,
                                     IdentificationType = new Company.Models.Util.CatalogModel()
                                     {
                                         ItemId = rppg.Key.IdentificationTypeId,
                                         ItemName = rppg.Key.IdentificationTypeName,
                                     },
                                     IdentificationNumber = rppg.Key.IdentificationNumber,
                                     CompanyType = new Company.Models.Util.CatalogModel()
                                     {
                                         ItemId = rppg.Key.CompanyTypeId,
                                         ItemName = rppg.Key.CompanyTypeName,
                                     },

                                     CompanyInfo =
                                        (from prvinf in response.DataSetResult.Tables[1].AsEnumerable()
                                         where !prvinf.IsNull("CompanyInfoId") &&
                                                 prvinf.Field<string>("ProviderPublicId") == rppg.Key.ProviderPublicId
                                         group prvinf by new
                                         {
                                             CompanyInfoId = prvinf.Field<int>("CompanyInfoId"),
                                             CompanyInfoTypeId = prvinf.Field<int>("CompanyInfoTypeId"),
                                             CompanyInfoTypeName = prvinf.Field<string>("CompanyInfoTypeName"),
                                             CompanyInfoValue = prvinf.Field<string>("CompanyInfoValue"),
                                             CompanyInfoLargeValue = prvinf.Field<string>("CompanyInfoLargeValue"),
                                         } into prvinfg
                                         select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                         {
                                             ItemInfoId = prvinfg.Key.CompanyInfoId,
                                             ItemInfoType = new Company.Models.Util.CatalogModel()
                                             {
                                                 ItemId = prvinfg.Key.CompanyInfoTypeId,
                                                 ItemName = prvinfg.Key.CompanyInfoTypeName,
                                             },
                                             Value = prvinfg.Key.CompanyInfoValue,
                                             LargeValue = prvinfg.Key.CompanyInfoLargeValue,
                                         }).ToList()
                                 },

                                 #region Commercial

                                 RelatedCommercial =
                                   (from prvcm in response.DataSetResult.Tables[4].AsEnumerable()
                                    where !prvcm.IsNull("CommercialId") &&
                                           prvcm.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvcm.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvcm by new
                                    {
                                        CommercialId = prvcm.Field<int>("CommercialId"),
                                        CommercialType = prvcm.Field<int>("CommercialType"),
                                    } into prvcmg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvcmg.Key.CommercialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvcmg.Key.CommercialType,
                                        },
                                        ItemInfo =
                                           (from prvcminf in response.DataSetResult.Tables[4].AsEnumerable()
                                            where !prvcminf.IsNull("CommercialInfoId") &&
                                                   prvcminf.Field<int>("CommercialId") == prvcmg.Key.CommercialId
                                            group prvcminf by new
                                            {
                                                CommercialInfoId = prvcminf.Field<int>("CommercialInfoId"),
                                                CommercialInfoTypeId = prvcminf.Field<int>("CommercialInfoTypeId"),
                                                CommercialInfoValue = prvcminf.Field<string>("CommercialInfoValue"),
                                                CommercialInfoLargeValue = prvcminf.Field<string>("CommercialInfoLargeValue"),
                                            } into prvcminfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvcminfg.Key.CommercialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvcminfg.Key.CommercialInfoTypeId,
                                                },
                                                Value = prvcminfg.Key.CommercialInfoValue,
                                                LargeValue = prvcminfg.Key.CommercialInfoLargeValue,
                                                ValueName = prvcminfg.Key.CommercialInfoTypeId == 302013 ||
                                                            prvcminfg.Key.CommercialInfoTypeId == 302014 ?
                                                    string.Join(";",
                                                        (from prvcminfn in response.DataSetResult.Tables[4].AsEnumerable()
                                                         where !prvcminfn.IsNull("CommercialInfoValueName") &&
                                                                prvcminfn.Field<int>("CommercialInfoId") == prvcminfg.Key.CommercialInfoId &&
                                                                !string.IsNullOrEmpty(prvcminfn.Field<string>("CommercialInfoValueName"))
                                                         group prvcminfn by new
                                                         {
                                                             CommercialInfoValueName = prvcminfn.Field<string>("CommercialInfoValueName"),
                                                         } into prvcminfng
                                                         select prvcminfng.Key.CommercialInfoValueName).ToList())
                                                    : string.Empty,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Certification

                                 RelatedCertification =
                                   (from prvcr in response.DataSetResult.Tables[5].AsEnumerable()
                                    where !prvcr.IsNull("CertificationId") &&
                                           prvcr.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvcr.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvcr by new
                                    {
                                        CertificationId = prvcr.Field<int>("CertificationId"),
                                        CertificationType = prvcr.Field<int>("CertificationType"),
                                    } into prvcrg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvcrg.Key.CertificationId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvcrg.Key.CertificationType,
                                        },
                                        ItemInfo =
                                           (from prvcrinf in response.DataSetResult.Tables[5].AsEnumerable()
                                            where !prvcrinf.IsNull("CertificationInfoId") &&
                                                   prvcrinf.Field<int>("CertificationId") == prvcrg.Key.CertificationId
                                            group prvcrinf by new
                                            {
                                                CertificationInfoId = prvcrinf.Field<int>("CertificationInfoId"),
                                                CertificationInfoType = prvcrinf.Field<int>("CertificationInfoType"),
                                                CertificationInfoValue = prvcrinf.Field<string>("CertificationInfoValue"),
                                                CertificationInfoLargeValue = prvcrinf.Field<string>("CertificationInfoLargeValue"),
                                            } into prvcrinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvcrinfg.Key.CertificationInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvcrinfg.Key.CertificationInfoType,
                                                },
                                                Value = prvcrinfg.Key.CertificationInfoValue,
                                                LargeValue = prvcrinfg.Key.CertificationInfoLargeValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Financial

                                 RelatedFinantial =
                                   (from prvfi in response.DataSetResult.Tables[6].AsEnumerable()
                                    where !prvfi.IsNull("FinancialId") &&
                                           (int)prvfi.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvfi.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                           prvfi.Field<int>("FinancialType") != 501001
                                    group prvfi by new
                                    {
                                        FinancialId = prvfi.Field<int>("FinancialId"),
                                        FinancialType = prvfi.Field<int>("FinancialType"),
                                    } into prvfig
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvfig.Key.FinancialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvfig.Key.FinancialType,
                                        },
                                        ItemInfo =
                                           (from prvfiinf in response.DataSetResult.Tables[6].AsEnumerable()
                                            where !prvfiinf.IsNull("FinancialInfoId") &&
                                                   prvfiinf.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                            group prvfiinf by new
                                            {
                                                FinancialInfoId = prvfiinf.Field<int>("FinancialInfoId"),
                                                FinancialInfoType = prvfiinf.Field<int>("FinancialInfoType"),
                                                FinancialInfoValue = prvfiinf.Field<string>("FinancialInfoValue"),
                                                FinancialInfoLargeValue = prvfiinf.Field<string>("FinancialInfoLargeValue"),
                                            } into prvfiinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvfiinfg.Key.FinancialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvfiinfg.Key.FinancialInfoType,
                                                },
                                                Value = prvfiinfg.Key.FinancialInfoValue,
                                                LargeValue = prvfiinfg.Key.FinancialInfoLargeValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region BalanceSheet

                                 RelatedBalanceSheet =
                                   (from prvfi in response.DataSetResult.Tables[6].AsEnumerable()
                                    where !prvfi.IsNull("FinancialId") &&
                                           prvfi.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvfi.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId") &&
                                           prvfi.Field<int>("FinancialType") == 501001
                                    group prvfi by new
                                    {
                                        FinancialId = prvfi.Field<int>("FinancialId"),
                                        FinancialType = prvfi.Field<int>("FinancialType"),
                                    } into prvfig
                                    select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel()
                                    {
                                        ItemId = prvfig.Key.FinancialId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvfig.Key.FinancialType,
                                        },
                                        ItemInfo =
                                           (from prvfiinf in response.DataSetResult.Tables[6].AsEnumerable()
                                            where !prvfiinf.IsNull("FinancialInfoId") &&
                                                   prvfiinf.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                            group prvfiinf by new
                                            {
                                                FinancialInfoId = prvfiinf.Field<int>("FinancialInfoId"),
                                                FinancialInfoType = prvfiinf.Field<int>("FinancialInfoType"),
                                                FinancialInfoValue = prvfiinf.Field<string>("FinancialInfoValue"),
                                                FinancialInfoLargeValue = prvfiinf.Field<string>("FinancialInfoLargeValue"),
                                            } into prvfiinfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvfiinfg.Key.FinancialInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvfiinfg.Key.FinancialInfoType,
                                                },
                                                Value = prvfiinfg.Key.FinancialInfoValue,
                                                LargeValue = prvfiinfg.Key.FinancialInfoLargeValue,
                                            }).ToList(),

                                        BalanceSheetInfo =
                                            (from prvbs in response.DataSetResult.Tables[7].AsEnumerable()
                                             where !prvbs.IsNull("BalanceSheetId") &&
                                                    prvbs.Field<int>("FinancialId") == prvfig.Key.FinancialId
                                             group prvbs by new
                                             {
                                                 BalanceSheetId = prvbs.Field<int>("BalanceSheetId"),
                                                 Account = prvbs.Field<int>("Account"),
                                                 BalanceSheetValue = prvbs.Field<decimal>("BalanceSheetValue"),
                                             } into prvbsg
                                             select new ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel()
                                             {
                                                 BalanceSheetId = prvbsg.Key.BalanceSheetId,
                                                 RelatedAccount = new Company.Models.Util.GenericItemModel()
                                                 {
                                                     ItemId = prvbsg.Key.Account,
                                                 },
                                                 Value = prvbsg.Key.BalanceSheetValue,
                                             }).ToList(),
                                    }).ToList(),

                                 #endregion

                                 #region Legal

                                 RelatedLegal =
                                   (from prvlg in response.DataSetResult.Tables[8].AsEnumerable()
                                    where !prvlg.IsNull("LegalId") &&
                                           prvlg.Field<Int64>("ProjectCompanyId") == rppg.Key.ProjectCompanyId &&
                                           prvlg.Field<string>("ProjectPublicId") == response.DataSetResult.Tables[0].Rows[0].Field<string>("ProjectPublicId")
                                    group prvlg by new
                                    {
                                        LegalId = prvlg.Field<int>("LegalId"),
                                        LegalType = prvlg.Field<int>("LegalType"),
                                    } into prvlgg
                                    select new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                    {
                                        ItemId = prvlgg.Key.LegalId,
                                        ItemType = new Company.Models.Util.CatalogModel()
                                        {
                                            ItemId = prvlgg.Key.LegalType,
                                        },
                                        ItemInfo =
                                           (from prvlginf in response.DataSetResult.Tables[8].AsEnumerable()
                                            where !prvlginf.IsNull("LegalInfoId") &&
                                                   prvlginf.Field<int>("LegalId") == prvlgg.Key.LegalId
                                            group prvlginf by new
                                            {
                                                LegalInfoId = prvlginf.Field<int>("LegalInfoId"),
                                                LegalInfoType = prvlginf.Field<int>("LegalInfoType"),
                                                LegalInfoValue = prvlginf.Field<string>("LegalInfoValue"),
                                                LegalInfoLargeValue = prvlginf.Field<string>("LegalInfoLargeValue"),
                                            } into prvlginfg
                                            select new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                                            {
                                                ItemInfoId = prvlginfg.Key.LegalInfoId,
                                                ItemInfoType = new Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = prvlginfg.Key.LegalInfoType,
                                                },
                                                Value = prvlginfg.Key.LegalInfoValue,
                                                LargeValue = prvlginfg.Key.LegalInfoLargeValue,
                                            }).ToList(),
                                    }).ToList(),

                                 #endregion
                             },

                             #endregion

                         }).ToList(),

                    #endregion
                };
            }

            return oReturn;
        }
        


        #endregion

    }
}
