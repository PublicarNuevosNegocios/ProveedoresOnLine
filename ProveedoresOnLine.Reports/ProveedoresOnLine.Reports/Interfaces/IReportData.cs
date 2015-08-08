using Microsoft.Reporting.WebForms;
using ProveedoresOnLine.SurveyModule.Models;
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

        #region report
        
        List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId);
        
        SurveyModel SurveyGetById(string SurveyPublicId);

        List<string> SurveyGetIdsChildrenByParent(string vParentPublicId);

        #endregion

        #region Gerencial Report

        Company.Models.Company.CompanyModel C_Report_MPCompanyGetBasicInfo(string CompanyPublicId);

        List<Company.Models.Util.GenericItemModel> C_Report_BlackListGetByCompanyPublicId(string CompanyPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPContactGetBasicInfo(string CompanyPublicId, int? ContactType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetLastyearInfoDeta(string ProviderPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> C_Report_MPCertificationGetSpecificCert(string ProviderPublicId);

        #endregion

        #endregion
    }
}
