﻿using Microsoft.Reporting.WebForms;
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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
        }

        #endregion

        #region ReportSurveyGetAllByCustomer

        public static List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId)
        {
            List<SurveyModule.Models.SurveyModel> oSurveyParentModel = DAL.Controller.ReportsDataController.Instance.SurveyGetAllByCustomer(CustomerPublicId);

            if (oSurveyParentModel != null)
            {
                oSurveyParentModel.All(x =>
                    {
                        List<string> EvaluatorsList = x.SurveyInfo.Where(inf => inf.ItemInfoType.ItemId == 1204003).Select(inf => inf.Value).ToList();
                        List<string> Evaluators = EvaluatorsList.GroupBy(y => y).Select(grp => grp.First()).ToList();
                        x.ChildSurvey = new List<SurveyModel>();
                        Evaluators.All(ev =>
                            {
                                x.ChildSurvey.Add(DAL.Controller.ReportsDataController.Instance.SurveyGetByParentUser(x.SurveyPublicId, ev));
                                return true;
                            });

                        return true;
                    });
            }
            return oSurveyParentModel;
        }

        #endregion

        #region ReportSurveyEvaluatorDetail
        public static Tuple<byte[], string, string> SV_EvaluatorDetailReport(string FormatType, DataTable data, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            localReport.SetParameters(ReportData);

            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_EvaluatorDetailReport";
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
                       "  <MarginLeft>0.8in</MarginLeft>" +
                       "  <MarginRight>0.8in</MarginRight>" +
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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyEvaluatorDetailReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
        }

        #endregion ReportSurveyEvaluatorDetail

        #region Gerencial Report
        public static Tuple<byte[], string, string> CP_GerencialReport(string FormatType, DataTable data, DataTable data2, DataTable data3, DataTable data4, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            localReport.SetParameters(ReportData);

            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_GerencialReport";
            source.Value = data != null ? data : new DataTable();
            localReport.DataSources.Add(source);

            ReportDataSource source2 = new ReportDataSource();
            source2.Name = "DS_GerencialReport_Contact";
            source2.Value = data2 != null ? data2 : new DataTable();
            localReport.DataSources.Add(source2);

            ReportDataSource source3 = new ReportDataSource();
            source3.Name = "DS_GerencialReport_Terceros";
            source3.Value = data3 != null ? data3 : new DataTable();
            localReport.DataSources.Add(source3);

            ReportDataSource source4 = new ReportDataSource();
            source4.Name = "DS_GerencialReport_CalificationInfo";
            source4.Value = data4 != null ? data4 : new DataTable();
            localReport.DataSources.Add(source4);


            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                       "  <PageWidth>8.5in</PageWidth>" +
                       "  <PageHeight>11in</PageHeight>" +
                       "  <MarginTop>0.5in</MarginTop>" +
                       "  <MarginLeft>0.8in</MarginLeft>" +
                       "  <MarginRight>0.8in</MarginRight>" +
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
            if (FormatType == "Excel") { FormatType = "xls"; }
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

        public static Tuple<byte[], string, string> PJ_SelectionProcessReport(DataTable data, List<ReportParameter> ReportData, string FormatType, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = @"" + FilePath + "PJ_Report_SelectionProcess.rdlc";
            localReport.SetParameters(ReportData);

            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_SelectionProcessReport";
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
                       "  <MarginLeft>0.8in</MarginLeft>" +
                       "  <MarginRight>0.8in</MarginRight>" +
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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SelectionProcess + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);

        }


        public static Tuple<byte[], string, string> PJ_SelectionProcessReportDetail(DataTable DtHSEQ, DataTable DtExperiences, DataTable DtFinancial, DataTable DtLegal, DataTable DtTotal, List<ReportParameter> ReportData, string FormatType, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = @"" + FilePath + "PJ_Report_SelectionProcessDetail.rdlc";
            localReport.SetParameters(ReportData);

            ReportDataSource sourceDtHSEQ = new ReportDataSource();
            sourceDtHSEQ.Name = "DS_Selection_HSEQ";
            sourceDtHSEQ.Value = DtHSEQ != null ? DtHSEQ : new DataTable();
            localReport.DataSources.Add(sourceDtHSEQ);

            ReportDataSource sourceDtExperiences = new ReportDataSource();
            sourceDtExperiences.Name = "DS_Selection_Experience";
            sourceDtExperiences.Value = DtExperiences != null ? DtExperiences : new DataTable();
            localReport.DataSources.Add(sourceDtExperiences);

            ReportDataSource sourceDtFinancial = new ReportDataSource();
            sourceDtFinancial.Name = "DS_Selection_Financial";
            sourceDtFinancial.Value = DtFinancial != null ? DtFinancial : new DataTable();
            localReport.DataSources.Add(sourceDtFinancial);

            ReportDataSource sourceDtLegal = new ReportDataSource();
            sourceDtLegal.Name = "DS_Selection_Legal";
            sourceDtLegal.Value = DtLegal != null ? DtLegal : new DataTable();
            localReport.DataSources.Add(sourceDtLegal);

            ReportDataSource sourceDtTotal = new ReportDataSource();
            sourceDtTotal.Name = "DS_Selection_TotalArea";
            sourceDtTotal.Value = DtTotal != null ? DtTotal : new DataTable();
            localReport.DataSources.Add(sourceDtTotal);

            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                       "  <PageWidth>8.5in</PageWidth>" +
                       "  <PageHeight>11in</PageHeight>" +
                       "  <MarginTop>0.5in</MarginTop>" +
                       "  <MarginLeft>0.8in</MarginLeft>" +
                       "  <MarginRight>0.8in</MarginRight>" +
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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SelectionProcess + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
        }


        #endregion

        #region SVGeneralReport
        public static Tuple<byte[], string, string> SV_GeneralReport(DataTable data, DataTable data2, List<ReportParameter> ReportData, string FormatType, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = @"" + FilePath + "SV_Report_GeneralInfo.rdlc";
            localReport.SetParameters(ReportData);

            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_SurveyGeneralInfo";
            source.Value = data != null ? data : new DataTable();
            localReport.DataSources.Add(source);

            ReportDataSource source2 = new ReportDataSource();
            source2.Name = "DS_SurveyGeneralInfoAreas";
            source2.Value = data2 != null ? data2 : new DataTable();
            localReport.DataSources.Add(source2);

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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyGeneralReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);

        }

        #endregion

        #region ThirdKnowledgeReports

        public static Tuple<byte[], string, string> TK_QueryReport(string FormatType, DataTable data_rst, DataTable data_dce, DataTable data_fnc, DataTable data_psp, DataTable data_snc, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();

            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            localReport.SetParameters(ReportData);

            Microsoft.Reporting.WebForms.ReportDataSource  src_rst = new Microsoft.Reporting.WebForms.ReportDataSource();
            src_rst.Name = "DataSet_rst";
            src_rst.Value = data_rst != null ? data_rst : new DataTable();   
            Microsoft.Reporting.WebForms.ReportDataSource  src_dce = new Microsoft.Reporting.WebForms.ReportDataSource();
            src_dce.Name = "DataSet_dce";
            src_dce.Value = data_dce != null ? data_dce : new DataTable();
            Microsoft.Reporting.WebForms.ReportDataSource src_fnc = new Microsoft.Reporting.WebForms.ReportDataSource();
            src_fnc.Name = "DataSet_fnc";
            src_fnc.Value = data_fnc != null ? data_fnc : new DataTable();
            Microsoft.Reporting.WebForms.ReportDataSource src_psp = new Microsoft.Reporting.WebForms.ReportDataSource();
            src_psp.Name = "DataSet_psp";
            src_psp.Value = data_psp != null ? data_psp : new DataTable();
            Microsoft.Reporting.WebForms.ReportDataSource src_snc = new Microsoft.Reporting.WebForms.ReportDataSource();
            src_snc.Name = "DataSet_snc";
            src_snc.Value = data_snc != null ? data_snc : new DataTable();

            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_ThirdKnowledgeReport";

            localReport.DataSources.Add(src_rst);
            localReport.DataSources.Add(src_dce);
            localReport.DataSources.Add(src_fnc);
            localReport.DataSources.Add(src_psp);
            localReport.DataSources.Add(src_snc);

            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +

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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_ThirdKnowledgeQueryReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);

        }

        public static Tuple<byte[], string, string> TK_QueryDetailReport(string FormatType, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            localReport.SetParameters(ReportData);

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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_ThirdKnowledgeQueryDetailReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);

        }
        #endregion

        #region CustomerProviderReport

        public static List<ProveedoresOnLine.Reports.Models.Reports.CustomerProviderReportModel> R_ProviderCustomerReport(string CustomerPublicId)
        {
            return ProveedoresOnLine.Reports.DAL.Controller.ReportsDataController.Instance.R_ProviderCustomerReport(CustomerPublicId);
        }

        public static Tuple<byte[], string, string> GIBlackList_QueryReport(string FormatType, DataTable data, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            localReport.SetParameters(ReportData);

            ReportDataSource source = new ReportDataSource();
            source.Name = "DataSet_BlackListReport";
            source.Value = data != null ? data : new DataTable();
            localReport.DataSources.Add(source);

            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                       "  <PageWidth>11.69in</PageWidth>" +
                       "  <PageHeight>8.27in</PageHeight>" +
                       "  <MarginTop>0.7874in</MarginTop>" +
                       "  <MarginLeft>0.7874in</MarginLeft>" +
                       "  <MarginRight>0.7874in</MarginRight>" +
                       "  <MarginBottom>0.7874in</MarginBottom>" +
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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_GIBlackListQueryReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);

        }


        #endregion

        #region FinancialReport

        public static Tuple<byte[], string, string> F_FinancialReport(string FormatType, DataTable data, DataTable data2, DataTable data3, DataTable data4, DataTable data5, List<ReportParameter> ReportData, string FilePath)
        {
            LocalReport localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.ReportPath = FilePath;
            localReport.SetParameters(ReportData);

            ReportDataSource source = new ReportDataSource();
            source.Name = "DS_FinancialReport";
            source.Value = data != null ? data : new DataTable();
            localReport.DataSources.Add(source);

            ReportDataSource source2 = new ReportDataSource();
            source2.Name = "DS_FinancialReport_Liquidity";
            source2.Value = data2 != null ? data2 : new DataTable();
            localReport.DataSources.Add(source2);

            ReportDataSource souce3 = new ReportDataSource();
            souce3.Name = "DS_GerencialReport";
            souce3.Value = data3 != null ? data3 : new DataTable();
            localReport.DataSources.Add(souce3);

            ReportDataSource source4 = new ReportDataSource();
            source4.Name = "DS_GerencialReport_Contact";
            source4.Value = data4 != null ? data4 : new DataTable();
            localReport.DataSources.Add(source4);

            ReportDataSource source5 = new ReportDataSource();
            source5.Name = "DS_GerencialReport_Terceros";
            source5.Value = data5 != null ? data5 : new DataTable();
            localReport.DataSources.Add(source5);

            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
                       "<DeviceInfo>" +
                       "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                       "  <PageWidth>8.5in</PageWidth>" +
                       "  <PageHeight>11in</PageHeight>" +
                       "  <MarginTop>0.5in</MarginTop>" +
                       "  <MarginLeft>0.8in</MarginLeft>" +
                       "  <MarginRight>0.8in</MarginRight>" +
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
            if (FormatType == "Excel") { FormatType = "xls"; }
            return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_FinancialReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
        }

        #endregion

        #region BlackList
            public static Tuple<byte[], string, string> TK_GIBlackListQueryReport(string FormatType, DataTable data_rst, DataTable data_dce, DataTable data_fnc, DataTable data_psp, DataTable data_na, List<ReportParameter> ReportData, string FilePath)
            {
                LocalReport localReport = new LocalReport();
                localReport.EnableExternalImages = true;
                localReport.ReportPath = FilePath;
                localReport.SetParameters(ReportData);

                Microsoft.Reporting.WebForms.ReportDataSource src_rst = new Microsoft.Reporting.WebForms.ReportDataSource();
                src_rst.Name = "DataSet_rst";
                src_rst.Value = data_rst != null ? data_rst : new DataTable();

                Microsoft.Reporting.WebForms.ReportDataSource src_dce = new Microsoft.Reporting.WebForms.ReportDataSource();
                src_dce.Name = "DataSet_dce";
                src_dce.Value = data_dce != null ? data_dce : new DataTable();

                Microsoft.Reporting.WebForms.ReportDataSource src_fnc = new Microsoft.Reporting.WebForms.ReportDataSource();
                src_fnc.Name = "DataSet_fnc";
                src_fnc.Value = data_fnc != null ? data_fnc : new DataTable();

                Microsoft.Reporting.WebForms.ReportDataSource src_psp = new Microsoft.Reporting.WebForms.ReportDataSource();
                src_psp.Name = "DataSet_psp";
                src_psp.Value = data_psp != null ? data_psp : new DataTable();

                Microsoft.Reporting.WebForms.ReportDataSource src_na = new ReportDataSource();
                src_na.Name = "DataSet_na";
                src_na.Value = data_na != null ? data_na : new DataTable();

                ReportDataSource source = new ReportDataSource();
                source.Name = "DS_GIBlackListreport";
                localReport.DataSources.Add(src_rst);
                localReport.DataSources.Add(src_dce);
                localReport.DataSources.Add(src_fnc);
                localReport.DataSources.Add(src_psp);
                localReport.DataSources.Add(src_na);
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                           "<DeviceInfo>" +
                           "  <OutputFormat>" + FormatType + "</OutputFormat>" +
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
                if (FormatType == "Excel") { FormatType = "xls"; }
                return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_GIBlackListQueryReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
            }
            public static Tuple<byte[], string, string> TK_GIBlackListQueryDetailReport(string FormatType, List<ReportParameter> ReportData, string FilePath)
            {
                LocalReport localReport = new LocalReport();
                localReport.EnableExternalImages = true;
                localReport.ReportPath = FilePath;
                localReport.SetParameters(ReportData);

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
                if (FormatType == "Excel") { FormatType = "xls"; }
                return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_GIBlackListDetailQueryReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);

            }
        #endregion

        #region CalificationReport

            public static Tuple<byte[], string, string> CP_CalificationReport(string FormatType, DataTable LegalData, DataTable CommercialData, DataTable FinancialData, DataTable BalanceData, DataTable CertificationData, DataTable ConfigValidateData, List<ReportParameter> parameters,  string FilePath) 
            {
                LocalReport localReport = new LocalReport();
                localReport.EnableExternalImages = true;
                localReport.ReportPath = FilePath;
                localReport.SetParameters(parameters);

                ReportDataSource source = new ReportDataSource();
                source.Name = "DS_CalificationReport_LegalInfo";
                source.Value = LegalData != null ? LegalData : new DataTable();
                localReport.DataSources.Add(source);

                ReportDataSource source2 = new ReportDataSource();
                source2.Name = "DS_CalificationReport_FinancialInfo";
                source2.Value = FinancialData != null ? FinancialData : new DataTable();
                localReport.DataSources.Add(source2);

                ReportDataSource source3 = new ReportDataSource();
                source3.Name = "DS_CalificationReport_CommercialInfo";
                source3.Value = CommercialData != null ? CommercialData : new DataTable();
                localReport.DataSources.Add(source3);

                ReportDataSource source4 = new ReportDataSource();
                source4.Name = "DS_CalificationReport_Certification";
                source4.Value = CertificationData != null ? CertificationData : new DataTable();
                localReport.DataSources.Add(source4);

                ReportDataSource source5 = new ReportDataSource();
                source5.Name = "DS_CalificationReport_BalanceInfo";
                source5.Value = BalanceData != null ? BalanceData : new DataTable();
                localReport.DataSources.Add(source5);

                ReportDataSource source6 = new ReportDataSource();
                source6.Name = "DS_CalificationReport_ValidateInfo";
                source6.Value = ConfigValidateData != null ? ConfigValidateData : new DataTable();
                localReport.DataSources.Add(source6);

                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                           "<DeviceInfo>" +
                           "  <OutputFormat>" + FormatType + "</OutputFormat>" +
                           "  <PageWidth>8.5in</PageWidth>" +
                           "  <PageHeight>11in</PageHeight>" +
                           "  <MarginTop>0.5in</MarginTop>" +
                           "  <MarginLeft>0.8in</MarginLeft>" +
                           "  <MarginRight>0.8in</MarginRight>" +
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
                if (FormatType == "Excel") { FormatType = "xls"; }
                return Tuple.Create(renderedBytes, mimeType, "Proveedores_" + ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_CalificationReport + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "." + FormatType);
            }
        #endregion

    }
}
