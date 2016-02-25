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
}
