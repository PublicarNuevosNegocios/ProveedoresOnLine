using ProveedoresOnLine.IndexSearch.Interfaces;
using ProveedoresOnLine.IndexSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.DAL.Controller
{
    internal class IndexSearchDataController : IIndexSearch
    {
        #region singleton instance

        private static IIndexSearch oInstance;

        internal static IIndexSearch Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new IndexSearchDataController();
                return oInstance;
            }
        }

        private IIndexSearch DataFactory;

        #endregion

        #region Constructor

        public IndexSearchDataController()
        {
            IndexSearchDataFactory factory = new IndexSearchDataFactory();
            DataFactory = factory.GetIndexSearchInstance();
        }

        #endregion

        public List<IndexSearchModel> GetCompanyIndex()
        {
            return DataFactory.GetCompanyIndex();
        }
    }
}
