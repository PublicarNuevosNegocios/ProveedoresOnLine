using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.Models
{
    public class SurveyIndexSearchModel
    {
        public CompanyIndexSearchModel oCompanyIndexSearchModel { get; set; }

        public string SurveyPublicId { get; set; }

        public string SurveyTypeId { get; set; }

        public string SurveyType { get; set; }

        public string SurveyStatusId { get; set; }

        public string SurveyStatus { get; set; }
    }
}
