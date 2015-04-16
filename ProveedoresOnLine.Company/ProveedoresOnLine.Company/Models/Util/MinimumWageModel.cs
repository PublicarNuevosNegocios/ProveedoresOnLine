using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class MinimumWageModel
    {
        public int MinimumWageId { get; set; }

        public int Year { get; set; }

        public CatalogModel Country { get; set; }

        public CatalogModel MoneyType { get; set; }

        public decimal Value { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
