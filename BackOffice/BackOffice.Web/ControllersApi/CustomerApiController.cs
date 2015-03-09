﻿using BackOffice.Models.Provider;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyCustomer.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class CustomerApiController : BaseApiController
    {
        #region Search Methods

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Customer.CustomerSearchViewModel> SMCustomerSearch
            (string SMCustomerSearch,
            string SearchParam,
            string PageNumber,
            string RowCount)
        {
            string oCompanyType =
                    ((int)(BackOffice.Models.General.enumCompanyType.Buyer)).ToString() + "," +
                    ((int)(BackOffice.Models.General.enumCompanyType.BuyerProvider)).ToString();

            int oPageNumber = string.IsNullOrEmpty(PageNumber) ? 0 : Convert.ToInt32(PageNumber.Trim());

            int oRowCount = Convert.ToInt32(string.IsNullOrEmpty(RowCount) ?
                BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value :
                RowCount.Trim());

            int oTotalRows;

            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oSearchResult =
                ProveedoresOnLine.Company.Controller.Company.CompanySearch
                    (oCompanyType,
                    SearchParam,
                    string.Empty,
                    oPageNumber,
                    oRowCount,
                    out oTotalRows);

            List<BackOffice.Models.Customer.CustomerSearchViewModel> oReturn = new List<Models.Customer.CustomerSearchViewModel>();

            if (oSearchResult != null && oSearchResult.Count > 0)
            {
                oSearchResult.All(sr =>
                {
                    oReturn.Add(new Models.Customer.CustomerSearchViewModel(sr, oTotalRows));
                    return true;
                });
            }

            return oReturn;
        }

        #endregion

        #region Customer Rules

        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Customer.CustomerRoleViewModel> UserRolesByCustomer
        (string UserRolesByCustomer,
            string CustomerPublicId)
        {
            List<BackOffice.Models.Customer.CustomerRoleViewModel> oReturn = new List<Models.Customer.CustomerRoleViewModel>();

            if (UserRolesByCustomer == "true")
            {
                List<ProveedoresOnLine.Company.Models.Company.UserCompany> oUsersRole = ProveedoresOnLine.Company.Controller.Company.RoleCompany_GetUsersByPublicId
                    (CustomerPublicId);

                if (oUsersRole != null)
                {
                    oUsersRole.All(x =>
                        {
                            oReturn.Add(new BackOffice.Models.Customer.CustomerRoleViewModel(x));
                            return true;
                        });
                }
            }

            return oReturn;
        }

        #endregion
    }
}
