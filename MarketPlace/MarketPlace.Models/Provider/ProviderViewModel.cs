using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderViewModel
    {
        public bool RenderScripts { get; set; }

        public ProveedoresOnLine.ProjectModule.Models.ProjectModel RelatedProject { get; set; }

        public ProviderLiteViewModel RelatedLiteProvider { get; set; }

        public List<MarketPlace.Models.General.GenericMenu> ProviderMenu { get; set; }

        public MarketPlace.Models.General.GenericMenu CurrentSubMenu
        {
            get
            {
                if (ProviderMenu != null)
                {
                    return ProviderMenu.
                                Where(x => x.ChildMenu.Any(y => y.IsSelected && !string.IsNullOrEmpty(y.Url))).
                                Select(x => x.ChildMenu.
                                    Where(y => y.IsSelected && !string.IsNullOrEmpty(y.Url)).
                                    FirstOrDefault()).
                                FirstOrDefault();
                }
                return null;
            }
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> ContactCompanyInfo { get; set; }

        public List<ProviderContactViewModel> RelatedGeneralInfo { get; set; }

        public List<ProviderComercialViewModel> RelatedComercialInfo { get; set; }

        public List<ProviderHSEQViewModel> RelatedHSEQlInfo { get; set; }

        public List<ProviderFinancialViewModel> RelatedFinancialInfo { get; set; }

        public List<ProviderFinancialViewModel> RelatedKContractInfo { get; set; }

        public List<ProviderBalanceSheetViewModel> RelatedBalanceSheetInfo { get; set; }

        public List<ProviderLegalViewModel> RelatedLegalInfo { get; set; }

        public List<ProviderDesignationsViewModel> RelatedDesignationsInfo { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedBlackListInfo { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> RelatedTrackingInfo { get; set; }

        public List<ProviderFinancialBasicInfoViewModel> RelatedFinancialBasicInfo { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> RelatedCertificationBasicInfo { get; set; }

        public MarketPlace.Models.Survey.SurveySearchViewModel RelatedSurveySearch { get; set; }

        public MarketPlace.Models.Survey.SurveyViewModel RelatedSurvey { get; set; }

        public List<ProviderReportsViewModel> RelatedReportInfo { get; set; }
    }
}
