using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.DAL.Controller
{
    internal class ProjectDataFactory
    {
        public ProveedoresOnLine.ProjectModule.Interfaces.IProjectData GetProjectInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.ProjectModule.DAL.MySQLDAO.Project_MySqlDao,ProveedoresOnLine.ProjectModule");
            ProveedoresOnLine.ProjectModule.Interfaces.IProjectData oRetorno = (ProveedoresOnLine.ProjectModule.Interfaces.IProjectData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
