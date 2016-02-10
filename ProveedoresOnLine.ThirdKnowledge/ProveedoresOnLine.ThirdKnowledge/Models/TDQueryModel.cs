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

        public string CompayPublicId {get; set; }

        public string PeriodPublicId { get; set; }

        public TDCatalogModel SearchType { get; set; }

        public string  User { get; set; }

        public TDCatalogModel QueryStatus { get; set; }

        public bool IsSuccess { get; set; }

        public bool Enable { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModify{ get; set; }

        public List<TDQueryInfoModel> RelatedQueryBasicInfoModel { get; set; }

        public string FileName { get; set; }
    }
}
