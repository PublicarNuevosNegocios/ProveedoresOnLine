using Nest;
using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.IndexSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.Controller
{
    public class IndexSearch
    {
        #region Company Index

        public static List<CompanyIndexModel> GetCompanyIndex()
        {
            return DAL.Controller.IndexSearchDataController.Instance.GetCompanyIndex();
        }

        public static bool CompanyIndexationFunction()
        {

            List<CompanyIndexModel> oCompanyToIndex = GetCompanyIndex();

            oCompanyToIndex.All(prov =>
                {
                    //Uri node = new Uri(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_ElasticSearchUrl].Value);
                    //var settings = new ConnectionSettings(node);
                    //settings.DefaultIndex(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CompanyIndex].Value);
                    //ElasticClient client = new ElasticClient(settings);

                    //ICreateIndexResponse oElasticResponse = client.CreateIndex(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_CompanyIndex].Value, c => c
                    //    .Settings(s => s.NumberOfReplicas(0).NumberOfShards(1)
                    //    .Analysis(a => a.Analyzers(an => an.Custom("customWhiteSpace", anc => anc.Filters("asciifolding", "lowercase")
                    //        .Tokenizer("whitespace")
                    //        )).TokenFilters(tf => tf
                    //                .EdgeNGram("customEdgeNGram", engrf => engrf
                    //                .MinGram(1)
                    //                .MaxGram(10)))).NumberOfShards(1)
                    //    ));

                    //var Index = client.Index(oCompanyToIndex);
                    return true;
                });


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
    }
}
