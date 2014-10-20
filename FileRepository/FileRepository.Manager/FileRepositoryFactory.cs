using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRepository.Manager
{
    public class FileRepositoryFactory
    {
        public FileRepository GetFileRepository(string FileModule)
        {
            //get assembli info
            string AssemblyInfo = Models.InternalSettings.Instance
                [Models.Constants.C_Settings_FileModule_AssemblyInfo.
                Replace("{{FileModule}}", FileModule)].Value.Replace(" ", "");

            Type typetoreturn = Type.GetType(AssemblyInfo);
            FileRepository oRetorno = (FileRepository)Activator.CreateInstance(typetoreturn);

            oRetorno.FileModule = FileModule;

            return oRetorno;
        }
    }
}
