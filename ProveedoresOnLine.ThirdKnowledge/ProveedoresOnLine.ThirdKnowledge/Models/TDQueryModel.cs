using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Models
{
    public class TDQueryModel
    {
        public string QueryPublicId { get; set; }

        public string PeriodPublicId { get; set; }

        public TDCatalogModel SearchType { get; set; }

        public string  User { get; set; }

        public bool IsSuccess { get; set; }

        public bool Enable { get; set; }

        public DateTime CreateDate { get; set; }

        public List<TDQueryInfoModel> RelatedQueryInfoModel { get; set; }
    }
}
