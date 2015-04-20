using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProviderBatch
{
    internal class RecruitmentCalculate
    {
        private List<TreeModel> oKTree;
        public List<TreeModel> KTree
        {
            get
            {
                if (oKTree == null)
                {
                    oKTree = ProveedoresOnLine.Company.Controller.Company.TreeGetFullByType
                        ((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_ContrTreeType);
                }
                return oKTree;
            }
        }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel GetProviderScore(ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider)
        {
            GenericItemModel K_ContractModelToReturn;
            decimal ExpirienceScore;
            decimal FinancialScore;
            decimal CapacityOrganizationScore = 0;
            decimal oTechnicCapacity = CalculateTechnicCapacity(oProvider.RelatedCompany.CompanyPublicId);

            #region Expiriences Caltulation

            int Years = 0;
            DateTime ConstitutionCompanydate = oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumLegalInfoType.CP_ConstitutionDate)
                                                .Select(x => Convert.ToDateTime(x.Value)).DefaultIfEmpty(DateTime.Now).FirstOrDefault();
            //Set Expirience Years
            Years = DateTime.Now.Year - ConstitutionCompanydate.Year;

            ExpirienceScore = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_ProviderExpirienceScore, Years);
            #endregion

            #region Financial Caculation

            FinancialScore = CalculateFinancialCapacity(oProvider);

            #endregion

            #region Organization Capacity

            CapacityOrganizationScore = CalculateCapacityOrg(oProvider);

            #endregion
            
            #region Create K_Recruitment

            //Star to create the K_Recruitment Object return
            List<GenericItemModel> K_ContractModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo(oProvider.RelatedCompany.CompanyPublicId,
                                                              (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.KRecruitment, true);

            //Get the Financial Info when role like provider
            K_ContractModel = K_ContractModel != null ? K_ContractModel.Where(x => x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                                              (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType
                                                                && y.Value == Convert.ToInt32(CompanyProviderBatch.Models.Enumerations.enumUtil.K_Provider).ToString()).
                                                                Select(y => y.ItemInfoId).FirstOrDefault() != 0).Select(x => x).ToList() : null;
          

            K_ContractModelToReturn = new ProveedoresOnLine.Company.Models.Util.GenericItemModel
            {
                ItemId = K_ContractModel != null && K_ContractModel.Count > 0 ? K_ContractModel.FirstOrDefault().ItemId : 0,
                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.KRecruitment,
                },
                ItemName = "K_Recruitment",
                Enable = true,
                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalExpirienceScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalExpirienceScore,
                },
                Value = ExpirienceScore.ToString(),
                Enable = true,
            });

            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalFinancialScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalFinancialScore,
                },
                Value = FinancialScore.ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalOrgCapacityScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalOrgCapacityScore,
                },
                Value = CapacityOrganizationScore.ToString("0.##"),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalTechnicScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalTechnicScore,
                },
                Value = oTechnicCapacity.ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_MoneyType).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_MoneyType,
                },
                Value = Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.U_COP).ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalKValueScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalKValueScore,
                },
                Value = CalculateProviderScore(ExpirienceScore, FinancialScore, CapacityOrganizationScore).ToString("0.##"),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType,
                },
                Value = Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_Provider).ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalScore,
                },
                Value = (ExpirienceScore + FinancialScore + oTechnicCapacity).ToString(),
                Enable = true,
            });
            #endregion

            return K_ContractModelToReturn;
        }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel GetConsultantScore(ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider)
        {
            GenericItemModel K_ContractModelToReturn;
            decimal ExpirienceScore;
            decimal FinancialScore;
            decimal CapacityOrganizationScore = 0;
            decimal oTechnicCapacity = CalculateTechnicCapacity(oProvider.RelatedCompany.CompanyPublicId);

            #region Expiriences Caltulation

            int Years = 0;            

            DateTime ConstitutionCompanydate = oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumLegalInfoType.CP_ConstitutionDate)
                                                .Select(x => Convert.ToDateTime(x.Value)).DefaultIfEmpty(DateTime.Now).FirstOrDefault();
            //Set Expirience Years
            Years = DateTime.Now.Year - ConstitutionCompanydate.Year;

            decimal OldLonggerAge = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_YearsNumberConsultant, Years);

            List<GenericItemModel> oContractValue = new List<GenericItemModel>();
            //Obtain the contract whi he most value

            oContractValue = oProvider.RelatedCommercial.OrderByDescending(x => x.ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumCommercialInfoType.EX_ContractValue).
                                                                                               Select(y => Convert.ToDecimal(y.Value)).DefaultIfEmpty(0).FirstOrDefault()).ToList();
            oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(oContractValue.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                                                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumCommercialInfoType.EX_EndDate).
                                                                                                        Select(x => Convert.ToDateTime(x.Value).Year).DefaultIfEmpty(0).FirstOrDefault(), 988);

            decimal oMaxContractValue = oContractValue.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumCommercialInfoType.EX_ContractValue).
                                                                                Select(x => Convert.ToDecimal(x.Value)).DefaultIfEmpty(0).FirstOrDefault() / oMiminumWageModel.Value;

            oMaxContractValue = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MaxValueConsultant, oMaxContractValue);

            decimal maxContractCount = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_ContractsInBuilderConsultant, oContractValue.Count());

            ExpirienceScore = OldLonggerAge + oMaxContractValue + maxContractCount;
            #endregion

            #region Calculate Financial

            FinancialScore = CalculateFinancialCapacity(oProvider);

            #endregion

            #region Organization Capacity

            CapacityOrganizationScore = CalculateCapacityOrg(oProvider);

            #endregion

            #region Technic Capacity

            oTechnicCapacity = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_TecnicCapacityScore, oTechnicCapacity);

            #endregion

            #region Create K_Recruitment

            //Star to create the K_Recruitment Object return
            List<GenericItemModel> K_ContractModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo(oProvider.RelatedCompany.CompanyPublicId,
                                                              (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.KRecruitment, true);

            //Get the Financial Info when role like provider
            K_ContractModel = K_ContractModel != null ? K_ContractModel.Where(x => x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                                              (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType
                                                                && y.Value == Convert.ToInt32(CompanyProviderBatch.Models.Enumerations.enumUtil.K_Consultant).ToString()).
                                                                Select(y => y.ItemInfoId).FirstOrDefault() != 0).Select(x => x).ToList() : null;

            K_ContractModelToReturn = new ProveedoresOnLine.Company.Models.Util.GenericItemModel
            {
                ItemId = K_ContractModel != null && K_ContractModel.Count > 0 ? K_ContractModel.FirstOrDefault().ItemId : 0,
                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.KRecruitment,
                },
                ItemName = "K_Recruitment",
                Enable = true,
                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalExpirienceScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalExpirienceScore,
                },
                Value = ExpirienceScore.ToString(),
                Enable = true,
            });

            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalFinancialScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalFinancialScore,
                },
                Value = FinancialScore.ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalOrgCapacityScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalOrgCapacityScore,
                },
                Value = CapacityOrganizationScore.ToString("0.##"),                
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalTechnicScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalTechnicScore,
                },
                Value = oTechnicCapacity.ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_MoneyType).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_MoneyType,
                },
                Value = Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.U_COP).ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalKValueScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalKValueScore,
                },
                Value = CalculateConsultantScore(ExpirienceScore, FinancialScore, CapacityOrganizationScore, oTechnicCapacity).ToString("0.##"),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType,
                },
                Value = Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_Consultant).ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalScore,
                },
                Value = (ExpirienceScore + FinancialScore + oTechnicCapacity).ToString(),
                Enable = true,
            });
            #endregion

            return K_ContractModelToReturn;
        }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel GetBuilderScore(ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider)
        {
            GenericItemModel K_ContractModelToReturn;
            decimal ExpirienceScore;
            decimal FinancialScore;
            decimal CapacityOrganizationScore = 0;
            decimal oTechnicCapacity = CalculateTechnicCapacity(oProvider.RelatedCompany.CompanyPublicId);

            #region Expiriences Caltulation

            int Years = 0;
            DateTime ConstitutionCompanydate = oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumLegalInfoType.CP_ConstitutionDate)
                                                .Select(x => Convert.ToDateTime(x.Value)).DefaultIfEmpty(DateTime.Now).FirstOrDefault();
            //Set Expirience Years
            Years = DateTime.Now.Year - ConstitutionCompanydate.Year;

            ExpirienceScore = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_BuilderExpirienceScore, Years);
            #endregion

            #region Calculate Financial

            FinancialScore = CalculateFinancialCapacity(oProvider);            
            
            #endregion

            #region Organization Capacity

            CapacityOrganizationScore = CalculateCapacityOrg(oProvider);

            #endregion

            #region Technic Capacity

            oTechnicCapacity = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_TecnicCapacityScore, oTechnicCapacity);

            #endregion

            #region Create K_Recruitment

            //Star to create the K_Recruitment Object return
            List<GenericItemModel> K_ContractModel = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo(oProvider.RelatedCompany.CompanyPublicId,
                                                              (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.KRecruitment, true);

            //Get the Financial Info when role like provider
            K_ContractModel = K_ContractModel != null ? K_ContractModel.Where(x => x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                                              (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType
                                                                && y.Value == Convert.ToInt32(CompanyProviderBatch.Models.Enumerations.enumUtil.K_Builder).ToString()).
                                                                Select(y => y.ItemInfoId).FirstOrDefault() != 0).Select(x => x).ToList() : null;
            
            K_ContractModelToReturn = new ProveedoresOnLine.Company.Models.Util.GenericItemModel
            {
                ItemId = K_ContractModel != null && K_ContractModel.Count > 0  ? K_ContractModel.FirstOrDefault().ItemId : 0,
                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.KRecruitment,
                },
                ItemName = "K_Recruitment",
                Enable = true,
                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
            };

            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalExpirienceScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalExpirienceScore,
                },
                Value = ExpirienceScore.ToString(),
                Enable = true,
            });

            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalFinancialScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalFinancialScore,
                },
                Value = FinancialScore.ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalOrgCapacityScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalOrgCapacityScore,
                },
                Value = CapacityOrganizationScore.ToString("0.##"),  
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalTechnicScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalTechnicScore,
                },
                Value = oTechnicCapacity.ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_MoneyType).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_MoneyType,
                },
                Value = Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.U_COP).ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalKValueScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalKValueScore,
                },
                Value = CalculateBuilderScore(ExpirienceScore, FinancialScore, CapacityOrganizationScore, oTechnicCapacity).ToString("0.##"),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_RoleType,
                },
                Value = Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_Builder).ToString(),
                Enable = true,
            });
            K_ContractModelToReturn.ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
            {
                ItemInfoId = K_ContractModel != null && K_ContractModel.Count > 0 && K_ContractModel.FirstOrDefault().ItemInfo != null ?
                             K_ContractModel.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId ==
                                                            (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalScore).
                                                            Select(x => x.ItemInfoId).DefaultIfEmpty(0).FirstOrDefault() : 0,

                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                {
                    ItemId = (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FK_TotalScore,
                },
                Value = (ExpirienceScore + FinancialScore + oTechnicCapacity).ToString(),
                Enable = true,
            });
            #endregion

            return K_ContractModelToReturn;
        }

        private decimal GetTreeScore(int TreeId, decimal ValueToEval)
        {
            decimal oReturn = 0;

            KTree.Where(kt => kt.TreeId == TreeId).All(kt =>
            {
                kt.RelatedCategory.
                    OrderBy(cat => cat.ItemInfo.
                                    Where(catinf => catinf.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MinValue).
                                    Select(catinf => Convert.ToDecimal(catinf.Value)).
                                    DefaultIfEmpty(0).
                                    FirstOrDefault()).
                    All(cat =>
                    {

                        decimal minV = cat.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MinValue).Select(x => Convert.ToDecimal(x.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).FirstOrDefault();
                        decimal maxV = cat.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MaxValue).Select(x => Convert.ToDecimal(x.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).FirstOrDefault();
                        if (ValueToEval >= minV
                            && ValueToEval <= maxV)
                        {
                            oReturn = cat.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_Score).Select(x => Convert.ToDecimal(x.Value)).FirstOrDefault();
                        }
                        else if (ValueToEval > 0 && cat.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MaxScore).Select(x => Convert.ToDecimal(x.Value)).FirstOrDefault() != 0
                            && oReturn == 0)
                        {
                            oReturn = cat.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MaxScore).Select(x => Convert.ToDecimal(x.Value)).FirstOrDefault();
                        }
                        return true;
                    });
                return true;
            });

            return oReturn;
        }

        private decimal CalculateTechnicCapacity(string ProviderPublicId)
        {
            decimal oTechnicCapacity = 0;
            List<GenericItemModel> OrgaEstructure = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo(ProviderPublicId,
                                               (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.OrganizationalEstructure, true);
            if (OrgaEstructure != null && OrgaEstructure.Count > 0)
            {
                OrgaEstructure.All(x =>
                {
                    oTechnicCapacity += x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FO_TechnicPersonal).
                                        Select(y => Convert.ToInt32(y.Value)).DefaultIfEmpty(0).FirstOrDefault();
                    oTechnicCapacity += x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FO_AdministersPersonal).
                                        Select(y => Convert.ToInt32(y.Value)).DefaultIfEmpty(0).FirstOrDefault();
                    oTechnicCapacity += x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FO_CommercialPersonal).
                                        Select(y => Convert.ToInt32(y.Value)).DefaultIfEmpty(0).FirstOrDefault();
                    oTechnicCapacity += x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FO_ContratistActive).
                                        Select(y => Convert.ToInt32(y.Value)).DefaultIfEmpty(0).FirstOrDefault();
                    oTechnicCapacity += x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FO_ActiveProviders).
                                        Select(y => Convert.ToInt32(y.Value)).DefaultIfEmpty(0).FirstOrDefault();
                    oTechnicCapacity += x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FO_Partners).
                                        Select(y => Convert.ToInt32(y.Value)).DefaultIfEmpty(0).FirstOrDefault();

                    return true;
                });
            }
            return oTechnicCapacity;
        }

        private decimal CalculateFinancialCapacity(ProviderModel oProvider)
        {
            //Get Last year
            int FinancialLastYear = 0;
            GenericItemModel oFinancialLastYear = new GenericItemModel();
            MinimumWageModel oMiminumWageMode;
            oProvider.RelatedFinantial.All(x =>
            {
                if (FinancialLastYear == 0)
                {
                    FinancialLastYear = x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FI_Year)
                                                        .Select(y => Convert.ToInt32(y.Value)).FirstOrDefault();
                }
                int itemYear = x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FI_Year)
                                                        .Select(y => Convert.ToInt32(y.Value)).FirstOrDefault();
                if (itemYear >= FinancialLastYear)
                    FinancialLastYear = itemYear;
                return true;
            });

            oFinancialLastYear = oProvider.RelatedFinantial.Where(x => x.ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FI_Year &&
                                            Convert.ToInt32(y.Value) == FinancialLastYear).Select(y => y).FirstOrDefault() != null)
                                                            .Select(x => x).FirstOrDefault();
            //Get detail to evaluate
            List<BalanceSheetDetailModel> BalanceDetailList;
            BalanceDetailList = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial(oFinancialLastYear.ItemId);

            //Get Minimmum Wage
            oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(FinancialLastYear, 
                                Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.InternalSettings.Instance[ProveedoresOnLine.CompanyProviderBatch.Models.Constants.C_Settings_Company_DefaultCountry].Value)); 

            decimal FinancialScoreHeritage = 0;
            decimal FinancialScoreLiquidity = 0;
            decimal FinancialScoreIndebtedness = 0;
            decimal CurrentHeritage = 0;

            CurrentHeritage = BalanceDetailList.Where(x => x.RelatedAccount.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialDetailType.FD_TotalHeritage).
                                    Select(x => x.Value).DefaultIfEmpty(0).FirstOrDefault();

            //Get First Table score
            if (oMiminumWageModel != null)
            {
                CurrentHeritage = (CurrentHeritage / oMiminumWageModel.Value);
                FinancialScoreHeritage = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_ScoreHeritage, CurrentHeritage);
            }

            decimal CurrentActive = BalanceDetailList.Where(x => x.RelatedAccount.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialDetailType.FD_CurrentActive).
                                    Select(x => x.Value).DefaultIfEmpty(0).FirstOrDefault();

            decimal CurrentPassive = BalanceDetailList.Where(x => x.RelatedAccount.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialDetailType.FD_CurentPassive).
                                    Select(x => x.Value).DefaultIfEmpty(0).FirstOrDefault();
            decimal ResulLiquidity = CurrentActive / CurrentPassive;

            FinancialScoreLiquidity = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_LiquidityScore, ResulLiquidity);

            decimal TotalActive = BalanceDetailList.Where(x => x.RelatedAccount.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialDetailType.FD_TotalActive).
                                    Select(x => x.Value).DefaultIfEmpty(0).FirstOrDefault();
            decimal TotalPassive = BalanceDetailList.Where(x => x.RelatedAccount.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialDetailType.FD_TotalPassive).
                                   Select(x => x.Value).DefaultIfEmpty(0).FirstOrDefault();

            decimal ResultIndebtedness = (TotalActive / TotalPassive) * 100;
            FinancialScoreIndebtedness = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_IndebtednessScore, ResultIndebtedness);

            return FinancialScoreHeritage + FinancialScoreLiquidity + FinancialScoreIndebtedness;
        }

        private decimal CalculateCapacityOrg(ProviderModel oProvider)
        {
            decimal CapacityByYearScore = 0;
            List<GenericItemModel> TwoLastBalaces = new List<GenericItemModel>();
            List<BalanceSheetDetailModel> TwoLastBalacesDetail;
            List<string> LastTwoYears = new List<string>();

            oProvider.RelatedFinantial.All(x =>
            {
                LastTwoYears.Add(x.ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FI_Year).
                                Select(y => y.Value).FirstOrDefault());
                return true;
            });

            LastTwoYears = LastTwoYears.OrderByDescending(x => x).ToList();

            LastTwoYears.All(ly =>
            {
                if (LastTwoYears.IndexOf(ly) <= 1)
                {
                    TwoLastBalaces.Add(oProvider.RelatedFinantial.
                                        Where(x => x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FI_Year
                                        && y.Value == ly).Select(y => y).FirstOrDefault() != null).
                                        Select(x => x).FirstOrDefault());
                }
                return true;
            });

            TwoLastBalaces.All(x =>
            {
                TwoLastBalacesDetail = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial(x.ItemId);
                decimal OperIng = TwoLastBalacesDetail.Where(y => y.RelatedAccount.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialDetailType.FD_OperatingIncome).
                               Select(y => y.Value).DefaultIfEmpty(0).FirstOrDefault();

                oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                    (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FI_Year).Select(y => Convert.ToInt32(y.Value)).FirstOrDefault(), 988);


                CapacityByYearScore += (OperIng / oMiminumWageModel.Value);
                return true;
            });
            return CapacityByYearScore / 2;
        }

        private decimal CalculateProviderScore(decimal oTotalExperience, decimal oTotalFinancial, decimal oTotalCapacityOrg)
        {
            decimal oTotalResult;
            oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(DateTime.Now.Year,
                Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.InternalSettings.Instance[ProveedoresOnLine.CompanyProviderBatch.Models.Constants.C_Settings_Company_DefaultCountry].Value)); 

            oTotalResult = oTotalExperience + oTotalFinancial;
            oTotalResult = oTotalResult / 1000;
            oTotalResult = oTotalResult + 1;
            oTotalResult = oTotalCapacityOrg * oTotalResult;           

            return oTotalResult;
        }

        private decimal CalculateBuilderScore(decimal oTotalExperience, decimal oTotalFinancial, decimal oTotalCapacityOrg ,  decimal oTechnicCapacity)
        {
            decimal oTotalResult;
            oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(DateTime.Now.Year, 
                Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.InternalSettings.Instance[ProveedoresOnLine.CompanyProviderBatch.Models.Constants.C_Settings_Company_DefaultCountry].Value)); 

            oTotalResult = oTotalExperience + oTotalFinancial + oTechnicCapacity;
            oTotalResult = oTotalResult / 1000;
            oTotalResult = oTotalResult + 1;
            decimal FPIResult = Convert.ToDecimal(ProveedoresOnLine.CompanyProviderBatch.Models.InternalSettings.Instance[ProveedoresOnLine.CompanyProviderBatch.Models.Constants.C_Settings_Company_DefaultCountry].Value) * oTotalCapacityOrg;

            oTotalResult = oTotalResult * FPIResult;

            //oTotalResult = oTotalResult * oMiminumWageModel.Value;
            return oTotalResult;
        }

        private decimal CalculateConsultantScore(decimal oTotalExperience, decimal oTotalFinancial, decimal oTotalCapacityOrg, decimal oTechnicCapacity)
        {
            decimal oTotalResult;
            oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(DateTime.Now.Year,
                Convert.ToInt32(ProveedoresOnLine.CompanyProviderBatch.Models.InternalSettings.Instance[ProveedoresOnLine.CompanyProviderBatch.Models.Constants.C_Settings_Company_DefaultCountry].Value)); 

            oTotalResult = oTotalExperience + oTotalFinancial + oTechnicCapacity;
            oTotalResult = oTotalResult / 1000;
            oTotalResult = oTotalResult + 1;
            decimal FPIResult = Convert.ToDecimal(1.6333) * oTotalCapacityOrg;

            oTotalResult = oTotalResult * FPIResult;

            //oTotalResult = oTotalResult * oMiminumWageModel.Value;
            return oTotalResult;
        }

        public MinimumWageModel oMiminumWageModel { get; set; }
    }
}
