﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderExcelModel
    {
        public string RazonSocial { get; set; }
        public string IdentificationType { get; set; }
        public string IdentifiationNumber { get; set; }
        public string SearchType { get; set; }   
        public string ProviderPublicId { get; set; }
        public string BlackListSatatus { get; set; }        
    }
}
