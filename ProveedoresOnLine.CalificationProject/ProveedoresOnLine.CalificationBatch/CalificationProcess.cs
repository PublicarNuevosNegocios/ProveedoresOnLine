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
                        List<Models.CalificationProjectBatch.CalificationProjectBatchModel> oRelatedCalificationProject =
                            ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetByCustomer(cnf.Company.CompanyPublicId, prv.CompanyPublicId, true);

                        List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel> oRelatedModules = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>();

                        if (oRelatedCalificationProject != null &&
                            oRelatedCalificationProject.Count > 0)
                        {
                            //validate date calification process config

                        }
                        else
                        {
                            //execute all calification process
                            cnf.ConfigItemModel.All(md =>
                            {
                                switch (md.CalificationProjectConfigItemType.ItemId)
                                {
                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                        List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel> oLegalModule = 
                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, md);

                                        oLegalModule.All(lg =>
                                        {
                                            oRelatedModules.Add(lg);
                                            return true;
                                        });                                        

                                        break;
                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

                                        break;
                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                        break;
                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                        break;
                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                        break;
                                }

                                return true;
                            });
                        }

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
