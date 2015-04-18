using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ProjectModule.Controller
{
    public class ProjectEvaluator
    {
        #region Properties

        public ProveedoresOnLine.ProjectModule.Models.ProjectModel RelatedProject { get; private set; }

        #endregion

        #region Constructors

        public ProjectEvaluator(string oProjectPublicId, string oCustomerPublicId)
        {
            //get project calculate info
            RelatedProject = ProjectModule.ProjectGetByIdCalculate(oProjectPublicId, oCustomerPublicId);
        }

        #endregion

        public void InitEval()
        {
            RelatedProject.RelatedProjectProvider.All(pjpv =>
            {
                //create upsert model
                ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel oProjectProviderToUpsert =
                    new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                {
                    ProjectCompanyId = pjpv.ProjectCompanyId,
                    ItemInfo = new List<Models.ProjectProviderInfoModel>(),
                };

                //loop for evaluation criteria

                List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oResult;

                RelatedProject.RelatedProjectConfig.RelatedEvaluationItem.
                Where(ei => ei.ItemType.ItemId == 1401002).
                All(ei =>
                {
                    switch (ei.ItemInfo.
                                Where(eiinf => eiinf.ItemInfoType.ItemId == 1402005).
                                Select(eiinf => string.IsNullOrEmpty(eiinf.Value) ? 0 : Convert.ToInt32(eiinf.Value)).
                                DefaultIfEmpty(0).
                                FirstOrDefault())
                    {
                        case 1404001:
                            oResult = Commercial_EvalExperience(pjpv, ei);
                            if (oResult != null && oResult.Count > 0)
                            {
                                oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            }
                            break;
                        case 1404002:
                            //oResult = Certification_EvalNorms(pjpv, ei);
                            //if (oResult != null && oResult.Count > 0)
                            //{
                            //    oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            //}
                            break;
                        case 1404003:
                            //oResult = Certification_EvalLTIF(pjpv, ei);
                            //if (oResult != null && oResult.Count > 0)
                            //{
                            //    oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            //}
                            break;
                        case 1404004:
                            //oResult = Certification_EvalRiskPolicies(pjpv, ei);
                            //if (oResult != null && oResult.Count > 0)
                            //{
                            //    oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            //}
                            break;
                        case 1404005:
                            //oResult = Financial_EvalBalanceSheet(pjpv, ei);
                            //if (oResult != null && oResult.Count > 0)
                            //{
                            //    oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            //}
                            break;
                        case 1404006:
                            //oResult = Legal_EvalChamberOfCommerce(pjpv, ei);
                            //if (oResult != null && oResult.Count > 0)
                            //{
                            //    oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            //}
                            break;
                        case 1404007:
                            //oResult = Legal_EvalRut(pjpv, ei);
                            //if (oResult != null && oResult.Count > 0)
                            //{
                            //    oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            //}
                            break;
                        case 1404008:
                            //oResult = Legal_EvalSARLAFT(pjpv, ei);
                            //if (oResult != null && oResult.Count > 0)
                            //{
                            //    oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            //}
                            break;
                        default:
                            break;
                    }

                    return true;
                });

                //recalculate area results

                //upsert evaluation items responses
                oProjectProviderToUpsert = ProjectModule.ProjectCompanyInfoUpsert(oProjectProviderToUpsert);

                return true;
            });
        }

        #region Commercial

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Commercial_EvalExperience
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //get years to eval
            int oMinYear = RelatedProject.ProjectInfo.
                Where(pjinf => pjinf.ItemInfoType.ItemId == 1407003).
                Select(pjinf => string.IsNullOrEmpty(pjinf.Value) ? DateTime.Now.Year - 1 : DateTime.Now.Year - Convert.ToInt32(pjinf.Value.Replace(" ", ""))).
                DefaultIfEmpty(DateTime.Now.Year - 1).
                FirstOrDefault();

            //get min project value
            decimal oMinValue = RelatedProject.ProjectInfo.
                Where(pjinf => pjinf.ItemInfoType.ItemId == 1407002).
                Select(pjinf => string.IsNullOrEmpty(pjinf.Value) ? 0 : Convert.ToDecimal(pjinf.Value.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
                DefaultIfEmpty(0).
                FirstOrDefault();

            //get min number of experience
            int oMinExperience = RelatedProject.ProjectInfo.
                Where(pjinf => pjinf.ItemInfoType.ItemId == 1407004).
                Select(pjinf => string.IsNullOrEmpty(pjinf.Value) ? 0 : Convert.ToInt32(pjinf.Value.Replace(" ", ""))).
                DefaultIfEmpty(0).
                FirstOrDefault();

            //get default economic activity list
            List<string> oDefaultActivityList = RelatedProject.ProjectInfo.
                Where(pjinf => pjinf.ItemInfoType.ItemId == 1407005).
                Select(pjinf => string.IsNullOrEmpty(pjinf.LargeValue) ? string.Empty : pjinf.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault().
                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                ToList();

            //get custom economic activity list
            List<string> oCustomActivityList = RelatedProject.ProjectInfo.
                Where(pjinf => pjinf.ItemInfoType.ItemId == 1407006).
                Select(pjinf => string.IsNullOrEmpty(pjinf.LargeValue) ? string.Empty : pjinf.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault().
                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                ToList();

            //get experience in activity
            vProjectProvider.RelatedProvider.RelatedCommercial.
                Where(cm => cm.ItemType.ItemId == 301001 &&
                            (
                            cm.ItemInfo.Any(cminf => cminf.ItemInfoType.ItemId == 302012 &&
                                                    (oDefaultActivityList.Count == 0 ? true :
                                                    cminf.LargeValue.Split(',').
                                                    Any(dea => oDefaultActivityList.
                                                                Any(deate => deate == dea)))
                                            )
                            ) &&
                            (
                            cm.ItemInfo.Any(cminf => cminf.ItemInfoType.ItemId == 302013 &&
                                                    (oCustomActivityList.Count == 0 ? true :
                                                    cminf.LargeValue.Split(',').
                                                    Any(dea => oCustomActivityList.
                                                                Any(deate => deate == dea)))
                                            )
                            )
                    );







            //evaluate certification items
            var oValidCertificationId =
                vProjectProvider.RelatedProvider.RelatedCertification.
                Where(rc => rc.ItemType.ItemId == 701001 &&
                            rc.ItemInfo.
                                Any(rcinf => rcinf.ItemInfoType.ItemId == 702004 &&
                                             !string.IsNullOrEmpty(rcinf.Value) &&
                                             Convert.ToDateTime(rcinf.Value) >= DateTime.Now)).
                Select(ct => new
                {
                    ItemId = ct.ItemId,
                    ItemResults = vEvaluationItem.ItemInfo.
                                    Where(eiinf => eiinf.ItemInfoType.ItemId == 1402006 &&
                                                    !string.IsNullOrEmpty(eiinf.Value) &&
                                                    eiinf.Value.Split('_').Length >= 3).
                                    Select(eiinf => ValidateCondition(eiinf.Value, ct)).
                                    ToList(),
                }).
                Where(er => !er.ItemResults.Any(erit => !erit)).
                Select(er => (int?)er.ItemId).
                DefaultIfEmpty(null).
                FirstOrDefault();

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oValidCertificationId == null ? "0" : "100");
            oValues.Add(1408002, oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }

        #endregion

        #region Certification

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Certification_EvalNorms
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //evaluate certification items
            var oValidCertificationId =
                vProjectProvider.RelatedProvider.RelatedCertification.
                Where(rc => rc.ItemType.ItemId == 701001 &&
                            rc.ItemInfo.
                                Any(rcinf => rcinf.ItemInfoType.ItemId == 702004 &&
                                             !string.IsNullOrEmpty(rcinf.Value) &&
                                             Convert.ToDateTime(rcinf.Value) >= DateTime.Now)).
                Select(ct => new
                {
                    ItemId = ct.ItemId,
                    ItemResults = vEvaluationItem.ItemInfo.
                                    Where(eiinf => eiinf.ItemInfoType.ItemId == 1402006 &&
                                                    !string.IsNullOrEmpty(eiinf.Value) &&
                                                    eiinf.Value.Split('_').Length >= 3).
                                    Select(eiinf => ValidateCondition(eiinf.Value, ct)).
                                    ToList(),
                }).
                Where(er => !er.ItemResults.Any(erit => !erit)).
                Select(er => (int?)er.ItemId).
                DefaultIfEmpty(null).
                FirstOrDefault();

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oValidCertificationId == null ? "0" : "100");
            oValues.Add(1408002, oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Certification_EvalLTIF
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //evaluate certification items
            var oValidCertificationId =
                vProjectProvider.RelatedProvider.RelatedCertification.
                Where(rc => rc.ItemType.ItemId == 701003).
                Select(ct => new
                {
                    ItemId = ct.ItemId,
                    ItemResults = vEvaluationItem.ItemInfo.
                                    Where(eiinf => eiinf.ItemInfoType.ItemId == 1402006 &&
                                                    !string.IsNullOrEmpty(eiinf.Value) &&
                                                    eiinf.Value.Split('_').Length >= 3).
                                    Select(eiinf => ValidateCondition(eiinf.Value, ct)).
                                    ToList(),
                }).
                Where(er => !er.ItemResults.Any(erit => !erit)).
                Select(er => (int?)er.ItemId).
                DefaultIfEmpty(null).
                FirstOrDefault();

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oValidCertificationId == null ? "0" : "100");
            oValues.Add(1408002, oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Certification_EvalRiskPolicies
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //evaluate certification items
            var oValidCertificationId =
                vProjectProvider.RelatedProvider.RelatedCertification.
                Where(rc => rc.ItemType.ItemId == 701002).
                OrderByDescending(rc => rc.ItemInfo.
                                        Where(rcinf => rcinf.ItemInfoType.ItemId == 703001).
                                        Select(rcinf => string.IsNullOrEmpty(rcinf.Value) ? 0 : Convert.ToInt32(rcinf.Value.Replace(" ", ""))).
                                        DefaultIfEmpty(0).
                                        FirstOrDefault()).
                Select(ct => new
                {
                    ItemId = ct.ItemId,
                    ItemResults = vEvaluationItem.ItemInfo.
                                    Where(eiinf => eiinf.ItemInfoType.ItemId == 1402006 &&
                                                    !string.IsNullOrEmpty(eiinf.Value) &&
                                                    eiinf.Value.Split('_').Length >= 3).
                                    Select(eiinf => ValidateCondition(eiinf.Value, ct)).
                                    ToList(),
                }).
                Where(er => !er.ItemResults.Any(erit => !erit)).
                Select(er => (int?)er.ItemId).
                DefaultIfEmpty(null).
                FirstOrDefault();

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oValidCertificationId == null ? "0" : "100");
            oValues.Add(1408002, oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }

        #endregion

        #region Financial

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Financial_EvalBalanceSheet
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //get years to eval
            int oMinYear = vEvaluationItem.ItemInfo.
                Where(eiinf => eiinf.ItemInfoType.ItemId == 1402007 &&
                              !string.IsNullOrEmpty(eiinf.Value)).
                Select(eiinf => DateTime.Now.Year - Convert.ToInt32(eiinf.Value)).
                DefaultIfEmpty(DateTime.Now.Year - 1).
                FirstOrDefault();

            //get Account to eval and ranges account,minvalue,comparison,result
            List<Tuple<int, decimal, int, decimal>> oEvalInfo = vEvaluationItem.ItemInfo.
                Where(eiinf => eiinf.ItemInfoType.ItemId == 1402006 &&
                            !string.IsNullOrEmpty(eiinf.Value) &&
                            eiinf.Value.Split('_').Length >= 4).
                Select(eiinf => new Tuple<int, decimal, int, decimal>
                                (Convert.ToInt32(eiinf.Value.Split('_')[0].Replace(" ", "")),
                                Convert.ToDecimal(eiinf.Value.Split('_')[1].Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")),
                                Convert.ToInt32(eiinf.Value.Split('_')[2].Replace(" ", "")),
                                Convert.ToDecimal(eiinf.Value.Split('_')[3].Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")))).
                ToList();

            //get avg value to evaluate
            decimal oAccountValue = vProjectProvider.RelatedProvider.RelatedBalanceSheet.
                Where(fi => fi.ItemInfo.
                    Any(fiinf => fiinf.ItemInfoType.ItemId == 502001 &&
                                !string.IsNullOrEmpty(fiinf.Value) &&
                                Convert.ToInt32(fiinf.Value) >= oMinYear &&
                                Convert.ToInt32(fiinf.Value) < DateTime.Now.Year)).
                Select(fi => fi.BalanceSheetInfo.
                    Where(bs => bs.RelatedAccount.ItemId == oEvalInfo.FirstOrDefault().Item1).
                    FirstOrDefault()).
                Sum(acinf => acinf.Value);

            oAccountValue = oAccountValue / (DateTime.Now.Year > oMinYear ? (DateTime.Now.Year - oMinYear) : 1);

            //eval ranges and get value
            decimal oResult = oEvalInfo.
                OrderByDescending(evinf => evinf.Item2).
                Where(evinf => evinf.Item3 == 1409002 ? evinf.Item2 < oAccountValue : evinf.Item2 <= oAccountValue).
                Select(evinf => evinf.Item4).
                DefaultIfEmpty(0).
                FirstOrDefault();


            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oResult.ToString("0.##"));
            oValues.Add(1408002, oEvalInfo.FirstOrDefault().Item1.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }


        #endregion

        #region Legal

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Legal_EvalChamberOfCommerce
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //evaluate certification items
            var oValidCertificationId =
                vProjectProvider.RelatedProvider.RelatedLegal.
                Where(rlg => rlg.ItemType.ItemId == 601001 &&
                    //validate file exists
                             ValidateCondition("602006_1_1409001", rlg) &&
                    //validate expired date
                             rlg.ItemInfo.
                                Any(rlginf => rlginf.ItemInfoType.ItemId == 602002 &&
                                             !string.IsNullOrEmpty(rlginf.Value) &&
                                             Convert.ToDateTime(rlginf.Value) >= DateTime.Now)).
                Select(ct => (int?)ct.ItemId).
                DefaultIfEmpty(null).
                FirstOrDefault();

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oValidCertificationId == null ? "0" : "100");
            oValues.Add(1408002, oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Legal_EvalRut
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //evaluate certification items
            var oValidCertificationId =
                vProjectProvider.RelatedProvider.RelatedLegal.
                Where(rlg => rlg.ItemType.ItemId == 601002 &&
                    //validate file exists
                             ValidateCondition("603012_1_1409001", rlg)).
                Select(ct => (int?)ct.ItemId).
                DefaultIfEmpty(null).
                FirstOrDefault();

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oValidCertificationId == null ? "0" : "100");
            oValues.Add(1408002, oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Legal_EvalSARLAFT
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //evaluate certification items
            var oValidCertificationId =
                vProjectProvider.RelatedProvider.RelatedLegal.
                Where(rlg => rlg.ItemType.ItemId == 601004 &&
                    //validate file exists
                             ValidateCondition("605003_1_1409001", rlg)).
                Select(ct => (int?)ct.ItemId).
                DefaultIfEmpty(null).
                FirstOrDefault();

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oValidCertificationId == null ? "0" : "100");
            oValues.Add(1408002, oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString());

            //get standar response
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = CreateStandarResponse
                (vProjectProvider,
                vEvaluationItem,
                oValues);

            return oReturn;
        }

        #endregion

        #region Util

        private bool ValidateCondition
            (string EvaluationItemValue,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel ItemToEval)
        {
            bool oReturn = false;

            string[] strSplit = EvaluationItemValue.Split('_');

            decimal ValueToEval = ItemToEval.ItemInfo.
                Where(itinf => itinf.ItemInfoType.ItemId.ToString() == strSplit[0].Replace(" ", "")).
                Select(itinf => ConvertToDecimal(itinf.Value)).
                DefaultIfEmpty(0).
                FirstOrDefault();

            switch (strSplit[2].Replace(" ", ""))
            {
                case "1409001":
                    oReturn = ValueToEval == Convert.ToDecimal(strSplit[1].Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"));
                    break;
                case "1409002":
                    oReturn = ValueToEval > Convert.ToDecimal(strSplit[1].Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"));
                    break;
                case "1409003":
                    oReturn = ValueToEval >= Convert.ToDecimal(strSplit[1].Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"));
                    break;
                default:
                    break;
            }

            return oReturn;
        }

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> CreateStandarResponse
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem,
            Dictionary<int, string> oResultInfos)
        {

            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn =
                new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>();

            oResultInfos.All(ri =>
            {
                oReturn.Add(new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
                {
                    ItemInfoId = vProjectProvider.ItemInfo == null ? 0 :
                        vProjectProvider.ItemInfo.
                        Where(pjpvinf => pjpvinf.ItemInfoType.ItemId == ri.Key &&
                                        pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItem.ItemId).
                        Select(pjpvinf => pjpvinf.ItemInfoId).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),

                    RelatedEvaluationItem = new Company.Models.Util.GenericItemModel()
                    {
                        ItemId = vEvaluationItem.ItemId,
                    },
                    ItemInfoType = new Company.Models.Util.CatalogModel()
                    {
                        ItemId = ri.Key,
                    },
                    Value = ri.Value,
                    Enable = true,
                });

                return true;
            });

            return oReturn;
        }

        private decimal ConvertToDecimal(string strToEval)
        {
            decimal oReturn = 0;
            if (!string.IsNullOrEmpty(strToEval))
            {
                if (strToEval.Contains("http"))
                    oReturn = 1;
                else
                    oReturn = Convert.ToDecimal(strToEval.Replace(" ", ""), System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"));
            }
            return oReturn;
        }

        #endregion

    }
}
