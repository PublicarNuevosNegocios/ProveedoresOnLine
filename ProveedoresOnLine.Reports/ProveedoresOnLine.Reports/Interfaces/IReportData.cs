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

        #region ReportSurveyGetAllByCustomer

        List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId);

        SurveyModel SurveyGetByParentUser(string ParentSurveyPublicId, string User);
        
        #endregion

        #region Gerencial Report



        #region Data

        Company.Models.Company.CompanyModel C_Report_MPCompanyGetBasicInfo(string CompanyPublicId);

        List<ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel> C_Report_BlackListGetBasicInfo(string CompanyPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPContactGetBasicInfo(string CompanyPublicId, int? ContactType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetLastyearInfoDeta(string ProviderPublicId);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType);

        List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> C_Report_MPCertificationGetSpecificCert(string ProviderPublicId);

        #endregion

        #endregion

        #region SelectionProcess Report

        ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetByIdProviderDetail(string ProjectPublicId, string CustomerPublicId, string ProviderPublicId);

        #endregion

        #endregion
    }
}
