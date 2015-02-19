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

        #region AccountGetValues

        public int AccountId { get { return RelatedAccount.ItemId; } }

        public bool AccountIsParetn { get { return !(RelatedAccount.ParentItem != null && RelatedAccount.ParentItem.ItemId > 0); } }

        public int AccountType { get { return RelatedAccount.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_IsValue).Select(x => Convert.ToInt32(x.Value.Replace(" ", ""))).DefaultIfEmpty(1).FirstOrDefault(); } }

        public string AccountFormula { get { return RelatedAccount.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Formula).Select(x => x.LargeValue).DefaultIfEmpty(string.Empty).FirstOrDefault(); } }

        public string AccountValidateFormula { get { return RelatedAccount.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_ValidationRule).Select(x => x.LargeValue).DefaultIfEmpty(null).FirstOrDefault(); } }

        public string AccountUnit { get { return RelatedAccount.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCategoryInfoType.AI_Unit).Select(x => x.Value).DefaultIfEmpty(string.Empty).FirstOrDefault(); } }

        #endregion

        public List<ProviderBalanceSheetDetailViewModel> RelatedBalanceSheetDetail { get; set; }

        public List<ProviderBalanceSheetViewModel> ChildBalanceSheet { get; set; }

        public decimal? HorizontalValue { get; set; }

    }
}
