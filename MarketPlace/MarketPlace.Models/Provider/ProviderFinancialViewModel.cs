using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderFinancialViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedFinancialInfo { get; private set; }

        public ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel RelatedBalanceSheetInfo { get; private set; }

        #region BalanceSheet

        private string oSH_Year;
        public string SH_Year
        {
            get
            {
                if (RelatedBalanceSheetInfo != null && string.IsNullOrEmpty(oSH_Year))
                {
                    oSH_Year = RelatedBalanceSheetInfo.ItemInfo.
                               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.SH_Year).
                               Select(y => y.Value).
                               DefaultIfEmpty(string.Empty).
                               FirstOrDefault();
                }
                return oSH_Year;
            }
        }

        private string oSH_BalanceSheetFile;
        public string SH_BalanceSheetFile
        {
            get
            {
                if (RelatedBalanceSheetInfo != null && string.IsNullOrEmpty(oSH_BalanceSheetFile))
                {
                    oSH_BalanceSheetFile = RelatedBalanceSheetInfo.ItemInfo.
                               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.SH_BalanceSheetFile).
                               Select(y => y.Value).
                               DefaultIfEmpty(string.Empty).
                               FirstOrDefault();
                }
                return oSH_BalanceSheetFile;
            }
        }

        private string oSH_Currency;
        public string SH_Currency
        {
            get
            {
                if (RelatedBalanceSheetInfo != null && string.IsNullOrEmpty(oSH_Currency))
                {
                    oSH_Currency = RelatedBalanceSheetInfo.ItemInfo.
                               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.SH_Currency).
                               Select(y => y.Value).
                               DefaultIfEmpty(string.Empty).
                               FirstOrDefault();
                }
                return oSH_Currency;
            }
        }

        private bool? oSH_HasValues;
        public bool SH_HasValues
        {
            get
            {
                if (oSH_HasValues == null)
                {
                    oSH_HasValues = (RelatedBalanceSheetInfo != null &&
                        RelatedBalanceSheetInfo.BalanceSheetInfo != null &&
                        RelatedBalanceSheetInfo.BalanceSheetInfo.Count > 0);
                }
                return oSH_HasValues.Value;
            }
        }

        #endregion

        #region Taxes

        public string TX_Year { get; set; }
        public string TX_TaxFile { get; set; }

        #endregion

        #region IncomeStatement

        public string IS_Year { get; set; }
        public string IS_GrossIncome { get; set; }
        public string IS_NetIncome { get; set; }
        public string IS_GrossEstate { get; set; }
        public string IS_LiquidHeritage { get; set; }
        public string IS_FileIncomeStatement { get; set; }

        #endregion

        #region Bank Info

        public string IB_Bank { get; set; }
        public string IB_BankName { get; set; }
        public string IB_AccountType { get; set; }
        public string IB_AccountNumber { get; set; }
        public string IB_AccountHolder { get; set; }
        public string IB_ABA { get; set; }
        public string IB_Swift { get; set; }
        public string IB_IBAN { get; set; }
        public string IB_Customer { get; set; }
        public string IB_AccountFile { get; set; }

        #endregion

        public ProviderFinancialViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedFinancial,
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oBank,
                List<CatalogModel> oOptions)
        {
            #region TAX
            TX_Year = RelatedFinancial.ItemInfo.
                   Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.TX_Year).
                   Select(y => y.Value).
                   DefaultIfEmpty(string.Empty).
                   FirstOrDefault();

            TX_TaxFile = RelatedFinancial.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.TX_TaxFile).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();
            #endregion

            #region IncomeStatement
            IS_Year = RelatedFinancial.ItemInfo.
                   Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_Year).
                   Select(y => y.Value).
                   DefaultIfEmpty(string.Empty).
                   FirstOrDefault();

            IS_GrossIncome = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_GrossIncome).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_NetIncome = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_NetIncome).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_GrossEstate = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_GrossEstate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_LiquidHeritage = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_LiquidHeritage).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_FileIncomeStatement = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_FileIncomeStatement).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region Bank Info

            IB_Bank = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_Bank).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            if (oBank != null && oBank.Count > 0)
            {
                IB_BankName = oBank.
                    Where(y => y.ItemId.ToString() == IB_Bank).
                    Select(y => y.ItemName).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }

            IB_AccountType = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountType = !string.IsNullOrEmpty(IB_AccountType) && oOptions != null && oOptions.Count > 0 ?
              oOptions.Where(x => x.ItemId.ToString() == IB_AccountType).Select(x => x.ItemName).FirstOrDefault() : "N/A";

            IB_AccountNumber = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountNumber).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountHolder = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountHolder).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_ABA = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_ABA).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_Swift = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_Swift).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_IBAN = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_IBAN).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_Customer = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_Customer).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountFile = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
            #endregion
        }

        public ProviderFinancialViewModel(ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel oRelatedBalanceSheetInfo)
        {
            RelatedBalanceSheetInfo = oRelatedBalanceSheetInfo;
        }


        public ProviderFinancialViewModel() { }
    }
}
