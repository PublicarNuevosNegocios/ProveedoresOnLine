using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Compare
{
    public class CompareDetailViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedEvaluationArea { get; set; }

        public List<ProveedoresOnLine.CompareModule.Models.CompareCompanyModel> RelatedCompanyInfo { get; set; }

        public List<Tuple<int, string>> Colum { get; set; }

        public List<CompareDetailViewModel> ChildCompareDetail { get; set; }
    }
}
