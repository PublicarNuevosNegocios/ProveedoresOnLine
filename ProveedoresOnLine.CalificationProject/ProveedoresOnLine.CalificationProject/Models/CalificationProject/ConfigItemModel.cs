using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Models.CalificationProject
{
    public class ConfigItemModel
    {
        public int CalificationProjectConfigItemId { get; set; }

        public int CalificationProjectConfigId { get; set; }

        public string CalificationProjectConfigItemName { get; set; }

        public CatalogModel CalificationProjectConfigItemType { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ConfigItemInfoModel> CalificationProjectConfigItemInfoModel { get; set; }
    }
}
