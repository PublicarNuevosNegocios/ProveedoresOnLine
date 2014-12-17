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


        public string AccountId
        {
            get
            {
                if (RelatedAccount != null)
                    return RelatedAccount.ItemId.ToString();
                return string.Empty;
            }
        }

        public string AccountName
        {
            get
            {
                if (RelatedAccount != null)
                    return RelatedAccount.ItemName;
                return string.Empty;
            }
        }

        public string BalanceItemId
        {
            get
            {
                if (RelatedBalanceSheetDetail != null)
                    return RelatedBalanceSheetDetail.BalanceSheetId.ToString();
                return string.Empty;
            }
        }

        public decimal BalanceItemValue
        {
            get
            {
                if (RelatedBalanceSheetDetail != null)
                    return RelatedBalanceSheetDetail.Value;
                return 0;
            }
        }

        public bool BalanceItemIsValue
        {
            get
            {
                if (ChildBalanceSheet != null && ChildBalanceSheet.Count > 0)
                    return false;
                return true;
            }
        }


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
