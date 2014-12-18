using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderFinancialViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedFinancial { get; private set; }

        public string FinancialId { get; set; }

        public string FinancialName { get; set; }

        public bool Enable { get; set; }

        #region Balance sheet

        public string SH_Year { get; set; }
        public string SH_YearId { get; set; }

        public string SH_BalanceSheetFile { get; set; }
        public string SH_BalanceSheetFileId { get; set; }

        public string SH_Currency { get; set; }
        public string SH_CurrencyId { get; set; }

        #endregion

        public ProviderFinancialViewModel() { }

        public ProviderFinancialViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedFinancial)
        {
            RelatedFinancial = oRelatedFinancial;

            FinancialId = RelatedFinancial.ItemId.ToString();

            FinancialName = RelatedFinancial.ItemName;

            Enable = RelatedFinancial.Enable;

            #region Balance sheet

            SH_Year = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.SH_Year).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SH_YearId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.SH_Year).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SH_BalanceSheetFile = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.SH_BalanceSheetFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SH_BalanceSheetFileId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.SH_BalanceSheetFile).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SH_Currency = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.SH_Currency).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            SH_CurrencyId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.SH_Currency).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

        }
    }
}
