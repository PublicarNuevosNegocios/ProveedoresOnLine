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

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedContactInfo { get; set; }

        #region Experience

        public string EX_ContractType { get; set; }
        public string EX_Currency { get; set; }
        public string EX_DateIssue { get; set; }
        public string EX_DueDate { get; set; }
        public string EX_Client { get; set; }
        public string EX_ContractNumber { get; set; }
        public string EX_ContractValue { get; set; }
        public string EX_Phone { get; set; }
        public string EX_ExperienceFile { get; set; }
        public string EX_ContractSubject { get; set; }
        public List<MarketPlace.Models.General.EconomicActivityViewModel> EX_EconomicActivity { get; set; }
        public List<MarketPlace.Models.General.EconomicActivityViewModel> EX_CustomEconomicActivity { get; set; }

        #endregion


        public ProviderComercialViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedInfo,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivity,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCustomActivity)
        {
            RelatedContactInfo = oRelatedInfo;

            #region Experience
            EX_ContractType = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Currency = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_Currency).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DateIssue = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_DateIssue).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DueDate = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_DueDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Client = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_Client).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractNumber = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractNumber).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractValue = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractValue).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Phone = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_Phone).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ExperienceFile = oRelatedInfo.ItemInfo.
              Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ExperienceFile).
              Select(y => y.Value).
              DefaultIfEmpty(string.Empty).
              FirstOrDefault();

            EX_ContractSubject = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_ContractSubject).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_EconomicActivity = oRelatedInfo.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_EconomicActivity).
                Select(y => y.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault().
                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                Select(y => new MarketPlace.Models.General.EconomicActivityViewModel()
                {
                    EconomicActivityId = y,
                    ActivityName = oActivity.
                        Where(z => z.ItemId.ToString() == y).
                        Select(z => z.ItemName).
                        DefaultIfEmpty(string.Empty).FirstOrDefault()
                }).ToList();

            EX_CustomEconomicActivity = oRelatedInfo.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCommercialInfoType.EX_CustomEconomicActivity).
               Select(y => y.LargeValue).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault().
               Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
               Select(y => new MarketPlace.Models.General.EconomicActivityViewModel()
               {
                   EconomicActivityId = y,
                   ActivityName = oCustomActivity.
                       Where(z => z.ItemId.ToString() == y).
                       Select(z => z.ItemName).
                       DefaultIfEmpty(string.Empty).FirstOrDefault()
               }).ToList();
            #endregion
        }
        public ProviderComercialViewModel() { }
    }
}
