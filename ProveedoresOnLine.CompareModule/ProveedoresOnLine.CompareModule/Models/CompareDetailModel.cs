using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompareModule.Models
{
    public class CompareDetailModel
    {
        public int EvaluationAreaId { get; set; }

        public string EvaluationAreaName { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// value order, value, unit
        /// </summary>
        public List<Tuple<int, string, string>> Value { get; set; }
    }
}
