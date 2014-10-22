using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.Models.Form
{
    public class FormModel
    {
        public string FormPublicId { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<StepModel> RelatedStep { get; set; }
    }
}
