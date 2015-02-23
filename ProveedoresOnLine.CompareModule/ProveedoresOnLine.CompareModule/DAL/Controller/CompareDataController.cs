using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompareModule.DAL.Controller
{
    internal class CompareDataController : ProveedoresOnLine.CompareModule.Interfaces.ICompareData
    {
        #region singleton instance

        private static ProveedoresOnLine.CompareModule.Interfaces.ICompareData oInstance;
        internal static ProveedoresOnLine.CompareModule.Interfaces.ICompareData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new CompareDataController();
                return oInstance;
            }
        }

        private ProveedoresOnLine.CompareModule.Interfaces.ICompareData DataFactory;

        #endregion

        #region Constructor

        public CompareDataController()
        {
            CompareDataFactory factory = new CompareDataFactory();
            DataFactory = factory.GetCompareInstance();
        }

        #endregion

        #region Implemented methods

        public int CompareUpsert(int? CompareId, string CompareName, string User, bool Enable)
        {
            return DataFactory.CompareUpsert(CompareId, CompareName, User, Enable);
        }

        public int CompareCompanyUpsert(int CompareId, string CompanyPublicId, bool Enable)
        {
            return DataFactory.CompareCompanyUpsert(CompareId, CompanyPublicId, Enable);
        }

        public List<ProveedoresOnLine.CompareModule.Models.CompareModel> CompareSearch(string SearchParam, string User, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.CompareSearch(SearchParam, User, PageNumber, RowCount, out TotalRows);
        }

        public ProveedoresOnLine.CompareModule.Models.CompareModel CompareGetCompanyBasicInfo(int CompareId, string User)
        {
            return DataFactory.CompareGetCompanyBasicInfo(CompareId, User);
        }

        #endregion
    }
}
