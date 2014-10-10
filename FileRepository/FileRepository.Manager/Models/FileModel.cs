using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRepository.Manager.Models
{
    public class FileModel
    {
        private Guid oInternalFileId = Guid.NewGuid();

        public Guid InternalFileId { get { return oInternalFileId; } }

        public enumOperation Operation { get; set; }

        public enumActionResult ActionResult { get; set; }

        public string FilePathLocalSystem { get; set; }

        public string FilePathRemoteSystem { get; set; }

        public Uri UploadedFile { get; set; }

        public Uri PublishFile { get; set; }

        public int ProgressProcess { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }
    }
}
