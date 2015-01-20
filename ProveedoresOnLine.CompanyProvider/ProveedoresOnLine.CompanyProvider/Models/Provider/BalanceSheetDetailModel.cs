using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Models.Provider
{
    public class BalanceSheetDetailModel
    {
        public int BalanceSheetId { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount { get; set; }

        public decimal Value { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
