using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Models
{
    public class TDQueryInfoModel
    {
        public int QueryBasicInfoId { get; set; }

        public string QueryBasicPublicId { get; set; }

        public string QueryPublicId { get; set; }

        public string NameResult { get; set; }

        public string IdentificationResult { get; set; }

        public string Priority { get; set; }

        public string Peps { get; set; }

        public string Status { get; set; }

        public string Alias { get; set; }

        public string Offense { get; set; }

        public bool Enable { get; set; }

        public List<TDQueryDetailInfoModel> DetailInfo { get; set; }
    }
}
