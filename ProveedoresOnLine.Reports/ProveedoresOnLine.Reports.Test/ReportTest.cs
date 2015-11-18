using Microsoft.Reporting.WebForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.Test
{
    [TestClass]
    public class ReportTest
    {
        #region Survey Reports
        [TestMethod]
        public void SV_Report_SurveyDetail()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("currentCompanyName", "Alpina"));
            parameters.Add(new ReportParameter("currentCompanyTipoDni", "Nit"));
            parameters.Add(new ReportParameter("currentCompanyDni", "1235466879645"));
            parameters.Add(new ReportParameter("currentCompanyLogo", "http://www.industriaalimenticia.com/ext/resources/images/news/alpinalogo3.jpg"));
            parameters.Add(new ReportParameter("providerName", "NombreProveedor"));
            parameters.Add(new ReportParameter("providerTipoDni", "Nit"));
            parameters.Add(new ReportParameter("providerDni", "7894596126"));
            parameters.Add(new ReportParameter("remarks", "Remarks"));
            parameters.Add(new ReportParameter("actionPlan", "Action Plan"));
            parameters.Add(new ReportParameter("dateStart", "12/12/2015"));
            parameters.Add(new ReportParameter("dateEnd", "13/12/2015"));
            parameters.Add(new ReportParameter("average", "50"));
            parameters.Add(new ReportParameter("reportDate", "01/01/2016"));
            parameters.Add(new ReportParameter("responsible", "Alexander"));
            parameters.Add(new ReportParameter("author", "Autor Chino"));
            Tuple<byte[], string, string> report = ProveedoresOnLine.Reports.Controller.ReportModule.CP_SurveyReportDetail((int)ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyReport, "PDF", parameters, "");
            parameters = null;
        }

        [TestMethod]
        public void SV_Report_SurveyGetAllByCustomer()
        {
            List<SurveyModule.Models.SurveyModel> oModel = ProveedoresOnLine.Reports.Controller.ReportModule.SurveyGetAllByCustomer("DA5C572E");
        }

        #endregion

        #region GerencialReport

        [TestMethod]
        public void C_GerencialReport()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("CustomerName", "Representante de prueba"));

            DataTable data = new DataTable();

            Tuple<byte[], string, string> report =
                ProveedoresOnLine.Reports.Controller.ReportModule.CP_GerencialReport("PDF",
                                                                                     data,
                                                                                     parameters,
                                                                                     "C:\\Publicar Software\\ProveedoresOnLine\\ProveedoresOnLine.Reports\\ProveedoresOnLine.Reports.Test\\Reports\\C_Report_GerencialInfo.rdlc");
            parameters = null;
        }

        #endregion

        #region SelectionProcessReport
        [TestMethod]
        public void PJ_Report_SelectionProcess()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            //current User
            parameters.Add(new ReportParameter("reportGeneratedBy", "generado@mail.com"));
            //CurrentCompany
            parameters.Add(new ReportParameter("currentCompanyName", "Publicar SAS"));
            parameters.Add(new ReportParameter("currentCompanyTypeId", "NIT"));
            parameters.Add(new ReportParameter("currentCompanyId", "123456879"));
            parameters.Add(new ReportParameter("currentCompanyLogo", "http://proveedoresonline.s3-website-us-east-1.amazonaws.com/BackOffice/CompanyFile/DA5C572E/CompanyFile_DA5C572E_20150311090116.png"));
            //Header
            parameters.Add(new ReportParameter("PJ_Name", "Nombre del proyecto"));
            parameters.Add(new ReportParameter("PJ_Type", "tipo del proyecto"));
            parameters.Add(new ReportParameter("PJ_Date", "18/08/2015"));
            parameters.Add(new ReportParameter("PJ_Price", "2.000.000" + "COP"));
            parameters.Add(new ReportParameter("PJ_MinExperience", "1"));
            parameters.Add(new ReportParameter("PJ_InternalCodeProcess", "2"));
            parameters.Add(new ReportParameter("PJ_YearsExperince", "3"));
            parameters.Add(new ReportParameter("PJ_ActivityName", "Insumos de papeleria"));
            parameters.Add(new ReportParameter("PJ_AdjudicateNote", "Nota de adjudicación que jue adjudicada por Alex-JP"));
            parameters.Add(new ReportParameter("PJ_ResponsibleName", "responsable@mail.com"));
            //areas
            DataTable dtProvidersProject = new DataTable();
            dtProvidersProject.Columns.Add("providerName");
            dtProvidersProject.Columns.Add("TypeId");
            dtProvidersProject.Columns.Add("providerId");
            dtProvidersProject.Columns.Add("hsq");
            dtProvidersProject.Columns.Add("tecnica");
            dtProvidersProject.Columns.Add("financiera");
            dtProvidersProject.Columns.Add("legal");
            dtProvidersProject.Columns.Add("estado");
            DataRow rowProvider = dtProvidersProject.NewRow();
            //add provider info
            rowProvider["providerName"] = "El Tiempo";
            rowProvider["TypeId"] = "Provedor:";
            rowProvider["providerId"] = "459698654";
            rowProvider["hsq"] = "5 % no pasa Aprobado";
            rowProvider["tecnica"] = "pasa Aprobado";
            rowProvider["financiera"] = "3 % no pasa Aprobado";
            rowProvider["legal"] = "pasa Aprobado";
            rowProvider["estado"] = "Ajudicado";

            dtProvidersProject.Rows.Add(rowProvider);

            Tuple<byte[], string, string> report = ProveedoresOnLine.Reports.Controller.ReportModule.PJ_SelectionProcessReport(dtProvidersProject, parameters, "PDF", @"C:\PublicarPO\ProveedoresOnLine.Reports\ProveedoresOnLine.Reports\Reports\");
        }
        #endregion

        #region CustomerProviderReport

        [TestMethod]
        public void CustomerProviderReport()
        {
            List<ProveedoresOnLine.Reports.Models.Reports.CustomerProviderReportModel> oReturn = new List<Models.Reports.CustomerProviderReportModel>();

            oReturn = ProveedoresOnLine.Reports.Controller.ReportModule.R_ProviderCustomerReport("205974DF");

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        #endregion
    }
}
