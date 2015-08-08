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

        #endregion

        #region Gerencial Report

        public List<GenericItemModel> C_Report_BlackListGetByCompanyPublicId(string CompanyPublicId)
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

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCompanyPublicId", CompanyPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vFinancialType", FinancialType));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "MP_CP_Financial_GetBasicInfo",
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
    }
}
