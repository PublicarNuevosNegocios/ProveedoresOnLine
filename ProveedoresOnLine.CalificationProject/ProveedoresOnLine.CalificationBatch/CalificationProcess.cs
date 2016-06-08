using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch
{
    public class CalificationProcess
    {
        public static void StartProcess()
        {
            try
            {
                List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oCalificationProjectConfigModel =
                    ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetAll();

                oCalificationProjectConfigModel.All(cnf =>
                {
                    List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedProvider =
                        ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(cnf.Company.CompanyPublicId);

                    oRelatedProvider.All(prv =>
                    {
                        return true;
                    });

                    return true;
                });

                //Get Configuration Process


            }
            catch (Exception)
            {
                
                throw;
            }
            //Call Customers
            //Get CongigCalificationProject
            //Get Info Provider from
        }

        private void GetCustomer()
        {
        }
    }
}
