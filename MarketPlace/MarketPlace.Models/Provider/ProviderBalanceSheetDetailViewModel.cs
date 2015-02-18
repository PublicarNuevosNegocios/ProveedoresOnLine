using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderBalanceSheetDetailViewModel
    {
        public ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel RelatedBalanceSheetDetail { get; set; }

        public int Order { get; set; }
    }
}
