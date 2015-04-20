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
            K_Provider = 117005,
            K_Consultant = 117006,
            K_Builder = 117007,
            

            //Tree Tables
            K_ProviderExpirienceScore = 37,
            K_BuilderExpirienceScore = 38,
            K_YearsNumberConsultant = 39,
            K_MaxValueConsultant = 40,
            K_ContractsInBuilderConsultant = 41,
            K_ScoreHeritage = 42,
            K_LiquidityScore = 43,
            K_IndebtednessScore = 44,
            K_TecnicCapacityScore = 45,

            //Money Type
            U_USD = 108001,
            U_COP = 108002,
            U_EUR = 108003,	
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
            EX_ContractValue = 302007,            
            EX_StartDate=302003,
            EX_EndDate=302004,            
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
            OrganizationalEstructure = 501005,
            KRecruitment = 501006
        }

        public enum enumFinancialInfoType
        {
            FI_Year = 502001,
            
            FO_TechnicPersonal = 506002,
            FO_AdministersPersonal = 506003, 
            FO_CommercialPersonal = 506004, 
            FO_ContratistActive = 506005,
            FO_ActiveProviders = 506006,           
            FO_Partners = 506007,
                    
            //K_Recruitment InfoType
            FK_TotalExpirienceScore = 507001,
            FK_TotalFinancialScore = 507002,
            FK_TotalOrgCapacityScore = 507003,
            FK_TotalTechnicScore = 507004,
            FK_MoneyType = 507005,
            FK_TotalKValueScore = 507006,
            FK_RoleType = 507007,
            FK_TotalScore = 507008,
        }

        public enum enumFinancialDetailType
        {
            //Acounts
            FD_TotalActive = 3115,
            FD_TotalPassive = 3116,
            FD_TotalHeritage = 3117,
            FD_CurrentActive = 3118,
            FD_CurentPassive = 3120,
            FD_OperatingIncome = 3821,
        }        

        #endregion
    }
}
