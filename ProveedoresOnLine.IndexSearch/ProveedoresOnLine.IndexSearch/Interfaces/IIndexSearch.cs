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
        List<IndexSearchModel> GetCompanyIndex();
    }
}
