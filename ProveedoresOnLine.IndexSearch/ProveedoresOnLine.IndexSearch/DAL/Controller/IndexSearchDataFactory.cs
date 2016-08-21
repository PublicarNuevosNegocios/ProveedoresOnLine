using ProveedoresOnLine.IndexSearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.DAL.Controller
{
    internal class IndexSearchDataFactory
    {
        public IIndexSearch GetIndexSearchInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.IndexSearch.DAL.MySQLDAO.IndexSearch_MySqlDao,ProveedoresOnLine.IndexSearch");
            ProveedoresOnLine.IndexSearch.Interfaces.IIndexSearch oRetorno = (ProveedoresOnLine.IndexSearch.Interfaces.IIndexSearch)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
