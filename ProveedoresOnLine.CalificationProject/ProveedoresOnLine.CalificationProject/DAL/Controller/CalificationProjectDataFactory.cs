using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.DAL.Controller
{
    internal class CalificationProjectDataFactory
    {
        public CalificationProject.Interfaces.ICalificationProjectData GetCalificationProjectInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.CalificationProject.DAL.MySqlDAO.CalificationProject_MySqlDao,ProveedoresOnLine.CalificationProject");
            ProveedoresOnLine.CalificationProject.Interfaces.ICalificationProjectData oRetorno = (ProveedoresOnLine.CalificationProject.Interfaces.ICalificationProjectData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
