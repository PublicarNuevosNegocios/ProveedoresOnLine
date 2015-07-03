using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.Models
{
    public class SurveyModel
    {
        public string SurveyPublicId { get; set; }

        public ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel RelatedProvider { get; set; }

        public SurveyConfigModel RelatedSurveyConfig { get; set; }        

        public bool Enable { get; set; }
        
        public string ParentSurveyPublicId { get; set; }

        public string User { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> SurveyInfo { get; set; }

        public List<SurveyItemModel> RelatedSurveyItem { get; set; }

        public List<SurveyModel> ChildSurvey { get; set; }
    }
}
