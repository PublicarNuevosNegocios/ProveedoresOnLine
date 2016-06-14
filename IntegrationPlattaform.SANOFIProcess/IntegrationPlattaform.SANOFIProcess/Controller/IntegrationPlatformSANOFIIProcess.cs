using IntegrationPlattaform.SANOFIProcess.Models;
using ProveedoresOnLine.Company.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.Controller
{
    public class IntegrationPlatformSANOFIIProcess
    {
        public static void StartProcess()
        {
            try
            {
                // Get Providers SANOFI
               List<CompanyModel> oProviders = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(
                    IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance[
                    IntegrationPlattaform.SANOFIProcess.Models.Constants.C_SANOFI_ProviderPublicId].Value);

               if (oProviders != null)
               {
                   oProviders.All(p =>
                       {
                           //Get Last Process
                           //Modify Date against Last Created process

                           //TODO: Get GeneralInfo By Provider
                           

                           return true;
                       });
               }
                
                // Get Process Last Time 
                // Get Info by Provider by lastModify 

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static List<SanofiGeneralInfoModel> GetInfo_ByProvider(string vProviderPublicId) 
        {
           return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetInfo_ByProvider(vProviderPublicId);
        }

        public static List<SanofiComercialInfoModel> GetComercialInfo_ByProvider(string vProviderPublicId) 
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetComercialInfo_ByProvider(vProviderPublicId);
        }

        public static List<Models.SanofiContableInfoModel> GetContableInfo_ByProvider(string vProviderPublicId) 
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetContableInfo_ByProvider(vProviderPublicId);
        }

        public static SanofiProcessLogModel SanofiProcessLog_Insert(SanofiProcessLogModel oLogModel) 
        {
            try
            {
                if (oLogModel != null)
                {
                    oLogModel.SanofiProcessLogId = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.SanofiProcessLog_Insert
                        (
                            oLogModel.ProviderPublicId,
                            oLogModel.ProcessName,
                            oLogModel.IsSucces,
                            oLogModel.Enable
                        );
                }
            }
            catch (Exception err)
            {                
                throw err;
            }
            return oLogModel;
        }

        public static List<SanofiProcessLogModel> GetSanofiProcessLog(bool IsSuccess) 
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetSanofiProcessLog(IsSuccess);
        }
    }
}
