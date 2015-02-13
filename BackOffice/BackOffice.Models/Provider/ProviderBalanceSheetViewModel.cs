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

        #region AccountGetValues

        public int AccountId { get { return RelatedAccount.ItemId; } }

        public bool AccountIsParetn { get { return !(RelatedAccount.ParentItem != null && RelatedAccount.ParentItem.ItemId > 0); } }

        public int AccountType { get { return RelatedAccount.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.AI_IsValue).Select(x => Convert.ToInt32(x.Value.Replace(" ", ""))).DefaultIfEmpty(1).FirstOrDefault(); } }

        #endregion

        public ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel RelatedBalanceSheetDetail { get; private set; }

        #region BalanceSheetGetValues

        #endregion



        public List<ProviderBalanceSheetViewModel> ChildBalanceSheet { get; set; }

        public decimal ChildSum { get; set; }

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
