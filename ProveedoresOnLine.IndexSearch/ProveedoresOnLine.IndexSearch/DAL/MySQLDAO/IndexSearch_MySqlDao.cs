using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.IndexSearch.Interfaces;
using ProveedoresOnLine.IndexSearch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.DAL.MySQLDAO
{
    internal class IndexSearch_MySqlDao : IIndexSearch
    {
        private ADO.Interfaces.IADO DataInstance;

        public IndexSearch_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Models.Constants.C_POL_SearchConnectionName);
        }

        #region Company Index

        public List<CompanyIndexModel> GetCompanyIndex()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_IndexCompany",
                CommandType = System.Data.CommandType.StoredProcedure,
            });

            List<Company.Models.Company.CompanyIndexModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from idx in response.DataTableResult.AsEnumerable()
                           where !idx.IsNull("CompanyPublicId")
                           group idx by new
                           {
                               CompanyPublicId = idx.Field<string>("CompanyPublicId"),
                               CompanyName = idx.Field<string>("CompanyName"),
                               CompanyIdentificationTypeId = !idx.IsNull("CompanyIdentificationTypeId") ? idx.Field<int>("CompanyIdentificationTypeId") : 0,
                               CompanyIdentificationType = idx.Field<string>("CompanyIdentificationType"),
                               CompanyIdentificationNumber = idx.Field<string>("CompanyIdentificationNumber"),
                               CompanyEnable = idx.Field<UInt64>("CompanyEnable") == 1 ? true : false,
                               LogoUrl = idx.Field<string>("LogoUrl"),
                               CountryId = !idx.IsNull("CountryId") ? idx.Field<int>("CountryId") : 0,
                               Country = idx.Field<string>("Country"),
                               CityId = !idx.IsNull("CityId") ? idx.Field<int>("CityId") : 0,
                               City = idx.Field<string>("City"),
                               ICAId = !idx.IsNull("ICAId") ? idx.Field<int>("ICAId") : 0,
                               ICA = idx.Field<string>("ICA"),
                           }
                               into idxg
                               select new CompanyIndexModel()
                           {
                               CompanyPublicId = idxg.Key.CompanyPublicId,
                               CompanyName = idxg.Key.CompanyName,
                               IdentificationTypeId = idxg.Key.CompanyIdentificationTypeId,

                               IdentificationType = idxg.Key.CompanyIdentificationType,

                               IdentificationNumber = idxg.Key.CompanyIdentificationNumber,

                               CompanyEnable = idxg.Key.CompanyEnable,
                               LogoUrl = idxg.Key.LogoUrl,

                               CountryId = idxg.Key.CountryId,
                               Country = idxg.Key.Country,
                               CityId = idxg.Key.CityId,
                               City = idxg.Key.City,

                               ICAId = idxg.Key.ICAId,
                               ICA = idxg.Key.ICA,
                               oCustomerProviderIndexModel =
                                    (from cp in response.DataTableResult.AsEnumerable()
                                     where !cp.IsNull("CustomerProviderId") &&
                                           cp.Field<string>("CompanyPublicId") == idxg.Key.CompanyPublicId
                                     group cp by new
                                     {
                                         CustomerProviderId = !cp.IsNull("CustomerProviderId") ? cp.Field<int>("CustomerProviderId") : 0,
                                         CustomerPublicId = cp.Field<string>("CustomerPublicId"),
                                         StatusId = !cp.IsNull("StatusId") ? cp.Field<int>("StatusId") : 0,
                                         Status = cp.Field<string>("Status"),
                                         CustomerProviderEnable = cp.Field<UInt64>("CustomerProviderEnable") == 1 ? true : false,
                                     }
                                         into cpg
                                         select new CustomerProviderIndexModel()
                                         {
                                             CustomerProviderId = cpg.Key.CustomerProviderId,
                                             CustomerPublicId = cpg.Key.CustomerPublicId,
                                             StatusId = cpg.Key.StatusId,
                                             Status = cpg.Key.Status,
                                             CustomerProviderEnable = cpg.Key.CustomerProviderEnable,
                                         }).ToList()
                           }).ToList();
            }

            return oReturn;
        }

        public List<CustomerProviderIndexModel> GetCustomerProviderIndex()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_IndexCustomerProvider",
                CommandType = CommandType.StoredProcedure,
            });

            List<CustomerProviderIndexModel> oRetun = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetun =
                    (from cp in response.DataTableResult.AsEnumerable()
                     where !cp.IsNull("CustomerProviderId")
                     group cp by new
                     {
                         CustomerProviderId = cp.Field<int>("CustomerProviderId"),
                         CustomerPublicId = cp.Field<string>("CustomerPublicId"),
                         ProviderPublicId = cp.Field<string>("ProviderPublicId"),
                         StatusId = cp.Field<int>("StatusId"),
                         Status = cp.Field<string>("Status"),
                         CustomerProviderEnable = cp.Field<UInt64>("CustomerProviderEnable") == 1 ? true : false,
                     }
                         into cpg
                         select new CustomerProviderIndexModel()
                         {
                             CustomerProviderId = cpg.Key.CustomerProviderId,
                             CustomerPublicId = cpg.Key.CustomerPublicId,
                             ProviderPublicId = cpg.Key.ProviderPublicId,
                             StatusId = cpg.Key.StatusId,
                             Status = cpg.Key.Status,
                             CustomerProviderEnable = cpg.Key.CustomerProviderEnable,
                         }).ToList();
            }

            return oRetun;
        }

        #endregion

        #region Survey Index

        public List<SurveyIndexSearchModel> GetSurveyIndex()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_IndexSurvey",
                CommandType = CommandType.StoredProcedure,
            });

            List<SurveyIndexSearchModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from sv in response.DataTableResult.AsEnumerable()
                     where !sv.IsNull("CompanyPublicId")
                     group sv by new
                     {
                         CompanyPublicId = sv.Field<string>("CompanyPublicId"),
                         SurveyPublicId = sv.Field<string>("SurveyPublicId"),
                         SurveyTypeId = sv.Field<int?>("SurveyTypeId").ToString(),
                         SurveyType = sv.Field<string>("SurveyType"),
                         SurveyStatusId = sv.Field<int?>("SurveyStatusId").ToString(),
                         SurveyStatus = sv.Field<string>("SurveyStatus"),
                     }
                         into svg
                         select new SurveyIndexSearchModel()
                         {
                             CompanyPublicId = svg.Key.CompanyPublicId,
                             SurveyPublicId = svg.Key.SurveyPublicId,
                             SurveyTypeId = svg.Key.SurveyTypeId,
                             SurveyType = svg.Key.SurveyType,
                             SurveyStatusId = svg.Key.SurveyStatusId,
                             SurveyStatus = svg.Key.SurveyStatus,
                         }).ToList();
            }

            return oReturn;
        }

        #region Survey Info Index

        public List<SurveyInfoIndexSearchModel> GetSurveyInfoIndex()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_IndexSurveyInfo",
                CommandType = CommandType.StoredProcedure,
            });

            List<SurveyInfoIndexSearchModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn =
                    (from svinf in response.DataTableResult.AsEnumerable()
                     where !svinf.IsNull("ParentSurveyPublicId")
                     group svinf by new
                     {
                         ParentSurveyPublicId = svinf.Field<string>("ParentSurveyPublicId"),
                         SurveyPublicId = svinf.Field<string>("SurveyPublicId"),
                         UserEmail = svinf.Field<string>("UserEmail"),
                     }
                         into svinfg
                         select new SurveyInfoIndexSearchModel()
                         {
                             ParentSurveyPublicId = svinfg.Key.ParentSurveyPublicId,
                             SurveyPublicId = svinfg.Key.SurveyPublicId,
                             SurveyUserEmail = svinfg.Key.UserEmail,
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        #endregion
    }
}