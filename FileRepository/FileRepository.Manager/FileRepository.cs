using FileRepository.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileRepository.Manager
{
    public abstract class FileRepository
    {
        /// <summary>
        /// File module for the instance
        /// </summary>
        public string FileModule { get; set; }

        /// <summary>
        /// declare delegate to define exception event
        /// </summary>
        /// <param name="Error"></param>
        public delegate void DOperationError(FileModel FileDescription, Exception Error);

        /// <summary>
        /// delegate to define exception event
        /// </summary>
        public DOperationError OperationError { get; set; }

        /// <summary>
        /// declare delegate to define finish event
        /// </summary>
        /// <param name="Error"></param>
        public delegate void DOperationFinish(FileModel FileDescription);

        /// <summary>
        /// delegate to define finish event
        /// </summary>
        public DOperationFinish OperationFinish { get; set; }

        /// <summary>
        /// List operations in progress
        /// </summary>
        public List<FileModel> CurrentOperations { get; set; }

        /// <summary>
        /// Start operation sync
        /// </summary>
        public abstract void StartOperation(bool IsAsync);

        /// <summary>
        /// Get dictionary file params
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetFileParams()
        {
            Dictionary<string, string> oRetorno = new Dictionary<string, string>();

            XDocument xDoc = XDocument.Parse(Models.InternalSettings.Instance
                [Models.Constants.C_Settings_FileModule_Params.
                Replace("{{FileModule}}", FileModule)].Value);

            oRetorno = xDoc.Element("S3").Elements("key").ToDictionary(k => k.Attribute("name").Value, v => v.Value);

            return oRetorno;
        }

        /// <summary>
        /// get file process progress
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public int GetProgressProcess(Guid InternalFileId)
        {
            int oRetorno = -1;
            if (CurrentOperations != null)
            {
                oRetorno = CurrentOperations.
                    Where(x => x.InternalFileId == InternalFileId).
                    Select(x => x.ProgressProcess).
                    DefaultIfEmpty(-1).
                    FirstOrDefault();
            }
            return oRetorno;
        }
    }
}
