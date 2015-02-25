using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderFinancialBasicInfoViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedFinancialBasicInfo { get; set; }
        
        #region Financial BasicInfo

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
        
        #endregion

        public ProviderFinancialBasicInfoViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedInfo)
        {
            RelatedFinancialBasicInfo = oRelatedInfo;
        }

        public ProviderFinancialBasicInfoViewModel() { }
    }
}
