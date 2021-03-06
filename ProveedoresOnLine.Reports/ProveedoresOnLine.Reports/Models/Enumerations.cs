﻿namespace ProveedoresOnLine.Reports.Models
{
    public class Enumerations
    {
        #region Util
        public enum enumCategoryInfoType
        {
            //Format Report Type
            Excel = 120001,
            PDF = 120002,
            TXT = 120003,
            CSV = 120004,
        }
        #endregion

        #region Report

        public enum enumReportType
        {
            RP_SurveyReport = 1501001,
            RP_GerencialReport = 1501002,
            RP_SelectionProcess=1501003,
            RP_SurveyEvaluatorDetailReport = 1501004,
            RP_SurveyGeneralReport = 1501005,
            RP_ThirdKnowledgeQueryReport = 1501006,
            RP_ThirdKnowledgeQueryDetailReport = 1501007,
            RP_GIBlackListQueryReport = 1501008,
            RP_FinancialReport = 1501009,
            RP_GIBlackListDetailQueryReport = 1501010,
            RP_CalificationReport=1501011,
        }

        #endregion
    }
}
