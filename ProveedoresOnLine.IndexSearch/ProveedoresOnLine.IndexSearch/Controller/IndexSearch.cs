using Nest;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.IndexSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
namespace ProveedoresOnLine.IndexSearch.Controller
{
    public class IndexSearch
    {
        #region Company Index

        public static List<CompanyIndexModel> GetCompanyIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetCompanyIndex();
        }

        public static List<CustomerProviderIndexModel> GetCustomerProviderIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetCustomerProviderIndex();
        }

        public static bool CompanyIndexationFunction()
        {
            List<CompanyIndexModel> oCompanyToIndex = GetCompanyIndex();
            try
            {   
                LogFile("Start Process: " + "ProvidersToIndex:::" + oCompanyToIndex.Count());

                Uri node = new Uri(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_ElasticSearchUrl].Value);
                var settings = new ConnectionSettings(node);
                settings.DefaultIndex(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_CompanyIndex].Value);
                ElasticClient client = new ElasticClient(settings);

                ICreateIndexResponse oElasticResponse = client.
                        CreateIndex(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_CompanyIndex].Value, c => c
                        .Settings(s => s.NumberOfReplicas(0).NumberOfShards(1)
                        .Analysis(a => a.
                            Analyzers(an => an.
                                Custom("customWhiteSpace", anc => anc.
                                    Filters("asciifolding", "lowercase").
                                    Tokenizer("whitespace")
                                        )
                                    ).TokenFilters(tf => tf
                                    .EdgeNGram("customEdgeNGram", engrf => engrf
                                    .MinGram(1)
                                    .MaxGram(10))
                                )
                            ).NumberOfShards(1)
                        )
                    );
                client.Map<CompanyIndexModel>(m => m.AutoMap());
                var Index = client.IndexMany(oCompanyToIndex, ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_CompanyIndex].Value);
            }
            catch (Exception err)
            {
                LogFile("Index Process Failed for Company: " + err.Message + "Inner Exception::" + err.InnerException);
            }
            LogFile("Index Process Successfull for: " + oCompanyToIndex.Count());
            return true;
        }

        public static bool CustomerProviderIdexationFunction() 
        {
            int CustomerProviderId = 0;
            var Counter = 0;
            try
            {
                List<CustomerProviderIndexModel> oCustomerProviderToIndex = GetCustomerProviderIndex();
                LogFile("About to index: " + oCustomerProviderToIndex.Count + " CustomerProviders");

                oCustomerProviderToIndex.All(cp =>
                    {
                        Uri node = new Uri(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_ElasticSearchUrl].Value);
                        var settings = new ConnectionSettings(node);
                        settings.DefaultIndex(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_CustomerProviderIndex].Value);
                        ElasticClient client = new ElasticClient(settings);
                        CustomerProviderId = cp.CustomerProviderId;

                        ICreateIndexResponse oElasticResponse = client.CreateIndex(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_CustomerProviderIndex].Value, c => c
                        .Settings(s => s.NumberOfReplicas(0).NumberOfShards(1)
                        .Analysis(a => a.Analyzers(an => an.Custom("customWhiteSpace", anc => anc.Filters("asciifolding", "lowercase")
                            .Tokenizer("whitespace")
                            )).TokenFilters(tf => tf
                                    .EdgeNGram("customEdgeNGram", engrf => engrf
                                    .MinGram(1)
                                    .MaxGram(10)))).NumberOfShards(1)
                        ));
                        Counter++;
                        var Index = client.Index(cp);
                        LogFile(Counter + "-Index Process for: " + cp.CustomerProviderId);

                        return true;
                    });
            }
            catch (Exception err)
            {
                LogFile("Index Process Failed for CustomerProvider: " + CustomerProviderId + err.Message + "Inner Exception::" + err.InnerException);                
            }

            LogFile("Index Process Successfull for: " + Counter + " Customers-Providers");
            return true;
        }

        #endregion

        #region Survey Index

        public static List<SurveyIndexSearchModel> GetSurveyIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetSurveyIndex();
        }

        #region Survey Info Index

        public static List<SurveyInfoIndexSearchModel> GetSurveyInfoIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetSurveyInfoIndex();
        }

        #endregion

        #endregion

        #region LogFile
        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.IndexSearch.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_IndexSearchProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                using (System.IO.StreamWriter sw = System.IO.File.AppendText(LogFile))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "::" + LogMessage);
                    sw.Close();
                }
            }
            catch { }
        }
        #endregion
    }
}
