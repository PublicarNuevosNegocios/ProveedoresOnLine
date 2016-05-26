using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Models.CalificationProject
{
    public class ConfigValidateModel
    {
        public int CalificationProjectConfigValidateId { get; set; }

        public int CalificationProjectConfigId { get; set; }

        public CatalogModel Operator { get; set; }

        public string Value { get; set; }

        public string Result { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
