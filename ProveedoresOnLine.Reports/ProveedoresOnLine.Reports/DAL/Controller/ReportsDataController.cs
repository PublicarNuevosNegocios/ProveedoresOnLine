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
        public SurveyModel SurveyGetById(string SurveyPublicId)
        {
            return DataFactory.SurveyGetById(SurveyPublicId);
        }
        #endregion

        #region Gerencial Report

        public Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DataFactory.MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public List<GenericItemModel> BlackListGetByCompanyPublicId(string CompanyPublicId)
        {
            return DataFactory.BlackListGetByCompanyPublicId(CompanyPublicId);
        }

        #endregion
    }
}
