using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.Controller
{
    public class Reports
    {
        #region Reports 
        public static Tuple<byte[], string, string> PrintReport(int ReportType, string FormatType, List<ReportParameter> ReportData)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            string strFile = ProveedoresOnLine.Reports.Models.Util.InternalSettings.Instance[ProveedoresOnLine.Reports.Models.Constants.R_Path].Value;
            switch (ReportType)
            {
                case ((int)ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyReport):
                    localReport.ReportPath = strFile.Replace("\n", string.Empty).Trim() + "SV_Report_SurveyDetail.rdlc";
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

        #region Gerencial Report

        public static Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> BlackListGetByCompanyPublicId(string CompanyPublicId)
        {
            return DAL.Controller.ReportsDataController.Instance.BlackListGetByCompanyPublicId(CompanyPublicId);
        }

        #endregion

        #endregion
    }
}
