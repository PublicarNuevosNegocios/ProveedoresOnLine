using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderComercialViewModel
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

        public string EX_BuiltArea { get; set; }
        public string EX_BuiltAreaId { get; set; }

        public string EX_BuiltUnit { get; set; }
        public string EX_BuiltUnitId { get; set; }

        public string EX_ExperienceFile { get; set; }
        public string EX_ExperienceFileId { get; set; }

        public string EX_ContractSubject { get; set; }
        public string EX_ContractSubjectId { get; set; }

        /// <summary>
        /// ItemInfoId,Value - CategoryId, Category Name
        /// </summary>
        public List<Tuple<string, string, string>> EX_EconomicActivity { get; set; }

        /// <summary>
        /// ItemInfoId,Value - CategoryId, Category Name
        /// </summary>
        public List<Tuple<string, string, string>> EX_CustomEconomicActivity { get; set; }

        #endregion

        public ProviderComercialViewModel() { }

        public ProviderComercialViewModel
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedCommercial,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivity,
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCustomActivity)
        {
            RelatedCommercial = oRelatedCommercial;

            CommercialId = RelatedCommercial.ItemId.ToString();

            CommercialName = RelatedCommercial.ItemName;

            Enable = RelatedCommercial.Enable;

            #region Experience

            EX_ContractType = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractType).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractTypeId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractType).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Currency = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_Currency).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_CurrencyId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_Currency).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DateIssue = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_DateIssue).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DateIssueId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_DateIssue).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DueDate = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_DueDate).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_DueDateId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_DueDate).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Client = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_Client).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ClientId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_Client).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractNumber = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractNumber).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractNumberId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractNumber).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractValue = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractValue).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractValueId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractValue).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_Phone = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_Phone).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_PhoneId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_Phone).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_BuiltArea = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_BuiltArea).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_BuiltAreaId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_BuiltArea).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_BuiltUnit = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_BuiltUnit).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_BuiltUnitId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_BuiltUnit).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ExperienceFile = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ExperienceFile).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ExperienceFileId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ExperienceFile).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractSubject = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractSubject).
                Select(y => y.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_ContractSubjectId = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_ContractSubject).
                Select(y => y.ItemInfoId.ToString()).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            EX_EconomicActivity = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_EconomicActivity).
                Select(y => new Tuple<string, string, string>
                    (y.ItemInfoId.ToString(),
                    y.Value,
                    y.ValueName)).
                 ToList();

            EX_CustomEconomicActivity = oRelatedCommercial.ItemInfo.
                Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumExperienceInfoType.EX_CustomEconomicActivity).
                Select(y => new Tuple<string, string, string>
                    (y.ItemInfoId.ToString(),
                    y.Value,
                    y.ValueName)).
                 ToList();

            #endregion

        }
    }
}
