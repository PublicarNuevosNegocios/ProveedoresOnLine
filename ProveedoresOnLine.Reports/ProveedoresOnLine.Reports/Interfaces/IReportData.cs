using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.Interfaces
{
    internal interface IReportData
    {
        #region Reports

        #region Gerencial Report

        Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId);

        List<Company.Models.Util.GenericItemModel> BlackListGetByCompanyPublicId(string CompanyPublicId);

        #endregion

        #endregion
    }
}
