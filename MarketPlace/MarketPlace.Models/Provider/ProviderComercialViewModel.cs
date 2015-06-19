using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderComercialViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedCommercialInfo { get; set; }

        #region Experience

        private string oEX_ContractType;
        public string EX_ContractType
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_ContractType))
                {
                    oEX_ContractType = MarketPlace.Models.Company.CompanyUtil.GetProviderOptionName(
                        RelatedCommercialInfo.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractType).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                }
                return oEX_ContractType;
            }
        }

        private string oEX_Currency;
        public string EX_Currency
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_Currency))
                {
                    oEX_Currency = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_Currency).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    if (!string.IsNullOrEmpty(oEX_Currency))
                    {
                        oEX_Currency = MarketPlace.Models.Company.CompanyUtil.ProviderOptions.Where
                            (x => x.CatalogId == 108 && x.ItemId.ToString() == oEX_Currency).
                            Select(x => x.ItemName).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
                    }
                }
                return oEX_Currency;
            }
        }

        private string oEX_DateIssue;
        public string EX_DateIssue
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_DateIssue))
                {
                    oEX_DateIssue = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_DateIssue).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oEX_DateIssue;
            }
        }

        private string oEX_DueDate;
        public string EX_DueDate
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_DueDate))
                {
                    oEX_DueDate = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_DueDate).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oEX_DueDate;
            }
        }

        private string oEX_Client;
        public string EX_Client
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_Client))
                {
                    oEX_Client = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_Client).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oEX_Client;
            }
        }

        private string oEX_ContractNumber;
        public string EX_ContractNumber
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_ContractNumber))
                {
                    oEX_ContractNumber = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractNumber).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oEX_ContractNumber;
            }
        }

        private string oEX_ContractValue;
        public string EX_ContractValue
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_ContractValue))
                {
                    oEX_ContractValue = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractValue).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    if (!string.IsNullOrEmpty(oEX_ContractValue))
                    {
                        oEX_ContractValue = Convert.ToDecimal(oEX_ContractValue).ToString("#,0.##", System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"));
                    }
                    else
                    {
                        oEX_ContractValue = "0";
                    }
                }
                return oEX_ContractValue;
            }
        }

        private string oEX_Phone;
        public string EX_Phone
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_Phone))
                {
                    oEX_Phone = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_Phone).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oEX_Phone;
            }
        }

        private string oEX_ExperienceFile;
        public string EX_ExperienceFile
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_ExperienceFile))
                {
                    oEX_ExperienceFile = RelatedCommercialInfo.ItemInfo.
                      Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ExperienceFile).
                      Select(y => y.Value).
                      DefaultIfEmpty(string.Empty).
                      FirstOrDefault();
                }
                return oEX_ExperienceFile;
            }
        }

        private string oEX_ContractSubject;
        public string EX_ContractSubject
        {
            get
            {
                if (string.IsNullOrEmpty(oEX_ContractSubject))
                {
                    oEX_ContractSubject = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractSubject).
                        Select(y => y.LargeValue).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oEX_ContractSubject;
            }
        }

        private List<MarketPlace.Models.General.EconomicActivityViewModel> oEX_EconomicActivity;
        public List<MarketPlace.Models.General.EconomicActivityViewModel> EX_EconomicActivity
        {
            get
            {
                if (oEX_EconomicActivity == null)
                {
                    oEX_EconomicActivity = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_EconomicActivity).
                        Select(y => y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault().
                        Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                        Where(y => y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length >= 2).
                        Select(y => new MarketPlace.Models.General.EconomicActivityViewModel()
                        {
                            EconomicActivityId = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            ActivityName = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1],
                        }).ToList();
                }
                return oEX_EconomicActivity;
            }
            set { oEX_EconomicActivity = value; }
        }

        private List<MarketPlace.Models.General.EconomicActivityViewModel> oEX_CustomEconomicActivity;
        public List<MarketPlace.Models.General.EconomicActivityViewModel> EX_CustomEconomicActivity
        {
            get
            {
                if (oEX_CustomEconomicActivity == null)
                {
                    oEX_CustomEconomicActivity = RelatedCommercialInfo.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_CustomEconomicActivity).
                        Select(y => y.ValueName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault().
                        Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                        Where(y => y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length >= 2).
                        Select(y => new MarketPlace.Models.General.EconomicActivityViewModel()
                        {
                            EconomicActivityId = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            ActivityName = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1],
                        }).ToList();
                }
                return oEX_CustomEconomicActivity;
            }
            set { oEX_CustomEconomicActivity = value; }
        }

        #endregion

        public ProviderComercialViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedInfo)
        {
            RelatedCommercialInfo = oRelatedInfo;
        }

        public ProviderComercialViewModel() { }
    }
}
