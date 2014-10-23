using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class FormSearchModel
    {
        public int TotalRows { get; set; }

        public List<DocumentManagement.Customer.Models.Form.FormModel> RelatedForm { get; set; }
    }
}
