﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderFinancialBasicInfoViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedFinancialBasicInfo { get; private set; }

        #region Financial BasicInfo

        public decimal Exchange { get; set; }

        public string oBI_TotalActive { get; set; }
        public string BI_TotalActive
        {            
            get
            {
                if (string.IsNullOrEmpty(oBI_TotalActive))
                {
                    oBI_TotalActive = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_TotalActive).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(oBI_TotalActive))
                    {
                        oBI_TotalActive = (Convert.ToDecimal(oBI_TotalActive) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_TotalActive;
            }
        }

        public string oBI_TotalPassive { get; set; }
        public string BI_TotalPassive
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_TotalPassive))
                {
                    oBI_TotalPassive = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_TotalPassive).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(oBI_TotalPassive))
                    {
                        oBI_TotalPassive = (Convert.ToDecimal(oBI_TotalPassive) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_TotalPassive;
            }
        }

        public string oBI_TotalPatrimony { get; set; }
        public string BI_TotalPatrimony
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_TotalPatrimony))
                {
                    oBI_TotalPatrimony = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_TotalPatrimony).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(oBI_TotalPatrimony))
                    {
                        oBI_TotalPatrimony = (Convert.ToDecimal(oBI_TotalPatrimony) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_TotalPatrimony;
            }
        }

        public string oBI_OperationIncome { get; set; }
        public string BI_OperationIncome
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_OperationIncome))
                {
                    oBI_OperationIncome = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_OperationIncome).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(oBI_OperationIncome))
                    {
                        oBI_OperationIncome = (Convert.ToDecimal(oBI_OperationIncome) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_OperationIncome;
            }
        }

        public string oBI_IncomeBeforeTaxes { get; set; }
        public string BI_IncomeBeforeTaxes
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_IncomeBeforeTaxes))
                {
                    oBI_IncomeBeforeTaxes = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_IncomeBeforeTaxes).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(oBI_IncomeBeforeTaxes))
                    {
                        oBI_IncomeBeforeTaxes = (Convert.ToDecimal(oBI_IncomeBeforeTaxes) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_IncomeBeforeTaxes;
            }
        }

        public string oBI_CurrentActive { get; set; }
        public string BI_CurrentActive
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_CurrentActive))
                {
                    oBI_CurrentActive = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_CurrentActive).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(oBI_CurrentActive))
                    {
                        oBI_CurrentActive = (Convert.ToDecimal(oBI_CurrentActive) * Exchange).ToString("#,0.##");
                    }                    
                }

                return oBI_CurrentActive;
            }
        }

        public string oBI_CurrentPassive { get; set; }
        public string BI_CurrentPassive
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_CurrentPassive))
                {
                    oBI_CurrentPassive = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_CurrentPassive).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(oBI_CurrentPassive))
                    {
                        oBI_CurrentPassive = (Convert.ToDecimal(oBI_CurrentPassive) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_CurrentPassive;
            }
        }

        public string oBI_Altman { get; set; }
        public string BI_Altman
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_Altman))
                {
                    oBI_Altman = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_Altman).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oBI_Altman;
            }
        }

        public string oBI_ExerciseUtility { get; set; }
        public string BI_ExerciseUtility
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_ExerciseUtility))
                {
                    oBI_ExerciseUtility = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_ExerciseUtility).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(oBI_ExerciseUtility))
                    {
                        oBI_ExerciseUtility = (Convert.ToDecimal(oBI_ExerciseUtility) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_ExerciseUtility;
            }
        }

        public string oBI_EBITDA { get; set; }
        public string BI_EBITDA
        {
            get
            {
                if (string.IsNullOrEmpty(oBI_EBITDA))
                {
                    oBI_EBITDA = RelatedFinancialBasicInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumFinancialInfoType.BI_EBITDA).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(oBI_EBITDA))
                    {
                        oBI_EBITDA = (Convert.ToDecimal(oBI_EBITDA) * Exchange).ToString("#,0.##");
                    }
                }

                return oBI_EBITDA;
            }
        }

        public string BI_JobCapital { get; set; }
        public string BI_Year { get; set; }

        public string oCurrency { get; set; }
        public string Currency
        {
            get
            {
                if (string.IsNullOrEmpty(oCurrency))
                {
                    oCurrency = MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_CurrencyExchange_COP].Value);
                }

                return oCurrency;
            }
        }

        #endregion

        public ProviderFinancialBasicInfoViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedInfo, decimal oExchange)
        {
            RelatedFinancialBasicInfo = oRelatedInfo;
            Exchange = oExchange;
            BI_Year = oRelatedInfo.ItemName;            
        }

        public ProviderFinancialBasicInfoViewModel() { }
    }
}
