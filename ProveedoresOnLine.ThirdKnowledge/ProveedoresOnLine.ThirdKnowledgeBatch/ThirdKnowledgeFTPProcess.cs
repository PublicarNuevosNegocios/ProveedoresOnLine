using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledgeBatch
{
    public static class ThirdKnowledgeFTPProcess
    {
        public static void StartProcess()
        {
            try
            {
                //Get query masive, in process, file name
                //Connect to FTP for the ftp file
                ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetQueriesInProgress();

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
