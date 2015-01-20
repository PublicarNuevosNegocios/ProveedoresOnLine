using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Models.Provider
{
    public class BlackListModel
    {
        public int BlackListId { get; set; }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel BlackListStatus { get; set; }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel BlackListType { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> BlackListInfo { get; set; }

    }
}
