﻿using Microsoft.Reporting.WebForms;
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
    }
}
