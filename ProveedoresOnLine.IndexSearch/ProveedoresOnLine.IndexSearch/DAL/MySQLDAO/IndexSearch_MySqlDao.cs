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

        public List<CompanyIndexSearchModel> GetCompanyIndex()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "C_IndexCompany",
                CommandType = System.Data.CommandType.StoredProcedure,
            });

            List<CompanyIndexSearchModel> oReturn = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from idx in response.DataTableResult.AsEnumerable()
                           where !idx.IsNull("CompanyPublicId")
                           group idx by new
                           {
                               CompanyPublicId = idx.Field<string>("CompanyPublicId"),
                               CompanyName = idx.Field<string>("CompanyName"),
                               CompanyIdentificationTypeId = idx.Field<int>("CompanyIdentificationTypeId").ToString(),
                               CompanyIdentificationType = idx.Field<string>("CompanyIdentificationType"),
                               CompanyIdentificationNumber = idx.Field<string>("CompanyIdentificationNumber"),
                               CountryId = idx.Field<int?>("CountryId").ToString(),
                               Country = idx.Field<string>("Country"),
                               CityId = idx.Field<int?>("CityId").ToString(),
                               City = idx.Field<string>("City"),
                               StatusId = idx.Field<int?>("StatusId").ToString(),
                               Status = idx.Field<string>("Status"),
                           }
                               into idxg
                           select new CompanyIndexSearchModel()
                           {
                               CompanyPublicId = idxg.Key.CompanyPublicId,
                               CompanyName = idxg.Key.CompanyName,
                               CompanyIdentificationTypeId = idxg.Key.CompanyIdentificationTypeId,
                               CompanyIdentificationType = idxg.Key.CompanyIdentificationType,
                               CompanyIdentificationNumber = idxg.Key.CompanyIdentificationNumber,
                               CountryId = idxg.Key.CountryId,
                               Country = idxg.Key.Country,
                               CityId = idxg.Key.CityId,
                               City = idxg.Key.City,
                               StatusId = idxg.Key.StatusId,
                               Status = idxg.Key.Status,
                           }).ToList();
            }

            return oReturn;
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