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
                         CompanyName = sv.Field<string>("CompanyName"),
                         CompanyIdentificationTypeId = sv.Field<int?>("CompanyIdentificationTypeId").ToString(),
                         CompanyIdentificationType = sv.Field<string>("CompanyIdentificationType"),
                         CompanyIdentificationNumber = sv.Field<string>("CompanyIdentificationNumber"),
                         CountryId = sv.Field<int?>("CountryId").ToString(),
                         Country = sv.Field<string>("Country"),
                         CityId = sv.Field<int?>("CityId").ToString(),
                         City = sv.Field<string>("City"),
                         StatusId = sv.Field<int?>("StatusId").ToString(),
                         Status = sv.Field<string>("Status"),
                         SurveyPublicId = sv.Field<string>("SurveyPublicId"),
                         SurveyTypeId = sv.Field<int?>("SurveyTypeId").ToString(),
                         SurveyType = sv.Field<string>("SurveyType"),
                         SurveyStatusId = sv.Field<int?>("SurveyStatusId").ToString(),
                         SurveyStatus = sv.Field<string>("SurveyStatus"),
                     }
                         into svg
                         select new SurveyIndexSearchModel()
                         {
                             oCompanyIndexSearchModel = new CompanyIndexSearchModel()
                             {
                                 CompanyPublicId = svg.Key.CompanyPublicId,
                                 CompanyName = svg.Key.CompanyName,
                                 CompanyIdentificationTypeId = svg.Key.CompanyIdentificationTypeId,
                                 CompanyIdentificationType = svg.Key.CompanyIdentificationType,
                                 CompanyIdentificationNumber = svg.Key.CompanyIdentificationNumber,
                                 CountryId = svg.Key.CountryId,
                                 Country = svg.Key.Country,
                                 CityId = svg.Key.CityId,
                                 City = svg.Key.City,
                                 StatusId = svg.Key.StatusId,
                                 Status = svg.Key.Status,
                             },
                             SurveyPublicId = svg.Key.SurveyPublicId,
                             SurveyTypeId = svg.Key.SurveyTypeId,
                             SurveyType = svg.Key.SurveyType,
                             SurveyStatusId = svg.Key.SurveyStatusId,
                             SurveyStatus = svg.Key.SurveyStatus,
                         }).ToList();
            }

            return oReturn;
        }

        #endregion

        
    }
}
