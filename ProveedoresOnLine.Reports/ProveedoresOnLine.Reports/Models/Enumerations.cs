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
        }

        #endregion
    }
}
