using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProviderBatch.Models
{
    public class Enumerations
    {
        #region General K

        public enum enumUtil
        {
            RoleType = 102005,
            K_ContrTreeType = 114014,

            K_MinValue = 117001,
            K_MaxValue = 117002,
            K_Score = 117003,
            K_MaxScore = 117004,

            //Tree Tables
            K_ProviderExpirienceScore = 37,
            K_BuilderExpirienceScore = 38,
            K_YearsNumberConsultant = 39,
            K_MaxValueConsultant = 40,
            K_ContractsInBuilderConsultant = 41,
            K_ScoreHeritage = 42,
            K_LiquidityScore = 43,
            K_IndebtednessScore = 44,
            K_TecnicCapacityScore = 45
        }
        #endregion

        #region EconomyActivity

        public enum enumActivitiEconomicInfoType
        {
            Provider = 610001,
            Consultant = 610002,
            Builder = 610003,
        }

        #endregion

        #region Commecial
        public enum enumComercialType
        {
            Experience = 301001,
        }

        public enum enumCommercialInfoType
        {
            //Experience            
            EX_EconomicActivity = 302013,
            EX_CustomEconomicActivity = 302014,
        }
        #endregion

        #region Legal
        public enum enumLegalType
        {
            ChaimberOfCommerce = 601001,
        }

        public enum enumLegalInfoType
        {
            CP_ConstitutionDate = 602001,
        }
        #endregion

        #region Financial

        public enum enumFinancialType
        {
            BalanceSheetInfoType = 501001,
        }

        public enum enumFinancialInfoType
        {
            FI_Year = 502001,
        }

        public enum enumFinancialDetailType
        {
            FD_CurrentActive = 3118,
            FD_CurentPassive = 3120,
        }

        //enum AccountsInfo 

        #endregion
    }
}
