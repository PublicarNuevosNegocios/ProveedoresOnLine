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

                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel oCalificaitonProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel();

                        int oTotalScore = 0;

                        if (oRelatedCalificationProject != null &&
                            oRelatedCalificationProject.Count > 0)
                        {
                            //validate date calification process config

                        }
                        else
                        {
                            oCalificaitonProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                            {
                                CalificationProjectId = 0,
                                CalificationProjectPublicId = "",
                                ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                                {
                                    CalificationProjectConfigId = cnf.CalificationProjectConfigId,
                                },
                                RelatedProvider = new Company.Models.Company.CompanyModel()
                                {
                                    CompanyPublicId = prv.CompanyPublicId,
                                },
                                CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
                                Enable = true,
                            };

                            //execute all calification process
                            cnf.ConfigItemModel.All(md =>
                            {
                                switch (md.CalificationProjectConfigItemType.ItemId)
                                {
                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oLegalModule = 
                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, md, null);

                                        oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oLegalModule);

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

                        oCalificaitonProjectUpsert.TotalScore = oTotalScore;

                        //Upsert
                        oCalificaitonProjectUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oCalificaitonProjectUpsert);

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
