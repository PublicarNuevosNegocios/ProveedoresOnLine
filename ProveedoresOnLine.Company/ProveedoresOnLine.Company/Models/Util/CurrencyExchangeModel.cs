using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class CurrencyExchangeModel
    {
        public int CurrencyExchangeId { get; set; }

        public DateTime IssueDate { get; set; }

        public CatalogModel MoneyTypeFrom { get; set; }

        public CatalogModel MoneyTypeTo { get; set; }

        public decimal Rate { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
