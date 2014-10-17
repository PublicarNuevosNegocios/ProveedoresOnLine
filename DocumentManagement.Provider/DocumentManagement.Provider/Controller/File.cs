using DocumentManagement.Provider.Models;
using FileRepository.Manager;
using FileRepository.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Controller
{
    internal class File
    {        
        private FileRepository.Manager.FileRepository FileRepositoryInstance
        {
            get
            {
                if (oFileRepositoryInstance == null)
                {
                    oFileRepositoryInstance = (new FileRepositoryFactory()).GetFileRepository(DocumentManagement.Provider.Models.Constants.C_Settings_File_FileModuleName);
                    oFileRepositoryInstance.OperationError += new FileRepository.Manager.FileRepository.DOperationError(OperationError);
                    oFileRepositoryInstance.OperationFinish += new FileRepository.Manager.FileRepository.DOperationFinish(OperationFinish);
                }
                return oFileRepositoryInstance;
            }
        }
        private FileRepository.Manager.FileRepository oFileRepositoryInstance;

        //remote folder
        public string RemoteFolder { get; set; }

        //files to upload location
        public List<string> FilesToUpload { get; set; }

        //eval end process file
        public bool EndUpload
        {
            get
            {
                if (FileRepositoryInstance.CurrentOperations == null)
                {
                    return false;
                }
                else
                {
                    return !FileRepositoryInstance.CurrentOperations.Any(x => x.ActionResult == enumActionResult.NotStart);
                }
            }
        }

        public string StartUpload()
        {
            //fill file info to load
            FileRepositoryInstance.CurrentOperations = new List<FileModel>();
            FilesToUpload.All(OriginFile =>
            {
                FileRepositoryInstance.CurrentOperations.Add(
                    new FileModel()
                    {
                        FilePathLocalSystem = OriginFile,
                        FilePathRemoteSystem = RemoteFolder.TrimEnd('\\') + "\\" + OriginFile.Substring(OriginFile.LastIndexOf("\\")).TrimStart('\\'),
                        Operation = enumOperation.UploadFile
                    });

                return true;
            });
            //start load
            FileRepositoryInstance.StartOperation(false);

            return null;
        }

        #region Eventos del repositorio

        private void OperationFinish(FileModel FileDescription)
        {

        }

        private void OperationError(FileModel FileDescription, Exception Error)
        {

        }

        #endregion       
    }
}
