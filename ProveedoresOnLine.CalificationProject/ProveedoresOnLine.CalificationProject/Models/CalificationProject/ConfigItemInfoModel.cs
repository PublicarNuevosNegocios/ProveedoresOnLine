using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Models.CalificationProject
{
    public class ConfigItemInfoModel
    {
        public int CalificationProjectConfigItemInfoId { get; set; }

        public CatalogModel Question { get; set; }

        public CatalogModel Rule { get; set; }

        public CatalogModel ValueType { get; set; }

        public string Value { get; set; }

        public string Score { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
