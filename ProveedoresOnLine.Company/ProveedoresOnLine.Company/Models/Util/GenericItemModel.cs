using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Util
{
    public class GenericItemModel
    {
        public int ItemId { get; set; }

        public CatalogModel ItemType { get; set; }

        public string ItemName { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<GenericItemInfoModel> ItemInfo { get; set; }

        public GenericItemModel ParentItem { get; set; }

    }
}
