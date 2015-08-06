using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.DAL.Controller
{
    internal class ReportsDataController : ProveedoresOnLine.Reports.Interfaces.IReportData
    {
        #region singleton instance
        private static ProveedoresOnLine.Reports.Interfaces.IReportData oInstance;
        internal static ProveedoresOnLine.Reports.Interfaces.IReportData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ReportsDataController();
                return oInstance;
            }
        }
        private ProveedoresOnLine.Reports.Interfaces.IReportData DataFactory;
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

        #endregion

    }
}
