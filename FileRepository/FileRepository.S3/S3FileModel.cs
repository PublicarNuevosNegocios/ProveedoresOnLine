using FileRepository.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRepository.S3
{
    public class S3FileModel
    {
        public FileModel RelatedFile { get; set; }

        public long FileSize { get; set; }
    }
}
