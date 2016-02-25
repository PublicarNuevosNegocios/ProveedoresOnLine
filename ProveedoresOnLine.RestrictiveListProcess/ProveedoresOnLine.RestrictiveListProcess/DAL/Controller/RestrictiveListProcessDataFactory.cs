using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.DAL.Controller
{
    internal class RestrictiveListProcessDataFactory
    {

         public ProveedoresOnLine.RestrictiveListProcess.Interfaces.IRestrictiveListProcess GetReportsInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.RestrictiveListProcess.DAL.MySQLDAO.RestrictiveListProcess_MySqlDao, ProveedoresOnLine.RestrictiveListProcess");
            ProveedoresOnLine.RestrictiveListProcess.Interfaces.IRestrictiveListProcess oRetorno = (ProveedoresOnLine.RestrictiveListProcess.Interfaces.IRestrictiveListProcess)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }

    }
}
