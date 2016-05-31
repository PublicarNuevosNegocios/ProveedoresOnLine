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
        public void StartProcess()
        {
            try
            {
                // Get Providers SANOFI
               List<CompanyModel> oProviders = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId("");

                
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
