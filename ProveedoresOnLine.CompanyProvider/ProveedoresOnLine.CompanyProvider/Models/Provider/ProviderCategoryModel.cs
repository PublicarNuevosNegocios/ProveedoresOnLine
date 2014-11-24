using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Models.Provider
{
    public class ProviderCategoryModel
    {
        public int CompanyCategoryId { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedCategory { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
