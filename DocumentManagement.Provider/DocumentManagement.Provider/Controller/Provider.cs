using DocumentManagement.Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Provider.Controller
{
    public class Provider
    {
        static public string ProviderUpsert(string CustomerPublicId, string ProviderPublicId, string Name, Models.Enumerations.enumIdentificationType IdentificationType, string IdentificationNumber, string Email, Models.Enumerations.enumProcessStatus Status)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderUpsert(CustomerPublicId, ProviderPublicId, Name, IdentificationType, IdentificationNumber, Email, Status);
        }

        static public string ProviderInfoUpsert(int ProviderInfoId, string ProviderPublicId, Models.Enumerations.enumProviderInfoType ProviderInfoType, string Value, string LargeValue)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderInfoUpsert(ProviderInfoId, ProviderPublicId, ProviderInfoType, Value, LargeValue);
        }

        static public string ProviderCustomerInfoUpsert(int ProviderCustomerInfoId, string ProviderPublicId, string CustomerPublicId, DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType ProviderCustomerInfoType, string Value, string LargeValue)
        {
            return DAL.Controller.ProviderDataController.Instance.ProviderCustomerInfoUpsert(ProviderCustomerInfoId, ProviderPublicId, CustomerPublicId, ProviderCustomerInfoType, Value, LargeValue);
        }

        static public string LoadFile(string FilePath, string RemoteFolder)
        {
            File oLoader = new File()
            {
                FilesToUpload = new List<string>() { FilePath },
                RemoteFolder = RemoteFolder
            };

            oLoader.StartUpload();

            return oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == FilePath).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
        }
    }
}
