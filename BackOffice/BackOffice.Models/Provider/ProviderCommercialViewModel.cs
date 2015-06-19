using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderCommercialViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedCommercial { get; private set; }

        public string CommercialId { get; set; }

        public string CommercialName { get; set; }

        public bool Enable { get; set; }

        #region Experience

        public string EX_ContractType { get; set; }
        public string EX_ContractTypeId { get; set; }

        public string EX_Currency { get; set; }
        public string EX_CurrencyId { get; set; }

        public string EX_DateIssue { get; set; }
        public string EX_DateIssueId { get; set; }

        public string EX_DueDate { get; set; }
        public string EX_DueDateId { get; set; }

        public string EX_Client { get; set; }
        public string EX_ClientId { get; set; }

        public string EX_ContractNumber { get; set; }
        public string EX_ContractNumberId { get; set; }

        public string EX_ContractValue { get; set; }
        public string EX_ContractValueId { get; set; }

        public string EX_Phone { get; set; }
        public string EX_PhoneId { get; set; }

        public string EX_ExperienceFile { get; set; }
        public string EX_ExperienceFileId { get; set; }

        public string EX_ContractSubject { get; set; }
        public string EX_ContractSubjectId { get; set; }

        public List<BackOffice.Models.General.EconomicActivityViewModel> EX_EconomicActivity { get; set; }
        public string EX_EconomicActivityId { get; set; }

        public List<BackOffice.Models.General.EconomicActivityViewModel> EX_CustomEconomicActivity { get; set; }
        public string EX_CustomEconomicActivityId { get; set; }

        #endregion

        public ProviderCommercialViewModel() { }

        public ProviderCommercialViewModel
                    (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedCommercial)
        {
            RelatedCommercial = oRelatedCommercial;

            CommercialId = RelatedCommercial.ItemId.ToString();

            CommercialName = RelatedCommercial.ItemName;

            Enable = RelatedCommercial.Enable;

            #region Experience

            EX_ContractType = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractTypeId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractType).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Currency = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_Currency).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_CurrencyId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_Currency).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DateIssue = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_DateIssue).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DateIssueId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_DateIssue).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DueDate = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_DueDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DueDateId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_DueDate).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Client = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_Client).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ClientId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_Client).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractNumber = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractNumber).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractNumberId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractNumber).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractValue = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractValue).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractValueId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractValue).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Phone = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_Phone).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_PhoneId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_Phone).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ExperienceFile = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ExperienceFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ExperienceFileId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ExperienceFile).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractSubject = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractSubject).
                Select(y => y.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractSubjectId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_ContractSubject).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_EconomicActivity = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_EconomicActivity).
                Select(y => y.ValueName).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault().
                Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                Where(y => y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length >= 2).
                Select(y => new BackOffice.Models.General.EconomicActivityViewModel()
                {
                    EconomicActivityId = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0],
                    ActivityName = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1],
                }).ToList();

            EX_EconomicActivityId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_EconomicActivity).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_CustomEconomicActivity = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_CustomEconomicActivity).
                Select(y => y.ValueName).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault().
                Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                Where(y => y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length >= 2).
                Select(y => new BackOffice.Models.General.EconomicActivityViewModel()
                {
                    EconomicActivityId = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0],
                    ActivityName = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1],
                }).ToList();

            EX_CustomEconomicActivityId = RelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCommercialInfoType.EX_CustomEconomicActivity).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            #endregion

        }
    }
}
