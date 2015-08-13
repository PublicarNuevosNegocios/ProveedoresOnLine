using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.SurveyModule.Models;

namespace ProveedoresOnLine.Reports.DAL.Controller
{
    internal class ReportsDataController : ProveedoresOnLine.Reports.Interfaces.IReportData
    {
        #region singleton instance

        private static Interfaces.IReportData oInstance;
        internal static Interfaces.IReportData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ReportsDataController();
                return oInstance;
            }
        }

        private Interfaces.IReportData DataFactory;

        #endregion

        #region Constructor
        public ReportsDataController()
        {
            ReportsDataFactory factory = new ReportsDataFactory();
            DataFactory = factory.GetReportsInstance();
        }
        #endregion

        #region ReportSurveyGetAllByCustomer

        public List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId)
        {
            return DataFactory.SurveyGetAllByCustomer(CustomerPublicId);
        }

        public SurveyModel SurveyGetByParentUser(string ParentSurveyPublicId, string User) {
            return DataFactory.SurveyGetByParentUser(ParentSurveyPublicId,User);
        }

        #endregion

        #region Gerencial Report

        public Company.Models.Company.CompanyModel C_Report_MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.C_Report_MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public List<ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel> C_Report_BlackListGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.C_Report_BlackListGetBasicInfo(CompanyPublicId);    
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            return DataFactory.C_Report_MPContactGetBasicInfo(CompanyPublicId, ContactType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            return DataFactory.C_Report_MPLegalGetBasicInfo(CompanyPublicId, LegalType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId)
        {
            return DataFactory.C_Report_MPCustomerProviderGetTracking(CustomerPublicId, ProviderPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetLastyearInfoDeta(string ProviderPublicId)
        {
            return DataFactory.C_Report_MPFinancialGetLastyearInfoDeta(ProviderPublicId);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable)
        {
            return DataFactory.C_Report_FinancialGetBasicInfo(CompanyPublicId, FinancialType, Enable);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            return DataFactory.C_Report_MPCertificationGetBasicInfo(CompanyPublicId, CertificationType);
        }

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> C_Report_MPCertificationGetSpecificCert(string ProviderPublicId)
        {
            return DataFactory.C_Report_MPCertificationGetSpecificCert(ProviderPublicId);
        }

        #endregion

        #region SelectionProcess Report
        public ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetByIdProviderDetail(string ProjectPublicId, string CustomerPublicId, string ProviderPublicId)
        {
            return DataFactory.ProjectGetByIdProviderDetail(ProjectPublicId, CustomerPublicId, ProviderPublicId);
        }
        #endregion

    }
}
