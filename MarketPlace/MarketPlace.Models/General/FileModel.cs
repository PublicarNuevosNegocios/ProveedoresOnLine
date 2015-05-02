using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public class FileModel
    {
        public string FileObjectId { get; set; }

        public string FileName { get; set; }

        public string ServerUrl { get; set; }

        public string FileExtension
        {
            get
            {
                string oReturn = "pdf";
                if (!string.IsNullOrEmpty(FileName))
                {
                    oReturn = FileName.Split('.').DefaultIfEmpty("pdf").LastOrDefault();
                }
                return oReturn;
            }
        }
    }
}
