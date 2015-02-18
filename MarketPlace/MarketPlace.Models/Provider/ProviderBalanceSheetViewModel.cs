using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderBalanceSheetViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount { get; set; }

        public List<ProviderBalanceSheetDetailViewModel> RelatedBalanceSheetDetail { get; set; }

        public List<ProviderBalanceSheetViewModel> ChildBalanceSheet { get; set; }

    }
}
