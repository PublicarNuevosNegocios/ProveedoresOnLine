using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Models.Provider
{
    public class BalanceSheetModel : ProveedoresOnLine.Company.Models.Util.GenericItemModel
    {
        public List<BalanceSheetDetailModel> BalanceSheetInfo { get; set; }
    }
}
