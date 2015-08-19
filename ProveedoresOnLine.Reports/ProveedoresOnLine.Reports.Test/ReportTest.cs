﻿using Microsoft.Reporting.WebForms;
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
            //proyecto Pj_Id-> 12B70122
            //publicar customer-> DA5C572E
            //el tiempo provider-> 123601D3
            Tuple<byte[], string, string> report = ProveedoresOnLine.Reports.Controller.ReportModule.PJ_ProjectProviderReportAceptedDetail("12B70122", "DA5C572E", "123601D3", "PDF", @"C:\PublicarPO\ProveedoresOnLine.Reports\ProveedoresOnLine.Reports\Reports\");
        }
        #endregion
    }
}
