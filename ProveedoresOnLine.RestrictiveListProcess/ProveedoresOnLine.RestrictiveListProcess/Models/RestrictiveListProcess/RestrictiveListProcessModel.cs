using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess
{
    public class RestrictiveListProcessModel
    {
        //Table Model
        public int BlackListProcessId { get; set; }
        public string FilePath { get; set; }
        public bool ProcessStatus { get; set; }
        public bool IsSuccess { get; set; }
        public string ProviderStatus { get; set; }
        public bool Enable { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime CreateDate { get; set; }

        public List<ProviderModel> RelatedProvider { get; set; }

        public List<string> oStrListProviderStatus = new List<string>();
        public List<string> strListProviderStatus
        {
            get
            {
                if (oStrListProviderStatus.Count == 0)
                {
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.CreacionNacional));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.CreacionExtranjero));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ImposibleContactarExtranjero));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ImposibleContactarNacional));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.InactivoExtranjero));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.InactivoNacional));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ProcesoExtranjero));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ProcesoNacional));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ValidadoBasicaNacional));
                    //oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ValidadoCompletaExtranjero));
                    oStrListProviderStatus.Add(Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ValidadoCompletaNacional));
                }
                return oStrListProviderStatus;
            }
        }
    }
}
