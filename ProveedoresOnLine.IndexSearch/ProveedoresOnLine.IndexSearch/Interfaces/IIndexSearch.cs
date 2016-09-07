using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.IndexSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.Interfaces
{
    internal interface IIndexSearch
    {
        #region Company Index

        List<CompanyIndexModel> GetCompanyIndex();

        List<CustomerProviderIndexModel> GetCustomerProviderIndex();

        #endregion

        #region Survey Index

        List<SurveyIndexSearchModel> GetSurveyIndex();

        #region Survey Info Index

        List<SurveyInfoIndexSearchModel> GetSurveyInfoIndex();

        #endregion

        #endregion
    }
}
