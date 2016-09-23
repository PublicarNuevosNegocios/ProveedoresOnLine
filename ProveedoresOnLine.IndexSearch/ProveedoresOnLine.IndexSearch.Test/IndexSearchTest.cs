using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ProveedoresOnLine.IndexSearch.Models;
using ProveedoresOnLine.Company.Models.Company;
using System.Linq;
using Nest;
using System.Collections;

namespace ProveedoresOnLine.IndexSearch.Test
{
    [TestClass]
    public class IndexSearchTest
    {
        [TestMethod]
        public void GetAllCompanyIndexSearch()
        {
            List<CompanyIndexModel> oReturn =
                Controller.IndexSearch.GetCompanyIndex();

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        [TestMethod]
        public void GetAllCustomerProviderIndexSearch()
        {
            List<CustomerProviderIndexModel> oReturn = ProveedoresOnLine.IndexSearch.Controller.IndexSearch.GetCustomerProviderIndex();

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        [TestMethod]
        public void GetAllSurveyIndexSearch()
        {
            List<SurveyIndexSearchModel> oReturn =
                Controller.IndexSearch.GetSurveyIndex();

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        [TestMethod]
        public void GetAllSurveyInfoIndexSearch()
        {
            List<SurveyInfoIndexSearchModel> oReturn =
                Controller.IndexSearch.GetSurveyInfoIndex();

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }
        [TestMethod]
        public void CompanyIndexationFunction()
        {
            bool oReturn = Controller.IndexSearch.CompanyIndexationFunction();

            Assert.AreEqual(true, oReturn != null && oReturn == true);
        }

        [TestMethod]
        public void CustomerProviderIndexationFunction()
        {
            bool oReturn = Controller.IndexSearch.CustomerProviderIdexationFunction();

            Assert.AreEqual(true, oReturn != null && oReturn == true);
        }

        [TestMethod]
        public void GetCustomerProviderByCustomer()
        {
            Uri node = new Uri(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_ElasticSearchUrl].Value);
            var settings = new ConnectionSettings(node);

            settings.DefaultIndex(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_CustomerProviderIndex].Value);
            ElasticClient CustomerProviderClient = new ElasticClient(settings);
            var result = CustomerProviderClient.Search<CustomerProviderIndexModel>(s => s
            .From(0)
            .Size(20)
             .Query(q => q
                            .Match(m => m.Field("customerPublicId").Query("DA5C572E"))
            ));
        }

        [TestMethod]
        public void SearchCompany()
        {
            Uri node = new Uri(ProveedoresOnLine.IndexSearch.Models.Util.InternalSettings.Instance[ProveedoresOnLine.IndexSearch.Models.Constants.C_Settings_ElasticSearchUrl].Value);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex("dev_companyindex");
            ElasticClient CustomerProviderClient = new ElasticClient(settings);

            Nest.ISearchResponse<CompanyIndexModel> result = CustomerProviderClient.Search<CompanyIndexModel>(s => s
            .From(0)
                .Size(20)
                    .Aggregations
                     (agg => agg
                        .Nested("status_avg", x => x.
                            Path(p => p.oCustomerProviderIndexModel).
                            Aggregations(aggs => aggs.
                                Terms("status", term => term.
                                    Field(fi => fi.oCustomerProviderIndexModel.First().StatusId)
                                )
                            )
                        )
                        .Terms("city", aggv => aggv
                            .Field(fi => fi.CityId))
                        .Terms("country", c => c
                            .Field(fi => fi.CountryId))
                        .Terms("blacklist", bl => bl
                            .Field(fi => fi.InBlackList)))
                .Query(q =>
                    q.Nested(n => n
                        .Path(p => p.oCustomerProviderIndexModel)
                            .Query(fq => fq
                                .Match(match => match
                                                    .Field(field => field.oCustomerProviderIndexModel.First().CustomerPublicId)
                                                    .Query("DA5C572E")
                                )
                              ).ScoreMode(NestedScoreMode.Max)
                           )
                    )
                .Query(q =>
                     q.Term(p => p.CompanyName, "SAP") ||
                     q.Term(p => p.CommercialCompanyName, "SAP") ||
                     q.Term(p => p.IdentificationNumber, "SAP"))
                );
        }
    }
}
