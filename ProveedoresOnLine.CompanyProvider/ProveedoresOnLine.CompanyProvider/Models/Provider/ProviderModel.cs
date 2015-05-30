using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Models.Provider
{
    public class ProviderModel
    {
        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; set; }

        #region Commercial Experience

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedCommercial { get; set; }

        #endregion

        #region HSEQ

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedCertification { get; set; }

        #endregion

        #region Finantial

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedFinantial { get; set; }

        public List<BalanceSheetModel> RelatedBalanceSheet { get; set; }

        #endregion

        #region Legal

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedLegal { get; set; }

        #endregion

        #region BlackList

        public List<BlackListModel> RelatedBlackList { get; set; }

        #endregion

        #region Reports

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedReports { get; set; }

        #endregion

        #region Related Customer Info

        /// <summary>
        /// CustomerPublicId,CustomerProviderInfo
        /// </summary>
        public Dictionary<string, ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedCustomerInfo { get; set; }

        #endregion
    }
}
