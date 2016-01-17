using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.AsociateProvider.Client.Models
{
    public class HomologateModel
    {
        public int HologateId { get; set; }

        public  CatalogModel HomologateType { get; set; }

        public CatalogModel Source { get; set; }

        public CatalogModel HomologateType { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
