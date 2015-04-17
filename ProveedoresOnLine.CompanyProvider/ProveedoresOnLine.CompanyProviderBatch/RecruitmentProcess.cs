using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProviderBatch
{
    public class RecruitmentProcess
    {
        public static void StartProcess()
        {
            try
            {
                List<ProviderModel> oProviderModelList = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BPGetRecruitmentProviders();

                if (oProviderModelList != null && oProviderModelList.Count > 0)
                {
                    //log file
                    LogFile("Start K_Recruitment Calc" + DateTime.Now.ToString("yyyyMMdd"));

                    RecruitmentCalculate RecruitmentCal = new RecruitmentCalculate();


                    oProviderModelList.All(providerToCalculate =>
                    {
                        try
                        {
                            //Get CommercialInfo
                            providerToCalculate.RelatedCommercial = CompanyProvider.Controller.CompanyProvider.CommercialGetBasicInfo(providerToCalculate.RelatedCompany.CompanyPublicId, (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumComercialType.Experience, true);
                            //Get LegalInfo
                            providerToCalculate.RelatedLegal = CompanyProvider.Controller.CompanyProvider.LegalGetBasicInfo(providerToCalculate.RelatedCompany.CompanyPublicId, (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumLegalType.ChaimberOfCommerce, true);
                            //Get FinancialInfo
                            providerToCalculate.RelatedFinantial = CompanyProvider.Controller.CompanyProvider.FinancialGetBasicInfo(providerToCalculate.RelatedCompany.CompanyPublicId, (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumFinancialType.BalanceSheetInfoType, true);
                            if (providerToCalculate.RelatedCommercial != null && providerToCalculate.RelatedCommercial.Count > 0
                                && providerToCalculate.RelatedLegal != null && providerToCalculate.RelatedLegal.Count > 0
                                && providerToCalculate.RelatedFinantial != null && providerToCalculate.RelatedFinantial.Count > 0)
                            {                              

                                #region Find the Role

                                List<List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>> AllActivities = new List<List<Company.Models.Util.GenericItemModel>>();

                                providerToCalculate.RelatedCommercial.All(EcoAct =>
                                {
                                    List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> ProviderAE = EcoAct.ItemInfo.
                                        Where(y => y.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumCommercialInfoType.EX_EconomicActivity).
                                        Select(y => y.ValueName).
                                        DefaultIfEmpty(string.Empty).
                                        FirstOrDefault().
                                        Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                                        Where(y => y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length >= 2).
                                        Select(y => new ProveedoresOnLine.Company.Models.Util.GenericItemModel
                                        {
                                            ItemId = Convert.ToInt32(y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]),
                                            ItemName = y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1],
                                        }).ToList();

                                    if (ProviderAE != null && ProviderAE.Count > 0)
                                        AllActivities.Add(ProviderAE);

                                    return true;
                                });

                                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oActivities = new List<Company.Models.Util.GenericItemModel>();

                                AllActivities.All(ac =>
                                {
                                    ac.All(x =>
                                    {
                                        oActivities.Add(ProveedoresOnLine.Company.Controller.Company.CategorySearchByActivity(x.ItemName, 0, 20).FirstOrDefault());
                                        return true;
                                    });
                                    return true;
                                });

                                //List<GenericItemModel> ProvidersRole = new List<GenericItemModel>();
                                List<List<string>> ProvidersRole = new List<List<string>>();
                                List<string> oActivitieType = new List<string>();
                                if (oActivities != null && oActivities.Count > 0)
                                {
                                    oActivities.All(x =>
                                    {
                                        x.ItemInfo.Where(y => y.ItemInfoType.ItemId == (int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumUtil.RoleType)
                                            .Select(y => y.LargeValue).FirstOrDefault().
                                            Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                                            Where(y => y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length >= 1).
                                            Select(y =>
                                            {
                                                for (int i = 0; i < y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Count(); i++)
                                                    oActivitieType.Add(y.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[i]);
                                                return y;
                                            }).ToList();
                                        return true;
                                    });
                                }
                                #endregion

                                if (oActivitieType != null && oActivitieType.Count > 0)
                                {
                                    if (oActivitieType.Any(y => y == ((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumActivitiEconomicInfoType.Provider).ToString()))
                                    {
                                        //get calculate provider    
                                        RecruitmentCal.GetProviderScore(providerToCalculate);                                        
                                    }
                                    if (oActivitieType.Any(y => y == ((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumActivitiEconomicInfoType.Consultant).ToString()))
                                    {
                                        //Get Calculate Consultant
                                        GetConsultantScore(providerToCalculate);
                                    }
                                    if (oActivitieType.Any(y => y == ((int)ProveedoresOnLine.CompanyProviderBatch.Models.Enumerations.enumActivitiEconomicInfoType.Builder).ToString()))
                                    {
                                        //Get Calculate Builder
                                        GetBuilderScore(providerToCalculate);
                                    }
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            LogFile("Error:: ProviderPublicId '" + providerToCalculate.RelatedCompany.CompanyPublicId + "' :: " + err.Message);
                        }
                        return true;
                    });

                    //log file
                    LogFile("End Calculation" + "yyyyMMdd");
                }
            }
            catch (Exception err)
            {
                //log file for fatal error
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }

        #region Log File

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.CompanyProviderBatch.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_RecruitmentProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                using (System.IO.StreamWriter sw = System.IO.File.AppendText(LogFile))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "::" + LogMessage);
                    sw.Close();
                }
            }
            catch { }
        }

        #endregion

        #region Calucalate Functions

       

        private static GenericItemModel GetConsultantScore(ProviderModel oProvider)
        {
            GenericItemModel K_ContactModel = new GenericItemModel();

            return K_ContactModel;
        }

        private static GenericItemModel GetBuilderScore(ProviderModel oProvider)
        {
            GenericItemModel K_ContactModel = new GenericItemModel();

            return K_ContactModel;
        }

        #endregion       
    }
}
