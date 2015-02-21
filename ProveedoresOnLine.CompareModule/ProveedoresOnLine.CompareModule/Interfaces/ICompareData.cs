using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompareModule.Interfaces
{
    internal interface ICompareData
    {
        int CompareUpsert(int? CompareId, string CompareName, string User, bool Enable);

        int CompareCompanyUpsert(int CompareId, string CompanyPublicId, bool Enable);

        List<ProveedoresOnLine.CompareModule.Models.CompareModel> CompareSearch(string SearchParam, string User, int PageNumber, int RowCount, out int TotalRows);

        ProveedoresOnLine.CompareModule.Models.CompareModel CompareGetCompanyBasicInfo(int CompareId);
    }
}
