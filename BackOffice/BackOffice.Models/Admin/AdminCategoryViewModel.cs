
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminCategoryViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLegal { get; private set; }

        public bool Enable { get; set; }

        #region Geolocalization

        public string GIT_Country { get; set; }
        public string GIT_CountryId { get; set; }

        public string GIT_CountryDirespCode { get; set; }
        public string GIT_CountryDirespCodeId { get; set; }

        public string GIT_CountryType { get; set; }
        public string GIT_CountryTypeId { get; set; }

        public bool GIT_CountryEnable { get; set; }

        public string GIT_StateId { get; set; }
        public string GIT_State { get; set; }

        public string GIT_StateDirespCode { get; set; }
        public string GIT_StateDirespCodeId { get; set; }

        public bool GIT_StateEnable { get; set; }

        public string AG_City { get; set; }
        public string AG_CityId { get; set; }

        public string GI_CapitalType { get; set; }
        public string GI_CapitalTypeId { get; set; }

        public string GI_CityDirespCode { get; set; }
        public string GI_CityDirespCodeId { get; set; }

        public bool GI_CityEnable { get; set; }

        public int AllTotalRows { get; set; }
        #endregion

        #region Banks

        public string B_Bank { get; set; }
        public string B_BankId { get; set; }
        public bool B_BankEnable { get; set; }

        public string B_CityId { get; set; }
        public string B_City { get; set; }

        #endregion

        #region CompanyRules

        public string CR_CompanyRule { get; set; }
        public string CR_CompanyRuleId { get; set; }
        public bool CR_CompanyRuleEnable { get; set; }

        #endregion

        #region Rules

        public string R_Rule { get; set; }
        public string R_RuleId { get; set; }
        public bool R_RuleEnable { get; set; }

        #endregion

        #region Resolution

        public string RS_Resolution { get; set; }
        public string RS_ResolutionId { get; set; }
        public bool RS_ResolutionEnable { get; set; }

        #endregion

        #region StandarCustomerActivity

        public string ECS_EconomyActivityId { get; set; }
        public string ECS_EconomyActivity { get; set; }

        public string ECS_TypeId { get; set; }
        public string ECS_Type { get; set; }

        public string ECS_CategoryId { get; set; }
        public string ECS_Category { get; set; }

        public string ECS_GroupId { get; set; }
        public string ECS_Group { get; set; }
        public string ECS_GroupName { get; set; }

        public bool ECS_Enable { get; set; }

        #endregion

        #region Group

        public string G_Group { get; set; }
        public string G_GroupId { get; set; }
        public bool G_GroupEnable { get; set; }

        #endregion

        #region Current Exchange

        public string C_CurrentExchangeId { get; set; }
        public string C_IssueDate { get; set; }
        public string C_MoneyTypeFromId { get; set; }
        public string C_MoneyTypeFromName { get; set; }
        public string C_MoneyTypeToId { get; set; }
        public string C_MoneyTypeToName { get; set; }
        public string C_Rate { get; set; }
        public string C_CreateDate { get; set; }
        public string C_LastModify { get; set; }
        
        #endregion

        #region Tree

        public string T_TreeId { get; set; }
        public string T_TreeName { get; set; }
        public bool T_TreeEnable { get; set; }
        
	    #endregion

        public AdminCategoryViewModel() { }

        public AdminCategoryViewModel(ProveedoresOnLine.Company.Models.Util.GeographyModel oRelatedGeoGraphy)
        {
            #region Geolocalization

            #region Country
            GIT_Country = oRelatedGeoGraphy.Country.ItemName;
            GIT_CountryId = oRelatedGeoGraphy.Country.ItemId.ToString();

            GIT_CountryDirespCode = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.Value).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GIT_CountryDirespCodeId = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_CountryType = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_GeographyType).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_CountryTypeId = oRelatedGeoGraphy.Country.ItemInfo != null ? oRelatedGeoGraphy.Country.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_GeographyType).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_CountryEnable = oRelatedGeoGraphy.Country.Enable;

            #endregion

            #region State
            GIT_State = oRelatedGeoGraphy.State.ItemName;
            GIT_StateId = oRelatedGeoGraphy.State.ItemId.ToString();

            GIT_StateDirespCode = oRelatedGeoGraphy.State.ItemInfo != null ? oRelatedGeoGraphy.State.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_StateDirespCodeId = oRelatedGeoGraphy.State.ItemInfo != null ? oRelatedGeoGraphy.State.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            GIT_StateEnable = oRelatedGeoGraphy.State.Enable;
            #endregion

            #region City
            AG_City = oRelatedGeoGraphy.City.ItemName;
            AG_CityId = oRelatedGeoGraphy.City.ItemId.ToString();

            GI_CapitalType = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_CapitalType).
                             Select(y => y.Value).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_CapitalTypeId = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_CapitalType).
                             Select(y => y.ItemInfoId.ToString()).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_CityDirespCode = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.Value).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_CityDirespCodeId = oRelatedGeoGraphy.City.ItemInfo != null ? oRelatedGeoGraphy.City.ItemInfo.
                             Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.GI_DirespCode).
                             Select(y => y.ItemInfoId.ToString()).
                             DefaultIfEmpty(string.Empty).
                             FirstOrDefault() : string.Empty;

            GI_CityEnable = oRelatedGeoGraphy.City.Enable;

            #endregion

            #endregion
        }

        public AdminCategoryViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oCategory)
        {
            #region Banks

            B_Bank = oCategory.ItemName;
            B_BankId = oCategory.ItemId.ToString();
            B_BankEnable = oCategory.Enable;

            B_City = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId != null).
                            Select(y => !string.IsNullOrEmpty(y.Value) ? y.Value.ToString() : string.Empty).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            B_CityId = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                            Where(y => y.ItemInfoType.ItemId != null).
                            Select(y => y.ItemInfoId.ToString()).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault() : string.Empty;

            #endregion

            #region CompanyRules

            CR_CompanyRule = oCategory.ItemName;
            CR_CompanyRuleId = oCategory.ItemId.ToString();
            CR_CompanyRuleEnable = oCategory.Enable;

            #endregion

            #region Rules

            R_Rule = oCategory.ItemName;
            R_RuleId = oCategory.ItemId.ToString();
            R_RuleEnable = oCategory.Enable;

            #endregion

            #region Resolutions

            RS_Resolution = oCategory.ItemName;
            RS_ResolutionId = oCategory.ItemId.ToString();
            RS_ResolutionEnable = oCategory.Enable;

            #endregion

            #region StandarCustomerActivity

            ECS_EconomyActivityId = oCategory.ItemId.ToString();
            ECS_EconomyActivity = oCategory.ItemName;
            

            ECS_TypeId = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Type).
                        Select(y => y.ItemInfoId.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault() : string.Empty;

            ECS_Type = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                       Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Type).
                       Select(y => y.ItemInfoType.ItemName).
                       DefaultIfEmpty(string.Empty).
                       FirstOrDefault() : string.Empty;

            ECS_CategoryId = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Category).
                        Select(y => y.ItemInfoId.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault() : string.Empty;

            ECS_Category = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Category).
                        Select(y => y.ItemInfoType.ItemName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault() : string.Empty;

            ECS_GroupId = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Group).
                        Select(y => y.ItemInfoId.ToString()).
                         DefaultIfEmpty(string.Empty).
                        FirstOrDefault() : string.Empty;

            ECS_Group = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Group).
                        Select(y => y.ItemInfoType.ItemName).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault() : string.Empty;

            ECS_GroupName = oCategory.ItemInfo != null ? oCategory.ItemInfo.
                        Where(y => y.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCategoryInfoType.EA_Group).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault() : string.Empty;

            ECS_Enable = oCategory.Enable;

            #endregion

            #region Standar Group

            G_Group = oCategory.ItemName;
            G_GroupId = oCategory.ItemId.ToString();
            G_GroupEnable = oCategory.Enable;

            #endregion

            #region Tree

            T_TreeId = oCategory.ItemId.ToString();
            T_TreeName = oCategory.ItemName;
            T_TreeEnable = oCategory.Enable;

            #endregion
        }

        public AdminCategoryViewModel(ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel oCurrency)
        {
            C_CurrentExchangeId = oCurrency.CurrencyExchangeId.ToString();
            C_IssueDate = oCurrency.IssueDate.ToString();
            C_MoneyTypeFromId = oCurrency.MoneyTypeFrom.ItemId.ToString();
            C_MoneyTypeFromName = oCurrency.MoneyTypeFrom.ItemName.ToString();
            C_MoneyTypeToId = oCurrency.MoneyTypeTo.ItemId.ToString();
            C_MoneyTypeToName = oCurrency.MoneyTypeTo.ItemName.ToString();
            C_Rate = oCurrency.Rate.ToString();
            C_CreateDate = oCurrency.CreateDate.ToString();
            C_LastModify = oCurrency.LastModify.ToString();
        }
    }
}

