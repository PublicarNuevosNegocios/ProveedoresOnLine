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

        public string CompanyPublicId { get; set; }

        public ProveedoresOnLine.Company.Models.Util.CatalogModel BlackListStatus { get; set; }

        public string User{ get; set; }

        public string FileUrl { get; set; }       

        public DateTime CreateDate { get; set; }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> BlackListInfo { get; set; }

        public DateTime LastInquiry { get; set; }
    }
}
