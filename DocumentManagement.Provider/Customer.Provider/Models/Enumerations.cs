using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Provider.Models
{
    public enum enumIdentificationType
    {
        CedulaCiudadania = 101,
        Nit = 102
    }

    public enum enumProcessStatus
    {
        New = 201,
        FullRegister = 202,
        AgentDocumentaryReviewer = 203,
        CertificationStart = 204,
        CertificationEnds = 205
    }

    public enum enumProviderInfoType
    { }

    public enum enumProviderCustomerInfoType
    { }
}
