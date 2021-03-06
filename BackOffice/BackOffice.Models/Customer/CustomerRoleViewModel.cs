﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class CustomerRoleViewModel
    {
        #region Role Company

        public string RoleId { get; set; }
        public string RoleName { get; set; }

        #endregion

        #region User Role Company

        public string RoleCompanyId { get; set; }

        public string RoleCompanyName { get; set; }

        public string RoleCompanyEnable { get; set; }

        public string UserCompanyId { get; set; }

        public string User { get; set; }

        public string UserCompanyEnable { get; set; }

        #endregion

        public CustomerRoleViewModel() { }

        public CustomerRoleViewModel(ProveedoresOnLine.Company.Models.Company.UserCompany oUserRole)
        {
            RoleCompanyId = oUserRole.RelatedRole.ItemId.ToString();

            RoleCompanyName = oUserRole.RelatedRole.ItemName;

            RoleCompanyEnable = oUserRole.RelatedRole.Enable.ToString();

            UserCompanyId = oUserRole.UserCompanyId.ToString();

            User = oUserRole.User;

            UserCompanyEnable = oUserRole.Enable.ToString();
        }

        public CustomerRoleViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oRole)
        {
            RoleId = oRole.ItemId.ToString();

            RoleName = oRole.ItemName;
        }
    }
}
