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
                            break;
                        case 1404002:
                            var oResult = Certification_EvalNorms(pjpv, ei);
                            if (oResult != null && oResult.Count > 0)
                            {
                                oProjectProviderToUpsert.ItemInfo.AddRange(oResult);
                            }
                            break;
                        case 1404003:
                            break;
                        case 1404004:
                            break;
                        case 1404005:
                            break;
                        case 1404006:
                            break;
                        case 1404007:
                            break;
                        case 1404008:
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
                Select(itinf => string.IsNullOrEmpty(itinf.Value) ? 0 : Convert.ToDecimal(itinf.Value.Replace(" ", ""))).
                DefaultIfEmpty(0).
                FirstOrDefault();

            switch (strSplit[2].Replace(" ", ""))
            {
                case "1409001":
                    oReturn = ValueToEval == Convert.ToDecimal(strSplit[1].Replace(" ", ""));
                    break;
                case "1409002":
                    oReturn = ValueToEval > Convert.ToDecimal(strSplit[1].Replace(" ", ""));
                    break;
                case "1409003":
                    oReturn = ValueToEval >= Convert.ToDecimal(strSplit[1].Replace(" ", ""));
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

            //List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel> oReturn =
            //    new List<ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel>()
            //{
            //    //add ratting item
            //    new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
            //    {
            //        ItemInfoId = vProjectProvider.ItemInfo == null ? 0 :
            //            vProjectProvider.ItemInfo.
            //            Where(pjpvinf => pjpvinf.ItemInfoType.ItemId == 1408001 &&
            //                            pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItem.ItemId).
            //            Select(pjpvinf => pjpvinf.ItemInfoId).
            //            DefaultIfEmpty(0).
            //            FirstOrDefault(),

            //        RelatedEvaluationItem = new Company.Models.Util.GenericItemModel()
            //        {
            //            ItemId = vEvaluationItem.ItemId,
            //        },
            //        ItemInfoType = new Company.Models.Util.CatalogModel()
            //        {
            //            ItemId = 1408001,
            //        },
            //        Value = oValidCertificationId == null? "0":"100",
            //        Enable = true,
            //    },

            //    //add provider item 
            //    new ProveedoresOnLine.ProjectModule.Models.ProjectProviderInfoModel()
            //    {
            //        ItemInfoId = vProjectProvider.ItemInfo == null ? 0 :
            //            vProjectProvider.ItemInfo.
            //            Where(pjpvinf => pjpvinf.ItemInfoType.ItemId == 1408002 &&
            //                            pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItem.ItemId).
            //            Select(pjpvinf => pjpvinf.ItemInfoId).
            //            DefaultIfEmpty(0).
            //            FirstOrDefault(),

            //        RelatedEvaluationItem = new Company.Models.Util.GenericItemModel()
            //        {
            //            ItemId = vEvaluationItem.ItemId,
            //        },
            //        ItemInfoType = new Company.Models.Util.CatalogModel()
            //        {
            //            ItemId = 1408002,
            //        },
            //        Value = oValidCertificationId == null ? string.Empty : oValidCertificationId.Value.ToString(),
            //        Enable = true,
            //    },

            //};

        }

        #endregion

    }
}
