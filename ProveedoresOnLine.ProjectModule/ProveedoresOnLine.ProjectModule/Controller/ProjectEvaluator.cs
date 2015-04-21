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

        #region Project Infos

        private decimal? oProjectAmmount;
        public decimal ProjectAmmount
        {
            get
            {
                if (oProjectAmmount == null)
                {
                    oProjectAmmount = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == 1407002 && !string.IsNullOrEmpty(pjinf.Value)).
                        Select(pjinf => GetDecimalFromValue(pjinf.Value)).
                        DefaultIfEmpty(0).
                        FirstOrDefault();
                }
                return oProjectAmmount.Value;
            }
        }

        private string oProjectExperienceYear;
        public string ProjectExperienceYear
        {
            get
            {
                if (oProjectExperienceYear == null)
                {
                    oProjectExperienceYear = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == 1407003 && !string.IsNullOrEmpty(pjinf.Value)).
                        Select(pjinf => pjinf.Value.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }
                return oProjectExperienceYear;
            }
        }

        private int? oProjectExperienceCount;
        public int ProjectExperienceCount
        {
            get
            {
                if (oProjectExperienceCount == null)
                {
                    oProjectExperienceCount = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == 1407004 && !string.IsNullOrEmpty(pjinf.Value)).
                        Select(pjinf => Convert.ToInt32(pjinf.Value.Replace(" ", ""))).
                        DefaultIfEmpty(0).
                        FirstOrDefault();
                }
                return oProjectExperienceCount.Value;
            }
        }

        private List<string> oProjectDefaultEconomicActivity;
        public List<string> ProjectDefaultEconomicActivity
        {
            get
            {
                if (oProjectDefaultEconomicActivity == null)
                {
                    oProjectDefaultEconomicActivity = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == 1407005 && !string.IsNullOrEmpty(pjinf.LargeValue)).
                        Select(pjinf => pjinf.LargeValue.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault().
                        Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                        ToList();

                    if (oProjectDefaultEconomicActivity == null)
                        oProjectDefaultEconomicActivity = new List<string>();
                }
                return oProjectDefaultEconomicActivity;
            }
        }

        private List<string> oProjectCustomEconomicActivity;
        public List<string> ProjectCustomEconomicActivity
        {
            get
            {
                if (oProjectCustomEconomicActivity == null)
                {
                    oProjectCustomEconomicActivity = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == 1407006 && !string.IsNullOrEmpty(pjinf.LargeValue)).
                        Select(pjinf => pjinf.LargeValue.Replace(" ", "")).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault().
                        Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                        ToList();

                    if (oProjectCustomEconomicActivity == null)
                        oProjectCustomEconomicActivity = new List<string>();
                }
                return oProjectCustomEconomicActivity;
            }
        }


        private int? oProjectCurrency;
        public int ProjectCurrency
        {
            get
            {
                if (oProjectCurrency == null)
                {
                    oProjectCurrency = RelatedProject.ProjectInfo.
                        Where(pjinf => pjinf.ItemInfoType.ItemId == 1407007 && !string.IsNullOrEmpty(pjinf.Value)).
                        Select(pjinf => Convert.ToInt32(pjinf.Value.Replace(" ", ""))).
                        DefaultIfEmpty(DefaultCurrency).
                        FirstOrDefault();
                }
                return oProjectCurrency.Value;
            }
        }


        #endregion

        public int DefaultCurrency
        {
            get
            {
                return Convert.ToInt32(ProveedoresOnLine.Company.Models.Util.InternalSettings.Instance
                    [ProveedoresOnLine.Company.Models.Constants.C_Settings_CurrencyExchange_USD].Value);
            }
        }

        #endregion

        #region Constructors

        public ProjectEvaluator(string oProjectPublicId)
        {
            //get project calculate info
            RelatedProject = ProjectModule.ProjectGetByIdCalculate(oProjectPublicId);
        }

        #endregion

        public void InitEval()
        {
            RelatedProject.RelatedProjectProvider.All(pjpv =>
            {
                LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                try
                {
                    //loop for evaluation criteria

                    List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oResult = new List<Models.ProjectProviderInfoModel>();
                    List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oTmpResult = new List<Models.ProjectProviderInfoModel>();

                    RelatedProject.RelatedProjectConfig.RelatedEvaluationItem.
                    Where(ei => ei.ItemType.ItemId == 1401002).
                    All(ei =>
                    {
                        try
                        {
                            switch (ei.ItemInfo.
                                        Where(eiinf => eiinf.ItemInfoType.ItemId == 1402005).
                                        Select(eiinf => string.IsNullOrEmpty(eiinf.Value) ? 0 : Convert.ToInt32(eiinf.Value)).
                                        DefaultIfEmpty(0).
                                        FirstOrDefault())
                            {
                                case 1404001:
                                    oTmpResult = Commercial_EvalExperience(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                case 1404002:
                                    oTmpResult = Certification_EvalNorms(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                case 1404003:
                                    oTmpResult = Certification_EvalLTIF(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                case 1404004:
                                    oTmpResult = Certification_EvalRiskPolicies(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                case 1404005:
                                    oTmpResult = Financial_EvalBalanceSheet(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                case 1404006:
                                    oTmpResult = Legal_EvalChamberOfCommerce(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                case 1404007:
                                    oTmpResult = Legal_EvalRut(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                case 1404008:
                                    oTmpResult = Legal_EvalSARLAFT(pjpv, ei);
                                    if (oTmpResult != null && oTmpResult.Count > 0)
                                    {
                                        oResult.AddRange(oTmpResult);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }

                        return true;
                    });

                    //recalculate area results
                    if (oResult == null)
                        oResult = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>();

                    oResult.AddRange(AreaResults(pjpv, oResult));


                    //create upsert model
                    ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel oProjectProviderToUpsert =
                        new ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel()
                        {
                            ProjectCompanyId = pjpv.ProjectCompanyId,
                            ItemInfo = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>(),
                        };

                    //get new items to upsert
                    oProjectProviderToUpsert.ItemInfo.AddRange
                        (oResult.Where(ors => ors.ItemInfoId == 0));

                    //get change value items
                    oProjectProviderToUpsert.ItemInfo.AddRange
                        (oResult.
                            Where(ors => ors.ItemInfoId > 0 &&
                                        ors.Value !=
                                            pjpv.ItemInfo.
                                            Where(pjpvinf => pjpvinf.ItemInfoId == ors.ItemInfoId && !string.IsNullOrEmpty(pjpvinf.Value)).
                                            Select(pjpvinf => pjpvinf.Value).
                                            DefaultIfEmpty(string.Empty).
                                            FirstOrDefault()));

                    //upsert evaluation items responses
                    oProjectProviderToUpsert = ProjectModule.ProjectCompanyInfoUpsert(oProjectProviderToUpsert);
                }
                catch (Exception err)
                {
                    oLog.IsSuccess = false;
                    oLog.Message = err.Message + " - " + err.StackTrace;

                    throw err;
                }
                finally
                {
                    oLog.LogObject = pjpv;
                    LogManager.ClientLog.AddLog(oLog);
                }

                return true;
            });
        }

        #region Commercial

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> Commercial_EvalExperience
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel vEvaluationItem)
        {
            //get experience to eval
            var oExperienceToEval = vProjectProvider.RelatedProvider.RelatedCommercial.
                Where(cm => cm.ItemType.ItemId == 301001 &&
                            (!string.IsNullOrEmpty(ProjectExperienceYear) && ProjectExperienceYear.Split('_').Length >= 3 ?
                                cm.ItemInfo.Any(cminf =>
                                        !string.IsNullOrEmpty(cminf.Value) &&
                                        cminf.ItemInfoType.ItemId.ToString() == ProjectExperienceYear.Split('_')[0].Replace(" ", "") &&
                                        InternalComparisonValidation(
                                            ProjectExperienceYear.Split('_')[2],
                                            Convert.ToDateTime(cminf.Value).Year,
                                            DateTime.Now.Year - GetDecimalFromValue(ProjectExperienceYear.Split('_')[1])))
                                : true

                            ) &&
                            (ProjectDefaultEconomicActivity != null && ProjectDefaultEconomicActivity.Count > 0 ?
                                cm.ItemInfo.Any(cminf => cminf.ItemInfoType.ItemId == 302012 &&
                                                    !string.IsNullOrEmpty(cminf.LargeValue) &&
                                                    cminf.LargeValue.Split(',').Any(dea =>
                                                        ProjectDefaultEconomicActivity.Any(deate => deate.Replace(" ", "") == dea.Replace(" ", ""))))
                                : true
                            ) &&
                            (ProjectCustomEconomicActivity != null && ProjectCustomEconomicActivity.Count > 0 ?
                                cm.ItemInfo.Any(cminf => cminf.ItemInfoType.ItemId == 302013 &&
                                                    !string.IsNullOrEmpty(cminf.LargeValue) &&
                                                    cminf.LargeValue.Split(',').Any(cta =>
                                                        ProjectCustomEconomicActivity.Any(ctate => cta.Replace(" ", "") == ctate.Replace(" ", ""))))
                                : true
                            )
                    ).
                Select(cm => new
                {
                    ExperienceId = cm.ItemId,
                    ExperienceValue = cm.ItemInfo.
                        Where(cminf => cminf.ItemInfoType.ItemId == 302007 && !string.IsNullOrEmpty(cminf.Value)).
                        Select(cminf => GetDecimalFromValue(cminf.Value)).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),
                    ExperienceCurrency = cm.ItemInfo.
                        Where(cminf => cminf.ItemInfoType.ItemId == 302002 && !string.IsNullOrEmpty(cminf.Value)).
                        Select(cminf => Convert.ToInt32(cminf.Value.Replace(" ", ""))).
                        DefaultIfEmpty(DefaultCurrency).
                        FirstOrDefault(),
                    ExperienceYear = cm.ItemInfo.
                        Where(cminf => cminf.ItemInfoType.ItemId == 302003 && !string.IsNullOrEmpty(cminf.Value)).
                        Select(cminf => Convert.ToDateTime(cminf.Value.Replace(" ", "")).Year).
                        DefaultIfEmpty(0).
                        FirstOrDefault(),
                });

            //get ammount
            decimal oExpSum = oExperienceToEval.
                Sum(exp => exp.ExperienceValue * CurrencyExchangeGetRate
                        (exp.ExperienceCurrency, ProjectCurrency, exp.ExperienceYear));

            //get experience itemif
            string oExpItemId = string.Join(",", oExperienceToEval.Select(exp => exp.ExperienceId));

            //Response - Create project company info object to upsert
            Dictionary<int, string> oValues = new Dictionary<int, string>();
            oValues.Add(1408001, oExpSum >= ProjectAmmount && oExperienceToEval.Count() >= ProjectExperienceCount ? "100" : "0");
            oValues.Add(1408002, oExpItemId);

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
                                GetDecimalFromValue(eiinf.Value.Split('_')[1]),
                                Convert.ToInt32(eiinf.Value.Split('_')[2].Replace(" ", "")),
                                GetDecimalFromValue(eiinf.Value.Split('_')[3]))).
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

        #region AreaResults

        private List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> AreaResults
            (ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProvider,
            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> vEvaluationItemResults)
        {

            List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn = new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>();

            //loop for all areas
            RelatedProject.RelatedProjectConfig.RelatedEvaluationItem.
            Where(ei => ei.ItemType.ItemId == 1401001 &&
                        ei.ItemInfo.Any(eiinf =>
                            eiinf.ItemInfoType.ItemId == 1402003 &&
                            !string.IsNullOrEmpty(eiinf.Value) &&
                            (eiinf.Value.Replace(" ", "") == "1403001" || eiinf.Value.Replace(" ", "") == "1403002"))).
            All(ei =>
            {
                //get evaluation item unit
                int oeiUnit = ei.ItemInfo.
                    Where(eiinf => eiinf.ItemInfoType.ItemId == 1402003 &&
                                    !string.IsNullOrEmpty(eiinf.Value)).
                    Select(eiinf => Convert.ToInt32(eiinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(0).
                    FirstOrDefault();

                //loop for child evaluation
                var EvaluationItemChild = RelatedProject.RelatedProjectConfig.RelatedEvaluationItem.
                        Where(eichild => eichild.ItemType.ItemId == 1401002 &&
                                        eichild.ParentItem != null &&
                                        eichild.ParentItem.ItemId == ei.ItemId).
                        Select(eichild => new
                        {
                            EvaluationItemId = eichild.ItemId,
                            EvaluationItemWeight = eichild.ItemInfo.
                                Where(eichildinf => eichildinf.ItemInfoType.ItemId == 1402004 &&
                                                    !string.IsNullOrEmpty(eichildinf.Value)).
                                Select(eichildinf => GetDecimalFromValue(eichildinf.Value)).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),
                            EvaluationItemResult = vEvaluationItemResults.
                                Where(eir => eir.RelatedEvaluationItem.ItemId == eichild.ItemId &&
                                            eir.ItemInfoType.ItemId == 1408001 &&
                                            !string.IsNullOrEmpty(eir.Value)).
                                Select(eir => GetDecimalFromValue(eir.Value.Replace(" ", ""))).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),
                        });

                decimal oResult = 0;

                if (oeiUnit == 1403001)
                {
                    oResult = EvaluationItemChild.Any(eic => eic.EvaluationItemResult == 0) ? 0 : 100;
                }
                else if (oeiUnit == 1403002)
                {
                    oResult = EvaluationItemChild.Sum(eic => eic.EvaluationItemResult * eic.EvaluationItemWeight / 100);
                }

                //Response - Create project company info object to upsert
                Dictionary<int, string> oValues = new Dictionary<int, string>();
                oValues.Add(1408001, oResult.ToString("0.##"));

                //get standar response
                oReturn.AddRange(CreateStandarResponse(vProjectProvider, ei, oValues));

                return true;
            });

            return oReturn;
        }

        #endregion

        #region Util

        private bool ValidateCondition
            (string EvaluationItemValue,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel ItemToEval)
        {
            string[] strSplit = EvaluationItemValue.Split('_');

            decimal ValueToEval = ItemToEval.ItemInfo.
                Where(itinf => itinf.ItemInfoType.ItemId.ToString() == strSplit[0].Replace(" ", "") &&
                                !string.IsNullOrEmpty(itinf.Value)).
                Select(itinf => GetDecimalFromValue(itinf.Value)).
                DefaultIfEmpty(0).
                FirstOrDefault();

            decimal ValueToCompare = GetDecimalFromValue(strSplit[1]);

            return InternalComparisonValidation(strSplit[2], ValueToEval, ValueToCompare);
        }

        private bool InternalComparisonValidation
            (string Operator, decimal ValueToEval, decimal ValueToCompare)
        {
            bool oReturn = false;

            switch (Operator.Replace(" ", ""))
            {
                case "1409001":
                    oReturn = ValueToEval == ValueToCompare;
                    break;
                case "1409002":
                    oReturn = ValueToEval > ValueToCompare;
                    break;
                case "1409003":
                    oReturn = ValueToEval >= ValueToCompare;
                    break;
                default:
                    break;
            }
            return oReturn;
        }

        private decimal GetDecimalFromValue(string strToEval)
        {
            decimal oReturn = 0;
            if (!string.IsNullOrEmpty(strToEval))
            {
                if (strToEval.Contains("http"))
                {
                    oReturn = 1;
                }
                else if (string.IsNullOrEmpty(strToEval))
                {
                    oReturn = 0;
                }
                else
                {
                    decimal.TryParse(strToEval.Replace(" ", ""),
                        System.Globalization.NumberStyles.None,
                        System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"),
                        out oReturn);
                }
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

        #region Currency
        //MoneyFrom,MoneyTo,Year,Result
        private List<Tuple<int, int, int, decimal>> oConsultCurrency;

        private decimal CurrencyExchangeGetRate
            (int MoneyFrom,
            int MoneyTo,
            int Year)
        {
            if (oConsultCurrency == null)
                oConsultCurrency = new List<Tuple<int, int, int, decimal>>();

            if (!oConsultCurrency.Any(cc => cc.Item1 == MoneyFrom &&
                                        cc.Item2 == MoneyTo &&
                                        cc.Item3 == Year))
            {
                decimal oValue = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate(MoneyFrom, MoneyTo, Year);
                oConsultCurrency.Add(new Tuple<int, int, int, decimal>(MoneyFrom, MoneyTo, Year, oValue));
            }

            return oConsultCurrency.
                Where(cc => cc.Item1 == MoneyFrom &&
                            cc.Item2 == MoneyTo &&
                            cc.Item3 == Year).
                Select(cc => cc.Item4).
                DefaultIfEmpty(1).
                FirstOrDefault();
        }

        #endregion

        #endregion

    }
}
