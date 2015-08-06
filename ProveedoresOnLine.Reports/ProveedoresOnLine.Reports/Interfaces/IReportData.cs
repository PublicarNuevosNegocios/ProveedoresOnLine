using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Reports.Interfaces
{
    internal interface IReportData
    {
        #region Reports

        List<SurveyModule.Models.SurveyModel> SurveyGetAllByCustomer(string CustomerPublicId);

        #endregion
    }
}
