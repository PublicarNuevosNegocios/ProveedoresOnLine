using ProveedoresOnLine.IndexSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.Controller
{
    public class IndexSearch
    {
        #region Company Index

        public static List<CompanyIndexSearchModel> GetCompanyIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetCompanyIndex();
        }

        #endregion

        #region Survey Index

        public static List<SurveyIndexSearchModel> GetSurveyIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetSurveyIndex();
        }

        #region Survey Info Index

        public static List<SurveyInfoIndexSearchModel> GetSurveyInfoIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetSurveyInfoIndex();
        }

        #endregion

        #endregion
    }
}
