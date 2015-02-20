using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompareModule.Models
{
    public class CompareModel
    {
        public int CompareId { get; set; }

        public string CompareName { get; set; }

        public string User { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<CompareCompanyModel> RelatedProvider { get; set; }
    }
}
