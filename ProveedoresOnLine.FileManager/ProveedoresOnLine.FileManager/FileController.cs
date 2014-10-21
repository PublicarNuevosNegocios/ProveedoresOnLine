using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.FileManager
{
    public class FileController
    {
        #region Properties

        private FileRepository.Manager.FileRepository FileRepositoryInstance
        {
            get
            {
                if (oFileRepositoryInstance == null)
                {
                    oFileRepositoryInstance = (new FileRepository.Manager.FileRepositoryFactory()).GetFileRepository(ProveedoresOnLine.FileManager.Constants.C_Settings_File_FileModuleName);
                    oFileRepositoryInstance.OperationError += new FileRepository.Manager.FileRepository.DOperationError(OperationError);
                    oFileRepositoryInstance.OperationFinish += new FileRepository.Manager.FileRepository.DOperationFinish(OperationFinish);
                }
                return oFileRepositoryInstance;
            }
        }
        private FileRepository.Manager.FileRepository oFileRepositoryInstance;

        //remote folder
        internal string RemoteFolder { get; set; }

        //files to upload location
        internal List<string> FilesToUpload { get; set; }

        //eval end process file
        internal bool EndUpload
        {
            get
            {
                if (FileRepositoryInstance.CurrentOperations == null)
                {
                    return false;
                }
                else
                {
                    return !FileRepositoryInstance.CurrentOperations.Any(x => x.ActionResult == FileRepository.Manager.Models.enumActionResult.NotStart);
                }
            }
        }

        internal List<FileRepository.Manager.Models.FileModel> UploadedFiles
        {
            get
            {
                if (FileRepositoryInstance == null)
                    return null;
                else
                    return FileRepositoryInstance.CurrentOperations;
            }
        }

        #endregion

        #region Public Methods

        internal void StartUpload()
        {
            //fill file info to load
            FileRepositoryInstance.CurrentOperations = new List<FileRepository.Manager.Models.FileModel>();
            FilesToUpload.All(OriginFile =>
            {
                FileRepositoryInstance.CurrentOperations.Add(
                    new FileRepository.Manager.Models.FileModel()
                    {
                        FilePathLocalSystem = OriginFile,
                        FilePathRemoteSystem = RemoteFolder.TrimEnd('\\') + "\\" + OriginFile.Substring(OriginFile.LastIndexOf("\\")).TrimStart('\\'),
                        Operation = FileRepository.Manager.Models.enumOperation.UploadFile
                    });

                return true;
            });
            //start load
            FileRepositoryInstance.StartOperation(false);
        }

        #endregion

        #region Eventos del repositorio

        private void OperationFinish(FileRepository.Manager.Models.FileModel FileDescription)
        {

        }

        private void OperationError(FileRepository.Manager.Models.FileModel FileDescription, Exception Error)
        {

        }

        #endregion

        #region public static methods

        static public string LoadFile(string FilePath, string RemoteFolder)
        {
            FileController oLoader = new FileController()
            {
                FilesToUpload = new List<string>() { FilePath },
                RemoteFolder = RemoteFolder
            };

            oLoader.StartUpload();

            return oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == FilePath).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
        }

        #endregion
    }
}
