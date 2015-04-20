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

        private string oTX_Year;
        public string TX_Year
        {
            get
            {
                if (string.IsNullOrEmpty(oTX_Year))
                {
                    oTX_Year = RelatedFinancialInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.TX_Year).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }

                return oTX_Year;
            }
        }

        private string oTX_TaxFile;
        public string TX_TaxFile
        {
            get
            {
                if (string.IsNullOrEmpty(oTX_TaxFile))
                {
                    oTX_TaxFile = RelatedFinancialInfo.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.TX_TaxFile).
                       Select(y => y.Value).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault();
                }

                return oTX_TaxFile;
            }
        }

        #endregion

        #region IncomeStatement

        private string oIS_Year;
        public string IS_Year
        {
            get
            {
                if (string.IsNullOrEmpty(oIS_Year))
                {
                    oIS_Year = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_Year).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIS_Year;
            }
        }

        private string oIS_GrossIncome;
        public string IS_GrossIncome
        {
            get
            {
                if (string.IsNullOrEmpty(oIS_GrossIncome))
                {
                    oIS_GrossIncome = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_GrossIncome).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIS_GrossIncome;
            }
        }

        private string oIS_NetIncome;
        public string IS_NetIncome
        {
            get
            {
                if (string.IsNullOrEmpty(oIS_NetIncome))
                {
                    oIS_NetIncome = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_NetIncome).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIS_NetIncome;
            }
        }

        private string oIS_GrossEstate;
        public string IS_GrossEstate
        {
            get
            {
                if (string.IsNullOrEmpty(oIS_GrossEstate))
                {
                    oIS_GrossEstate = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_GrossEstate).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIS_GrossEstate;
            }
        }

        private string oIS_LiquidHeritage;
        public string IS_LiquidHeritage
        {
            get
            {
                if (string.IsNullOrEmpty(oIS_LiquidHeritage))
                {
                    oIS_LiquidHeritage = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_LiquidHeritage).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIS_LiquidHeritage;
            }
        }

        private string oIS_FileIncomeStatement;
        public string IS_FileIncomeStatement
        {
            get
            {
                if (string.IsNullOrEmpty(oIS_FileIncomeStatement))
                {
                    oIS_FileIncomeStatement = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IS_FileIncomeStatement).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIS_FileIncomeStatement;
            }
        }

        #endregion

        #region Bank Info

        private ProveedoresOnLine.Company.Models.Util.GenericItemModel oIB_Bank;
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel IB_Bank
        {
            get
            {
                if (oIB_Bank == null)
                {
                    oIB_Bank = MarketPlace.Models.Company.CompanyUtil.Bank.
                        Where(x => x.ItemId.ToString() == RelatedFinancialInfo.ItemInfo.
                                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_Bank).
                                        Select(y => y.Value).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault()).
                        FirstOrDefault();
                }
                return oIB_Bank;
            }
        }

        private string oIB_AccountType;
        public string IB_AccountType
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_AccountType))
                {
                    oIB_AccountType = RelatedFinancialInfo.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountType).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();

                    if (!string.IsNullOrEmpty(oIB_AccountType))
                    {
                        oIB_AccountType = MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where
                            (x => x.CatalogId == 1001 && x.ItemId.ToString() == oIB_AccountType).
                            Select(x => x.ItemName).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
                    }
                }

                return oIB_AccountType;
            }
        }

        private string oIB_AccountNumber;
        public string IB_AccountNumber
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_AccountNumber))
                {
                    oIB_AccountNumber = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountNumber).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIB_AccountNumber;
            }
        }

        private string oIB_AccountHolder;
        public string IB_AccountHolder
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_AccountHolder))
                {
                    oIB_AccountHolder = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountHolder).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIB_AccountHolder;
            }
        }

        private string oIB_ABA;
        public string IB_ABA
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_ABA))
                {
                    oIB_ABA = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_ABA).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIB_ABA;
            }
        }

        private string oIB_Swift;
        public string IB_Swift
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_Swift))
                {
                    oIB_Swift = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_Swift).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIB_Swift;
            }
        }

        private string oIB_IBAN;
        public string IB_IBAN
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_IBAN))
                {
                    oIB_IBAN = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_IBAN).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIB_IBAN;
            }
        }

        private string oIB_Customer;
        public string IB_Customer
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_Customer))
                {
                    oIB_Customer = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_Customer).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIB_Customer;
            }
        }

        private string oIB_AccountFile;
        public string IB_AccountFile
        {
            get
            {
                if (string.IsNullOrEmpty(oIB_AccountFile))
                {
                    oIB_AccountFile = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.IB_AccountFile).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oIB_AccountFile;
            }
        }

        #endregion

        #region K_Contract Info

        private string oFK_RoleType;
        public string FK_RoleType
        {
            get
            {
                if (oFK_RoleType == null)
                {
                    oFK_RoleType = MarketPlace.Models.Company.CompanyUtil.ProviderOptions.
                        Where(x =>  x.CatalogId == 117 && 
                                    x.ItemId.ToString() == RelatedFinancialInfo.ItemInfo.
                                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.FK_RoleType).
                                        Select(y => y.Value).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault()).
                        Select(x => x.ItemName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oFK_RoleType;
            }
        }

        private string oFK_TotalTechnicScore;
        public string FK_TotalTechnicScore
        {
            get
            {
                if (string.IsNullOrEmpty(oFK_TotalTechnicScore))
                {
                    oFK_TotalTechnicScore = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.FK_TotalTechnicScore).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oFK_TotalTechnicScore;
            }
        }

        private string oFK_TotalExpirienceScore;
        public string FK_TotalExpirienceScore
        {
            get
            {
                if (string.IsNullOrEmpty(oFK_TotalExpirienceScore))
                {
                    oFK_TotalExpirienceScore = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.FK_TotalExpirienceScore).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oFK_TotalExpirienceScore;
            }
        }

        private string oFK_TotalFinancialScore;
        public string FK_TotalFinancialScore
        {
            get
            {
                if (string.IsNullOrEmpty(oFK_TotalFinancialScore))
                {
                    oFK_TotalFinancialScore = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.FK_TotalFinancialScore).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oFK_TotalFinancialScore;
            }
        }

        private string oFK_TotalScore;
        public string FK_TotalScore
        {
            get
            {
                if (string.IsNullOrEmpty(oFK_TotalScore))
                {
                    oFK_TotalScore = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.FK_TotalScore).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oFK_TotalScore;
            }
        }

        private string oFK_TotalOrgCapacityScore;
        public string FK_TotalOrgCapacityScore
        {
            get
            {
                if (string.IsNullOrEmpty(oFK_TotalOrgCapacityScore))
                {
                    oFK_TotalOrgCapacityScore = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.FK_TotalOrgCapacityScore).
                        Select(y => Convert.ToDecimal(y.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
                        DefaultIfEmpty(0).
                        FirstOrDefault().ToString("0.##");
                }
                return oFK_TotalOrgCapacityScore;
            }
        }

        private string oFK_TotalKValueScore;
        public string FK_TotalKValueScore
        {
            get
            {
                if (string.IsNullOrEmpty(oFK_TotalKValueScore))
                {
                    oFK_TotalKValueScore = RelatedFinancialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.FK_TotalKValueScore).
                        Select(y => Convert.ToDecimal(y.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
                        DefaultIfEmpty(0).
                        FirstOrDefault().ToString("0.##");
                }
                return oFK_TotalKValueScore;
            }
        }

        #endregion

        public ProviderFinancialViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedFinancial)
        {
            RelatedFinancialInfo = RelatedFinancial;
        }

        public ProviderFinancialViewModel(ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel oRelatedBalanceSheetInfo)
        {
            RelatedBalanceSheetInfo = oRelatedBalanceSheetInfo;
        }

        public ProviderFinancialViewModel() { }
    }
}
