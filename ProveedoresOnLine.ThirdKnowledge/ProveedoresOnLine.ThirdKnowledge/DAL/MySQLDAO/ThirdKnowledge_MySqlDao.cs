using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.DAL.MySQLDAO
{
    internal class ThirdKnowledge_MySqlDao : ProveedoresOnLine.ThirdKnowledge.Interfaces.IThirdKnowledgeData
    {
        private ADO.Interfaces.IADO DataInstance;

        public ThirdKnowledge_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_POL_ThirdKnowledgeConnectionName);
        }
    }
}
