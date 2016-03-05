using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcessBatch.Models
{
    public enum enumProviderStatus
    {
        CreacionNacional = 902001,
        ProcesoNacional = 902002,
        ValidadoBasicaNacional = 902004,
        ValidadoCompletaNacional = 902005,
        CreacionExtranjero = 902006,
        ProcesoExtranjero = 902007,
        ValidadoCompletaExtranjero = 902008,
        ImposibleContactarNacional = 902009,
        ImposibleContactarExtranjero = 902010,
        InactivoNacional = 902011,
        InactivoExtranjero = 902012,
    }

    public enum enumLegalDesignationsInfoType
    {
        //Designations
        CD_PartnerName = 607001,
        CD_PartnerIdentificationNumber = 607002,
        CD_PartnerRank = 607003,
    }

    public enum enumCompanyInfoType
    {
        UpdateAlert = 203012,
        Alert = 203008,
    }

    public enum enumBlackList
    {
        BL_ShowAlert = 1101001,
        BL_DontShowAlert = 1101002,
    }
}
