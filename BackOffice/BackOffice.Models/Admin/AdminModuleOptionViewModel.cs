using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminModuleOptionViewModel
    {
        ProveedoresOnLine.Company.Models.Util.GenericItemModel oModuleOptions { get; set; }

        public string ModuleOptionId { get; set; }

        public string ModuleOption { get; set; }

        public string ModuleOptionTypeId { get; set; }

        public string ModuleOptionTypeName { get; set; }

        public bool Enable { get; set; }

        public string LastModify { get; set; }

        public string CreateDate { get; set; }

        public AdminModuleOptionViewModel() { }

        public AdminModuleOptionViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedModuleOption)
        {
            this.oModuleOptions = oRelatedModuleOption;

            this.ModuleOptionId = oModuleOptions.ItemId.ToString();

            this.ModuleOption = oModuleOptions.ItemName;

            this.ModuleOptionTypeId = oModuleOptions.ItemType.ItemId.ToString();

            this.ModuleOptionTypeName = oModuleOptions.ItemType.ItemName;

            this.Enable = oModuleOptions.Enable;

            this.LastModify = oModuleOptions.LastModify.ToString();

            this.CreateDate = oModuleOptions.CreateDate.ToString();
        }
    }
}
