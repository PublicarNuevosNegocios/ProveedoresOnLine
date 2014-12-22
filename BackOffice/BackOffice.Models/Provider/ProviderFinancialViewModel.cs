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

        #region IncomeStatement

        public string IS_Year { get; set; }
        public string IS_YearId { get; set; }

        public string IS_GrossIncome { get; set; }
        public string IS_GrossIncomeId { get; set; }

        public string IS_NetIncome { get; set; }
        public string IS_NetIncomeId { get; set; }

        public string IS_GrossEstate { get; set; }
        public string IS_GrossEstateId { get; set; }

        public string IS_LiquidHeritage { get; set; }
        public string IS_LiquidHeritageId { get; set; }

        public string IS_FileIncomeStatement { get; set; }
        public string IS_FileIncomeStatementId { get; set; }

        #endregion

        #region Bank Info

        public string IB_Bank { get; set; }
        public string IB_BankId { get; set; }
        public string IB_BankName { get; set; }

        public string IB_AccountType { get; set; }
        public string IB_AccountTypeId { get; set; }

        public string IB_AccountNumber { get; set; }
        public string IB_AccountNumberId { get; set; }

        public string IB_AccountHolder { get; set; }
        public string IB_AccountHolderId { get; set; }

        public string IB_ABA { get; set; }
        public string IB_ABAId { get; set; }

        public string IB_Swift { get; set; }
        public string IB_SwiftId { get; set; }

        public string IB_IBAN { get; set; }
        public string IB_IBANId { get; set; }

        public string IB_Customer { get; set; }
        public string IB_CustomerId { get; set; }

        public string IB_AccountFile { get; set; }
        public string IB_AccountFileId { get; set; }

        #endregion

        public ProviderFinancialViewModel() { }

        public ProviderFinancialViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedFinancial,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oBank)
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

            #region Income Statement

            IS_Year = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_Year).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_YearId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_Year).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_GrossIncome = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_GrossIncome).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_GrossIncomeId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_GrossIncome).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_NetIncome = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_NetIncome).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_NetIncomeId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_NetIncome).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_GrossEstate = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_GrossEstate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_GrossEstateId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_GrossEstate).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_LiquidHeritage = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_LiquidHeritage).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_LiquidHeritageId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_LiquidHeritage).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_FileIncomeStatement = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_FileIncomeStatement).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IS_FileIncomeStatementId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IS_FileIncomeStatement).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

            #region Bank Info

            IB_Bank = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_Bank).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_BankId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_Bank).
                Select(y => y.ItemInfoId.ToString()).
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
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountTypeId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountType).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountNumber = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountNumber).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountNumberId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountNumber).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountHolder = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountHolder).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountHolderId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountHolder).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_ABA = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_ABA).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_ABAId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_ABA).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_Swift = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_Swift).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_SwiftId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_Swift).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_IBAN = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_IBAN).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_IBANId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_IBAN).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_Customer = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_Customer).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_CustomerId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_Customer).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountFile = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            IB_AccountFileId = RelatedFinancial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumFinancialInfoType.IB_AccountFile).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

        }
    }
}
