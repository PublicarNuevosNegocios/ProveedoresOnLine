using ProveedoresOnLine.Company.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Models.CalificationProject
{
    public class CalificationProjectConfigModel
    {
        public int CalificationProjectConfigId { get; set; }

        public CompanyModel Company { get; set; }

        public string CalificationProjectConfigName { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ConfigItemModel> ConfigItemModel { get; set; }

        public List<ConfigValidateModel> ConfigValidateModel { get; set; }
    }
}
