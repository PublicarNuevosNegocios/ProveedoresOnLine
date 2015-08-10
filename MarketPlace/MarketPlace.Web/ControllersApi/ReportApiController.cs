using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class ReportApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public byte[] SurveyGeneralInfoReport(string SurveyGeneralInfoReport, string algo)
        {
            byte[] buffer = null;
            if (SurveyGeneralInfoReport == "true")
            {
                List<ProveedoresOnLine.SurveyModule.Models.SurveyModel>  surveyByProvider = ProveedoresOnLine.Reports.Controller.ReportModule.SurveyGetAllByCustomer(SessionModel.CurrentCompany.CompanyPublicId);
                StringBuilder data = new StringBuilder();

                string strSep = ";";
                surveyByProvider.All(x => {

                    if (x != null) { 
                        //if parent
                        data.AppendLine
                        ("\"" + "RazonSocial" + "\"" + strSep +
                        "\"" + "IdentificationType" + "\"" + strSep +
                        "\"" + "IdentificationNumber" + "\"" + strSep +
                        "\"" + "SearchType" + "\"" + strSep +
                        "\"" + "ProviderPublicId" + "\"" + strSep +
                        "\"" + "BlackListStatus" + "\"");

                        if (x.ChildSurvey != null) {
                            data.AppendLine
                                ("\"" + "RazonSocial" + "\"" + strSep +
                                "\"" + "IdentificationType" + "\"" + strSep +
                                "\"" + "IdentificationNumber" + "\"" + strSep +
                                "\"" + "SearchType" + "\"" + strSep +
                                "\"" + "ProviderPublicId" + "\"" + strSep +
                                "\"" + "BlackListStatus" + "\"");    
                            x.ChildSurvey.All(y=>{
                                
                                return true;
                            });
                        }
                    }
                    return true;
                });


                
                
            }
            return buffer;
        }
    }
}
