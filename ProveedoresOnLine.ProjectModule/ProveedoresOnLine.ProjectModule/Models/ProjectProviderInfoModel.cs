using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.Models
{
    public class ProjectProviderInfoModel : ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedEvaluationItem { get; set; }
    }
}
