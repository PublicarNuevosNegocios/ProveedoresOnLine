﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminModuleOptionViewModel
    {
        ProveedoresOnLine.Company.Models.Util.GenericItemModel oModuleOptions { get; set; }

        ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel oModuleOptionInfo { get; set; }


        #region ModuleOption

        public string ModuleOptionId { get; set; }

        public string ModuleOption { get; set; }

        public string ModuleOptionTypeId { get; set; }

        public string ModuleOptionTypeName { get; set; }

        #endregion

        #region ModuleOptionInfo

        public string ModuleOptionInfoId { get; set; }

        public string ModuleOptionInfoTypeId { get; set; }

        public string ModuleOptionInfoTypeName { get; set; }

        public string ModuleOptionInfoValue { get; set; }

        public string ModuleOptionInfoLargeValue { get; set; }

        #endregion

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

        public AdminModuleOptionViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel oRelatedModuleOptionInfo)
        {
            oModuleOptionInfo = oRelatedModuleOptionInfo;

            this.ModuleOptionInfoId = oModuleOptionInfo.ItemInfoId.ToString();

            this.ModuleOptionInfoTypeId = oModuleOptionInfo.ItemInfoType.ItemId.ToString();

            this.ModuleOptionInfoTypeName = oModuleOptionInfo.ItemInfoType.ItemName;

            this.ModuleOptionInfoValue = oModuleOptionInfo.Value;

            this.ModuleOptionInfoLargeValue = oModuleOptionInfo.LargeValue;

            this.Enable = oModuleOptionInfo.Enable;

            this.LastModify = oModuleOptionInfo.LastModify.ToString();

            this.CreateDate = oModuleOptionInfo.CreateDate.ToString();
        }
    }
}
