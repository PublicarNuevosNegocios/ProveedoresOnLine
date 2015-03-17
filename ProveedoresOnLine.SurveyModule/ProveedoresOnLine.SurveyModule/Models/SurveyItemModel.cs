using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.Models
{
    public class SurveyItemModel : ProveedoresOnLine.Company.Models.Util.GenericItemModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedSurveyConfigItem { get; set; }
    }
}
