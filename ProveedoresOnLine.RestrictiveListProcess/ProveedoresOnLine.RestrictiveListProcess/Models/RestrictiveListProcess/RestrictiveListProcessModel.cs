using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess
{
    public class RestrictiveListProcessModel
    {
        public UInt64 BlackListProcessId { get; set; }
        public string FilePath { get; set; }
        public bool ProcessStatus { get; set; }
        public bool IsSuccess { get; set; }
        public string ProviderStatus { get; set; }
        public bool Enable { get; set; }
        public string LastModify { get; set; }
        public string CreateDate { get; set; }
        public List<ProviderModel> RelatedProvider { get; set; }

        public List<string> oStrListProviderStatus;
        public List<string> strListProviderStatus
        {
            get
            {
                if (oStrListProviderStatus == null)
                {
                    string[] strProviderStatus = {                                    
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.CreacionExtranjero),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.CreacionNacional),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ImposibleContactarExtranjero),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ImposibleContactarNacional),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.InactivoExtranjero),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.InactivoNacional),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ProcesoExtranjero),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ProcesoNacional),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ValidadoBasicaNacional),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ValidadoCompletaExtranjero),
                                                    Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.ValidadoCompletaNacional),
                                                };

                    oStrListProviderStatus = strListProviderStatus;

                }
                return oStrListProviderStatus;
            }
        }

        // private string oCI_ContactType;
        //public string CI_ContactType
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(oCI_ContactType))
        //        {
        //            oCI_ContactType = RelatedContactInfo.ItemInfo.
        //               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumContactInfoType.CompanyContactType).
        //               Select(y => y.Value).
        //               DefaultIfEmpty(string.Empty).
        //               FirstOrDefault();
        //        }
        //        return oCI_ContactType;
        //    }
        //}
    }
}
