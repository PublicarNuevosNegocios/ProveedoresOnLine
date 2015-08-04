using Microsoft.Reporting.WebForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.Test
{
    [TestClass]
    public class ReportTest
    {
        #region Reporting
        [TestMethod]
        public void SV_Report_SurveyDetail()
        {
            List<ReportParameter> data = new List<ReportParameter>();
            data.Add(new ReportParameter("Nombre", "Alexander"));
            Tuple<byte[], string, string> report = ProveedoresOnLine.Reports.Controller.Reports.PrintReport((int)ProveedoresOnLine.Reports.Models.Enumerations.enumReportType.RP_SurveyReport, "PDF", data);
            data = null;
        }
        #endregion
    }
}
