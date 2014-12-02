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
                oReturn.Add(new BackOffice.Models.General.FileUploadModel()
                {
                    name = System.Web.HttpContext.Current.Request.Files[0].FileName,
                    ServerName = "http://amazon.com/archivo.txt",
                });
            }

            return oReturn;
        }
    }
}
