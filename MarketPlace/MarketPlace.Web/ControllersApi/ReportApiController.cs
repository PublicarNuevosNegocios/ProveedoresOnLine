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
                List<ProveedoresOnLine.SurveyModule.Models.SurveyModel> surveyByProvider = ProveedoresOnLine.Reports.Controller.ReportModule.SurveyGetAllByCustomer(SessionModel.CurrentCompany.CompanyPublicId);
                StringBuilder data = new StringBuilder();
                string strSep = ";";
                data.AppendLine
                        (
                        "\"" + "Tipo Evaluacion" + "\"" + strSep +
                        "\"" + "Responsable" + "\"" + strSep +
                        "\"" + "Proyecto" + "\"" + strSep +
                        "\"" + "Observaciones" + "\"" + strSep +
                        "\"" + "Estado" + "\"" + strSep +
                        "\"" + "Fecha Asignación" + "\"" + strSep +
                        "\"" + "Fecha Vencimiento" + "\"" + strSep +
                        "\"" + "Evaluadores" + "\"" + strSep +
                        "\"" + "Calificación" + "\"" + strSep +
                        "\"" + "Última Modificación" + "\"");
                surveyByProvider.All(x =>
                {
                    if (x != null && x.SurveyInfo.Count > 0)
                    {
                        data.AppendLine
                        (
                        "\"" + x.RelatedSurveyConfig.ItemName.ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Project).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(a => a.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.StartDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.EndDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + "N/D" + "\"" + strSep +
                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Rating).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                        "\"" + x.RelatedSurveyConfig.LastModify.ToString("dd/MM/yyyy") + "\""
                        );
                        if (x.ChildSurvey != null && x.ChildSurvey.Count > 0)
                        {
                            x.ChildSurvey.All(y =>
                            {
                                if (y != null && y.ParentSurveyPublicId == x.SurveyPublicId)
                                {
                                    data.AppendLine
                                      (
                                        "\"" + x.RelatedSurveyConfig.ItemName.ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Responsible).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.Project).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + y.SurveyInfo.Where(b => b.ItemInfoType.ItemId == (int)enumSurveyInfoType.Comments).Select(b => b.Value).DefaultIfEmpty("").FirstOrDefault() + "\"" + strSep +
                                        "\"" + y.SurveyInfo.Where(b => b.ItemInfoType.ItemId == (int)enumSurveyInfoType.Status).Select(b => b.ValueName).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.StartDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                        "\"" + x.SurveyInfo.Where(a => a.ItemInfoType.ItemId == (int)enumSurveyInfoType.EndDate).Select(a => a.Value).DefaultIfEmpty("").FirstOrDefault().ToString() + "\"" + strSep +
                                       "\"" + y.User.ToString() + "\"" + strSep +
                                       "\"" + y.SurveyInfo.Where(b => b.ItemInfoType.ItemId == (int)enumSurveyInfoType.Rating).Select(b => b.Value).DefaultIfEmpty("").FirstOrDefault() + "\"" + strSep +
                                       "\"" + y.LastModify.ToString("dd/MM/yyyy") + "\""
                                      );
                                }
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
