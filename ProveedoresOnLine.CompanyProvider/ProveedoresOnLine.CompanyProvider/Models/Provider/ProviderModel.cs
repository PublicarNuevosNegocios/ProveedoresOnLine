using System.Collections.Generic;

namespace ProveedoresOnLine.CompanyProvider.Models.Provider
{
    public class ProviderModel
    {
        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> ContactCompanyInfo { get; set; }

        #region Commercial Experience

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedCommercial { get; set; }

        #endregion Commercial Experience

        #region HSEQ

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedCertification { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedCertificationBasic { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> RelatedCertificationBasicInfo { get; set; }

        #endregion HSEQ

        #region Finantial

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedFinantial { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedKContractInfo { get; set; }

        public List<BalanceSheetModel> RelatedBalanceSheet { get; set; }

        #endregion Finantial

        #region Legal

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedLegal { get; set; }

        #endregion Legal

        #region BlackList

        public List<BlackListModel> RelatedBlackList { get; set; }

        #endregion BlackList

        #region Reports

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedReports { get; set; }

        #endregion Reports

        #region Aditional Documents

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedAditionalDocuments { get; set; }

        #endregion Aditional Documents

        #region Related Customer Info

        /// <summary>
        /// CustomerPublicId,CustomerProviderInfo
        /// </summary>
        public Dictionary<string, ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedCustomerInfo { get; set; }

        #endregion Related Customer Info

        #region Integration

        public List<ProveedoresOnLine.CompanyProvider.Models.Integration.AditionalFieldModel> AditionalData { get; set; }

        #endregion
    }
}