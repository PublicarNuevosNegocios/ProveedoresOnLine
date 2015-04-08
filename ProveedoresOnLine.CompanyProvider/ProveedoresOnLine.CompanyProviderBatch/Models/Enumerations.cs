using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProviderBatch.Models
{
    public class Enumerations
    {
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

      //enum AccountsInfo 

        #endregion
    }
}
