using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.DAL.Controller
{
    internal class ThirdKnowledgeDataFactory
    {
        public ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData GetThirdKnowledgeInstance()
        {
            Type typetoreturn = Type.GetType("ProveedoresOnLine.ThirdKnowledge.DAL.MySQLDAO.ThirdKnowledge_MySqlDao,ProveedoresOnLine.ThirdKnowledge");
            ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData oRetorno = (ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
