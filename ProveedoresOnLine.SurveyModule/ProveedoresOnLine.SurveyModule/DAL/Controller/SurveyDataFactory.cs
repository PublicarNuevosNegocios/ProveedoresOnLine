using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyModule.DAL.Controller
{
    internal class SurveyDataFactory
    {
        public ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData GetSurveyInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.SurveyModule.DAL.MySQLDAO.Survey_MySqlDao,ProveedoresOnLine.SurveyModule");
            ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData oRetorno = (ProveedoresOnLine.SurveyModule.Interfaces.ISurveyData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
