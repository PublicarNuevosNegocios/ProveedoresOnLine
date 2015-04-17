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

        public ProveedoresOnLine.ProjectModule.Models.ProjectModel RelatedProjectInfo { get; private set; }

        #endregion

        #region Constructors

        public ProjectEvaluator(string oProjectPublicId, string oCustomerPublicId)
        {
            //get project calculate info
            RelatedProjectInfo = ProjectModule.ProjectGetByIdCalculate(oProjectPublicId, oCustomerPublicId);
        }

        #endregion

        public void InitEval()
        {
            RelatedProjectInfo.RelatedProjectProvider.All(pjpv =>
            {
                EvalCertification(pjpv);
                return true;
            });
        }

        #region Certification

        private void EvalCertification(ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel vProjectProviderModel)
        {
            bool oEvalNorms = RelatedProjectInfo.RelatedProjectConfig.RelatedEvaluationItem.
                Any(ei => ei.ItemInfo.
                    Any(eiinf => eiinf.ItemInfoType.ItemId == 1402005 && eiinf.Value == "1404002"));


            //1404002 HSEQ - Norms
            if (vProjectProviderModel.RelatedProvider.RelatedCertification != null &&
                RelatedProjectInfo.RelatedProjectConfig.RelatedEvaluationItem.
                Any(ei => ei.ItemInfo.
                    Any(eiinf => eiinf.ItemInfoType.ItemId == 1402005 && eiinf.Value == "1404002")))
            {
                //all evaluations for norms
                RelatedProjectInfo.RelatedProjectConfig.RelatedEvaluationItem.
                Where(ei => ei.ItemInfo.
                        Any(eiinf => eiinf.ItemInfoType.ItemId == 1402005 && eiinf.Value == "1404002")).
                All(ei =>
                {
                    //loop all certifications norms
                    vProjectProviderModel.RelatedProvider.RelatedCertification.
                        Where(lg => lg.ItemType.ItemId == 701001 &&
                                    lg.ItemInfo.Any(lginf => ei.ItemInfo.
                                        Any(eiinf => eiinf.ItemInfoType.ItemId == 1402006 &&
                                                    lginf.ItemInfoType.ItemId.ToString() == eiinf.Value.Split('_').
                                                        DefaultIfEmpty(string.Empty).
                                                        FirstOrDefault()))).
                        All(lg =>
                        {
                            //validate norm

                            //get expiration date
                            DateTime oExpirationDate = lg.ItemInfo.
                                Where(lginf => lginf.ItemInfoType.ItemId == 702004).
                                Select(lginf => string.IsNullOrEmpty(lginf.Value) ? DateTime.MinValue : Convert.ToDateTime(lginf.Value)).
                                DefaultIfEmpty(DateTime.MinValue).
                                FirstOrDefault();

                            if (oExpirationDate >= DateTime.Now)
                            {
                                //validate Infos
                                bool oValidInfos = true;

                                ei.ItemInfo.Where(eiinf => eiinf.ItemInfoType.ItemId == 1402006).All(eiinf =>
                                {
                                    //get infotype values
                                    string[] strSplit = eiinf.Value.Split('_');

                                    if (strSplit.Length >= 3)
                                    {
                                        string oValue = lg.ItemInfo.
                                            Where(lginf => lginf.ItemInfoType.ItemId.ToString() == strSplit[0]).
                                            Select(lginf => !string.IsNullOrEmpty(lginf.Value) ? lginf.Value : (!string.IsNullOrEmpty(lginf.LargeValue) ? lginf.LargeValue : string.Empty)).
                                            DefaultIfEmpty(string.Empty).
                                            FirstOrDefault();

                                        if (!string.IsNullOrEmpty(oValue))
                                        {
                                            switch (strSplit[2])
                                            {
                                                case "1409001":
                                                    oValidInfos = Convert.ToDecimal(oValue) == Convert.ToDecimal(strSplit[1]);
                                                    break;
                                                case "1409002":
                                                    oValidInfos = Convert.ToDecimal(oValue) > Convert.ToDecimal(strSplit[1]);
                                                    break;
                                                case "1409003":
                                                    oValidInfos = Convert.ToDecimal(oValue) >= Convert.ToDecimal(strSplit[1]);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    return true;
                                });
                            }
                            return true;
                        });
                    return true;
                });
            }
        }

        #endregion

    }
}
