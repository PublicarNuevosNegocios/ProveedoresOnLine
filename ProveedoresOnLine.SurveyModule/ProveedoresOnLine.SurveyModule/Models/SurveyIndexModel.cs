using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.Models
{
    [ElasticsearchType(Name = "Survey_Info")]
    public class SurveyIndexModel
    {
        public SurveyIndexModel() { }

        ProveedoresOnLine.Company.Models.Company.CompanyIndexModel oCompanyIndexModel { get; set; }

        //Survey
        public int SurveyTypeId { get; set; }
        public string SurveyType { get; set; }

        public int SurveyStatusId { get; set; }
        public string SurveyStatus { get; set; }
    }
}
