using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderBalanceSheetViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount { get; private set; }

        public ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel RelatedBalanceSheetDetail { get; private set; }

        public List<ProviderBalanceSheetViewModel> ChildBalanceSheet { get; set; }

        public ProviderBalanceSheetViewModel() { }

        public ProviderBalanceSheetViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedAccount,
            ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel oRelatedBalanceSheetDetail)
        {
            RelatedAccount = oRelatedAccount;

            RelatedBalanceSheetDetail = oRelatedBalanceSheetDetail;
        }
    }
}
