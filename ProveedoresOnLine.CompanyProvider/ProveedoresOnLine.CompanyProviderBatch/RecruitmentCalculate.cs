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
            GenericItemModel K_ContactModel = new GenericItemModel();
            decimal ExpirienceScore;
            decimal FinancialScore;

            #region Expiriences Caltulation

            int Years = 0;
            DateTime ConstitutionCompanydate = oProvider.RelatedLegal.FirstOrDefault().ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumLegalInfoType.CP_ConstitutionDate)
                                                .Select(x => Convert.ToDateTime(x.Value)).DefaultIfEmpty(DateTime.Now).FirstOrDefault();
            //Set Expirience Years
            Years = DateTime.Now.Year - ConstitutionCompanydate.Year;

            ExpirienceScore = GetTreeScore((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_ProviderExpirienceScore, Years);
            #endregion

            #region Financial Calculation
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
            oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(FinancialLastYear, 988);

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

            FinancialScore = FinancialScoreHeritage + FinancialScoreLiquidity + FinancialScoreIndebtedness;
            #endregion

            #region Organization Capacity

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

            decimal oCapacityByYear = 0;
            TwoLastBalaces.All(x =>
                {                    
                    TwoLastBalacesDetail = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial(x.ItemId);
                    decimal OperIng = TwoLastBalacesDetail.Where(y => y.RelatedAccount.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialDetailType.FD_OperatingIncome).
                                   Select(y => y.Value).DefaultIfEmpty(0).FirstOrDefault();

                    oMiminumWageModel = ProveedoresOnLine.Company.Controller.Company.MinimumWageSearchByYear(x.ItemInfo.Where(y => y.ItemInfoType.ItemId ==
                                        (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialInfoType.FI_Year).Select(y => Convert.ToInt32(y.Value)).FirstOrDefault(), 988);


                    oCapacityByYear += (OperIng / oMiminumWageModel.Value);
                    return true;
                });
            oCapacityByYear = oCapacityByYear / 2;



            
            #endregion

            return K_ContactModel;
        }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel GetConsultantScore(ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider)
        {
            return null;
        }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel GetBuilderScore(ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oProvider)
        {
            return null;
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
                        if (ValueToEval >= cat.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MinValue).Select(x => Convert.ToDecimal(x.Value)).FirstOrDefault()
                            && ValueToEval <= cat.ItemInfo.Where(x => x.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.K_MaxValue).Select(x => Convert.ToDecimal(x.Value)).FirstOrDefault())
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

        public MinimumWageModel oMiminumWageModel { get; set; }
    }
}
