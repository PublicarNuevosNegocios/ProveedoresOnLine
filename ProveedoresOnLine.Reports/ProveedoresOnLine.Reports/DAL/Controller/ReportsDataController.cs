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

        #region report

        public List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId)
        {
            return DataFactory.SurveyGetAllByCustomer(CustomerPublicId);
        }

        public List<string> SurveyGetIdsChildrenByParent(string vParentPublicId)
        {
            return DataFactory.SurveyGetIdsChildrenByParent(vParentPublicId);
        }

        public SurveyModel SurveyGetById(string SurveyPublicId)
        {
            return DataFactory.SurveyGetById(SurveyPublicId);
        }
        #endregion

        #region Gerencial Report

        public Company.Models.Company.CompanyModel C_Report_MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.C_Report_MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public List<GenericItemModel> C_Report_BlackListGetByCompanyPublicId(string CompanyPublicId)
        {
            return DataFactory.C_Report_BlackListGetByCompanyPublicId(CompanyPublicId);
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

        public List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType)
        {
            return DataFactory.C_Report_MPFinancialGetBasicInfo(CompanyPublicId, FinancialType);
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
    }
}
