using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.Models.Form
{
    public class StepModel
    {
        public int StepId { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<FieldModel> RelatedField { get; set; }

        public bool ShowChanges { get; set; }
    }
}
