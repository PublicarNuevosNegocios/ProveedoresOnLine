using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.Models
{
    public class SanofiContableInfoModel : SanofiGeneralInfoModel
    {
        public string BankPassword { get; set; }

        public string BankCountNumber { get; set; }

        public Int64 CountType { get; set; }

        public string IBAN { get; set; }

        public string AssociatedCount { get; set; }
        
        public string PayCondition { get; set; }

        public string PayWay { get; set; }
    }
}
