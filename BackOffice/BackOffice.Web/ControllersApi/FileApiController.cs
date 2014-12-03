using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class FileApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.General.FileUploadModel> FileUpload
            (string CompanyPublicId)
        {
            List<BackOffice.Models.General.FileUploadModel> oReturn = new List<Models.General.FileUploadModel>();

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Length > 0)
            {
                //get folder
                string strFolder = System.Web.HttpContext.Current.Server.MapPath
                    (BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_File_TempDirectory].Value);

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                System.Web.HttpContext.Current.Request.Files.AllKeys.All(reqFile =>
                {
                    //get File
                    var UploadFile = System.Web.HttpContext.Current.Request.Files[reqFile];

                    if (UploadFile != null && !string.IsNullOrEmpty(UploadFile.FileName))
                    {
                        string strFile = strFolder.TrimEnd('\\') +
                            "\\CompanyFile_" +
                            CompanyPublicId + "_" +
                            DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                            UploadFile.FileName.Split('.').DefaultIfEmpty("pdf").LastOrDefault();

                        UploadFile.SaveAs(strFile);

                        //load file to s3
                        string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                            (strFile,
                            BackOffice.Models.General.InternalSettings.Instance
                                [BackOffice.Models.General.Constants.C_Settings_File_RemoteDirectory].Value +
                                CompanyPublicId + "\\");

                        //remove temporal file
                        if (System.IO.File.Exists(strFile))
                            System.IO.File.Delete(strFile);

                        oReturn.Add(new BackOffice.Models.General.FileUploadModel()
                        {
                            name = UploadFile.FileName,
                            ServerName = strRemoteFile,
                        });
                    }
                    return true;
                });
            }
            return oReturn;
        }
    }
}
