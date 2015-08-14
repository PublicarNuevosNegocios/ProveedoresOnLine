using Microsoft.Reporting.WebForms;
using ProveedoresOnLine.SurveyModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.Controller
{
    public class ReportModule
    {
        #region ReportsSurveyDetail

        public static Tuple<byte[], string, string> CP_SurveyReportDetail(int ReportType, string FormatType, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            switch (ReportType)
            {
                case ((int)ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyReport):
                    localReport.ReportPath = FilePath;
                    localReport.SetParameters(ReportData);
                    break;
                default:
                    break;
            }
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                       "  <PageWidth>8.5in</PageWidth>" +
                       "  <PageHeight>11in</PageHeight>" +
                       "  <MarginTop>0.5in</MarginTop>" +
                       "  <MarginLeft>1in</MarginLeft>" +
                       "  <MarginRight>1in</MarginRight>" +
                       "  <MarginBottom>0.5in</MarginBottom>" +
                       "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = localReport.Render(
                FormatType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
        }

        #endregion

        #region ReportSurveyGetAllByCustomer

        public static List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId)
        {
            List<SurveyModule.Models.SurveyModel> oSurveyModel = oGetAllSurveyByCustomer(CustomerPublicId);
            if (oSurveyModel != null)
            {
                oSurveyModel.All(x =>
                {
                    List<string> Evaluators = x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == 1204003).Select(a => a.Value).ToList();
                    Evaluators = Evaluators.GroupBy(Y => Y).Select(grp => grp.First()).ToList();

                    List<SurveyModel> oChildModel = new List<SurveyModel>();
                    Evaluators.All(y =>
                    {
                        SurveyModel oChild = SurveyGetByPrentUser(x.SurveyPublicId, y);
                        if (oChild != null) {
                            oChildModel.Add(oChild);
                        }
                        return true;
                    });

                    if (oChildModel.Count > 0)                    
                        x.ChildSurvey = oChildModel.Where(l => l != null).ToList();

                    return true;
                });
            }
            return oSurveyModel;
        }

        #region Data

        public static List<SurveyModule.Models.SurveyModel> oGetAllSurveyByCustomer(string CustomerPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.SurveyGetAllByCustomer(CustomerPublicId);
        }

        public static SurveyModel SurveyGetByPrentUser(string ParentSurveyPublicId, string User)
        {
            return DAL.Controller.ReportsDataController.Instance.SurveyGetByParentUser(ParentSurveyPublicId, User);
        }

        #endregion

        #endregion

        #region Gerencial Report

        public static Tuple<byte[], string, string> CP_GerencialReport(string FormatType, DataTable data, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            localReport.SetParameters(ReportData);

            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_GerencialReport";
            source.Value = data != null ? data : new DataTable();

            localReport.DataSources.Add(source);

            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                       "  <PageWidth>8.5in</PageWidth>" +
                       "  <PageHeight>11in</PageHeight>" +
                       "  <MarginTop>0.5in</MarginTop>" +
                       "  <MarginLeft>1in</MarginLeft>" +
                       "  <MarginRight>1in</MarginRight>" +
                       "  <MarginBottom>0.5in</MarginBottom>" +
                       "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                FormatType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_GerencialReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
        }

        #region Data

        public static Company.Models.Company.CompanyModel C_Report_MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public static List<ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel> C_Report_BlackListGetBasicInfo(string CompanyPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_BlackListGetBasicInfo(CompanyPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_MPContactGetBasicInfo(CompanyPublicId, ContactType);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_MPLegalGetBasicInfo(CompanyPublicId, LegalType);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_MPCustomerProviderGetTracking(CustomerPublicId, ProviderPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPFinancialGetLastyearInfoDeta(string ProviderPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_MPFinancialGetLastyearInfoDeta(ProviderPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_FinancialGetBasicInfo(CompanyPublicId, FinancialType, Enable);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> C_Report_MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_MPCertificationGetBasicInfo(CompanyPublicId, CertificationType);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> C_Report_MPCertificationGetSpecificCert(string ProviderPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.C_Report_MPCertificationGetSpecificCert(ProviderPublicId);
        }

        #endregion

        #endregion

        #region SelectionProcess Report

        public static Tuple<byte[], string, string> PJ_ProjectProviderReportAceptedDetail(string ProjectPublicId,string CustomerPublicId, string ProviderPublicId, string FormatType, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            //localReport.SetParameters(ReportData);
            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_GerencialReport";
            //source.Value = data;
            //proceso la data
            ProveedoresOnLine.ProjectModule.Models.ProjectModel objModel = ProjectGetByIdProviderDetail(ProjectPublicId, CustomerPublicId, ProviderPublicId);

            

            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                       "  <PageWidth>8.5in</PageWidth>" +
                       "  <PageHeight>11in</PageHeight>" +
                       "  <MarginTop>0.5in</MarginTop>" +
                       "  <MarginLeft>1in</MarginLeft>" +
                       "  <MarginRight>1in</MarginRight>" +
                       "  <MarginBottom>0.5in</MarginBottom>" +
                       "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = localReport.Render(
                FormatType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return Tuple.Create(renderedBytes, mimeType, ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SelectionProcess + "_" + DateTime.Now.ToString("yyyyMMddHHmm")+ "." + FormatType);
        }
        
        #region Data
        public static ProveedoresOnLine.ProjectModule.Models.ProjectModel ProjectGetByIdProviderDetail(string ProjectPublicId, string CustomerPublicId, string ProviderPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.ProjectGetByIdProviderDetail(ProjectPublicId, CustomerPublicId, ProviderPublicId);
        }
        #endregion
        #endregion
    }
}
