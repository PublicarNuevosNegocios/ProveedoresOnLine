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
    }
}
