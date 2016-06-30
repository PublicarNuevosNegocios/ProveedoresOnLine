using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.CalificationProjectModule
{
    public class CommercialModule
    {
        public static ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel CommercialRule(string CompanyPublicId, ConfigItemModel oCalificationProjectItemModel, CalificationProjectItemBatchModel oRelatedCalificationProjectItemModel)
        {
            ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Commercial Module in Process::");
            CalificationProjectItemBatchModel oReturn = new CalificationProjectItemBatchModel()
            {
                CalificationProjectItemId = oRelatedCalificationProjectItemModel != null && oRelatedCalificationProjectItemModel.CalificationProjectItemId > 0 ? oRelatedCalificationProjectItemModel.CalificationProjectItemId : 0,
                CalificationProjectConfigItem = new ConfigItemModel()
                {
                    CalificationProjectConfigItemId = oRelatedCalificationProjectItemModel != null && oRelatedCalificationProjectItemModel.CalificationProjectConfigItem != null ? oRelatedCalificationProjectItemModel.CalificationProjectConfigItem.CalificationProjectConfigItemId : oCalificationProjectItemModel.CalificationProjectConfigItemId,
                },
                CalificatioProjectItemInfoModel = new List<CalificationProjectItemInfoBatchModel>(),
                ItemScore = oRelatedCalificationProjectItemModel != null ? oRelatedCalificationProjectItemModel.ItemScore : 0,
                Enable = true,
            };
            
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCommercialProviderInfo;
            
            #region Variables

            int oTotalModuleScore = 0;
            int CommercialScore = 0;
            int RuleScore = 0;
            int oIntValue = 0;
            bool oBooleanValue = true;
            double oPercentValue = 0;
            DateTime oDateValue = new DateTime();
            string oTextValue = "";

            #endregion

            try
            {
                if (oRelatedCalificationProjectItemModel != null &&
                    oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel != null &&
                    oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Count > 0)
                {
                    oReturn.CalificatioProjectItemInfoModel = oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel;
                }

                if (oReturn.CalificatioProjectItemInfoModel != null &&
                    oReturn.CalificatioProjectItemInfoModel.Count > 0)
                {
                    //Validate if module is not enable
                    if (oRelatedCalificationProjectItemModel.CalificationProjectConfigItem.Enable)
                    {
                        //Validate rule config with mp rule
                        oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.All(mprule =>
                        {
                            bool mpEnable = false;

                            oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.Where(cnfrule => cnfrule.Enable == true).All(cnfrule =>
                            {
                                if (!mpEnable && mprule.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId == cnfrule.CalificationProjectConfigItemInfoId)
                                {
                                    mpEnable = true;
                                }

                                return true;
                            });

                            mprule.Enable = mpEnable;

                            return true;
                        });
                    }
                    else
                    {
                        //disable all params
                        oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.All(mprule =>
                        {
                            mprule.Enable = false;
                            mprule.CalificationProjectConfigItemInfoModel.Enable = false;
                            return true;
                        });
                    }

                    oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.Where(rule => rule.Enable == true).All(rule =>
                    {
                        ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Update validate to Commercial module ::: Provider public id ::: " + CompanyPublicId + " ::: RuleId ::: " + rule.CalificationProjectConfigItemInfoId);

                        if (oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Any(mprule => mprule.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId == rule.CalificationProjectConfigItemInfoId))
                        {
                            oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Where(mprule => mprule.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId == rule.CalificationProjectConfigItemInfoId).All(mprule =>
                            {
                                //add mp rule
                                oCommercialProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CommercialModuleInfo(CompanyPublicId, rule.Question.ItemId);

                                oCommercialProviderInfo.Where(pinf => pinf != null).All(pinf =>
                                {
                                    if (RuleScore <= 0)
                                    {
                                        switch (rule.Rule.ItemId)
                                        {
                                            #region Positivo

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue > 0)
                                                {
                                                    CommercialScore = int.Parse(rule.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    mprule.ItemInfoScore = CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Negativo

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue < 0)
                                                {
                                                    CommercialScore = int.Parse(rule.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    mprule.ItemInfoScore = CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region MayorQue

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: numérico

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oIntValue > int.Parse(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue > Convert.ToDateTime(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue > Convert.ToDouble(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                break;

                                            #endregion

                                            #region MenorQue

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorQue:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: numérico

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oIntValue < int.Parse(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue < Convert.ToDateTime(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue < Convert.ToDouble(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                break;

                                            #endregion

                                            #region MayorOIgual

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorOIgual:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: numérico

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oIntValue >= int.Parse(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue >= Convert.ToDateTime(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue >= Convert.ToDouble(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                break;

                                            #endregion

                                            #region MenorOIgual

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorOIgual:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: numérico

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oIntValue <= int.Parse(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue <= Convert.ToDateTime(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue <= Convert.ToDouble(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                break;

                                            #endregion

                                            #region IgualQue

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.IgualQue:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: numérico

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oIntValue == int.Parse(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue == Convert.ToDateTime(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue == Convert.ToDouble(rule.Value))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: texto

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                        oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oTextValue == rule.Value)
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                break;

                                            #endregion

                                            #region Entre

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Entre:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: numérico

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                        int minValue = 0;
                                                        int maxValue = 0;

                                                        string[] oValue = rule.Value.Split(',');

                                                        minValue = int.Parse(oValue[0]);
                                                        maxValue = int.Parse(oValue[1]);

                                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oIntValue < maxValue && oIntValue > minValue)
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        DateTime oMinValue;
                                                        DateTime oMaxValue;

                                                        oValue = rule.Value.Split(',');

                                                        oMinValue = Convert.ToDateTime(oValue[0].Trim());
                                                        oMaxValue = Convert.ToDateTime(oValue[1].Trim());

                                                        if (oDateValue < oMaxValue && oDateValue > oMinValue)
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        double oMiniValue;
                                                        double oMaxiValue;

                                                        oValue = rule.Value.Split(',');

                                                        oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                        oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                        if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                break;

                                            #endregion

                                            #region Pasa / No pasa

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.PasaNoPasa:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: archivo

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.File:

                                                        bool oRelatedFile = !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue) ? true : false;

                                                        if (oRelatedFile)
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: texto

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                        if (!string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue))
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: booleano

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Boolean:

                                                        oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value);

                                                        oBooleanValue = !string.IsNullOrEmpty(oTextValue) && (oTextValue == "1" || oTextValue == "True" || oTextValue == "true") ? true : false;

                                                        if (oBooleanValue)
                                                        {
                                                            CommercialScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += CommercialScore;

                                                            mprule.ItemInfoScore = CommercialScore;
                                                        }
                                                        else
                                                        {
                                                            CommercialScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                break;

                                            #endregion
                                        }
                                    }
                                    return true;
                                });

                                RuleScore = 0;

                                return true;
                            });
                        }
                        else
                        {
                            //run rule
                            oCommercialProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CommercialModuleInfo(CompanyPublicId, rule.Question.ItemId);

                            oCommercialProviderInfo.Where(pinf => pinf != null).All(pinf =>
                            {
                                if (RuleScore <= 0)
                                {
                                    switch (rule.Rule.ItemId)
                                    {
                                        #region Positivo

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                                            oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                            if (oIntValue >= 0)
                                            {
                                                CommercialScore = int.Parse(rule.Score);

                                                RuleScore++;

                                                oTotalModuleScore += CommercialScore;

                                                oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                {
                                                    CalificationProjectItemInfoId = 0,
                                                    CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                    {
                                                        CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                    },
                                                    ItemInfoScore = CommercialScore,
                                                    Enable = true,
                                                });
                                            }
                                            else
                                            {
                                                CommercialScore = 0;
                                            }

                                            break;

                                        #endregion

                                        #region Negativo

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                            oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                            if (oIntValue < 0)
                                            {
                                                CommercialScore = int.Parse(rule.Score);

                                                RuleScore++;

                                                oTotalModuleScore += CommercialScore;

                                                oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                {
                                                    CalificationProjectItemInfoId = 0,
                                                    CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                    {
                                                        CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                    },
                                                    ItemInfoScore = CommercialScore,
                                                    Enable = true,
                                                });
                                            }
                                            else
                                            {
                                                CommercialScore = 0;
                                            }

                                            break;

                                        #endregion

                                        #region MayorQue

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:

                                            switch (rule.ValueType.ItemId)
                                            {
                                                #region Tipo valor: numérico

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                    oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oIntValue > int.Parse(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue > Convert.ToDateTime(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oPercentValue > Convert.ToDouble(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            break;

                                        #endregion

                                        #region MenorQue

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorQue:

                                            switch (rule.ValueType.ItemId)
                                            {
                                                #region Tipo valor: numérico

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                    oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oIntValue < int.Parse(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue < Convert.ToDateTime(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oPercentValue < Convert.ToDouble(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            break;

                                        #endregion

                                        #region MayorOIgual

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorOIgual:

                                            switch (rule.ValueType.ItemId)
                                            {
                                                #region Tipo valor: numérico

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                    oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oIntValue >= int.Parse(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue >= Convert.ToDateTime(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oPercentValue >= Convert.ToDouble(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            break;

                                        #endregion

                                        #region MenorOIgual

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorOIgual:

                                            switch (rule.ValueType.ItemId)
                                            {
                                                #region Tipo valor: numérico

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                    oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oIntValue <= int.Parse(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue <= Convert.ToDateTime(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oPercentValue <= Convert.ToDouble(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            break;

                                        #endregion

                                        #region IgualQue

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.IgualQue:

                                            switch (rule.ValueType.ItemId)
                                            {
                                                #region Tipo valor: numérico

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                    oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oIntValue == int.Parse(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue == Convert.ToDateTime(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oPercentValue == Convert.ToDouble(rule.Value))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: texto

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                    oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oTextValue == rule.Value)
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);
                                                        RuleScore++;
                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            break;

                                        #endregion

                                        #region Entre

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Entre:

                                            switch (rule.ValueType.ItemId)
                                            {
                                                #region Tipo valor: numérico

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                    int minValue = 0;
                                                    int maxValue = 0;

                                                    string[] oValue = rule.Value.Split(',');

                                                    minValue = int.Parse(oValue[0]);
                                                    maxValue = int.Parse(oValue[1]);

                                                    oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oIntValue < maxValue && oIntValue > minValue)
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                    DateTime oMinValue;
                                                    DateTime oMaxValue;

                                                    oValue = rule.Value.Split(',');

                                                    oMinValue = Convert.ToDateTime(oValue[0].Trim());
                                                    oMaxValue = Convert.ToDateTime(oValue[1].Trim());

                                                    if (oDateValue < oMaxValue && oDateValue > oMinValue)
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                    double oMiniValue;
                                                    double oMaxiValue;

                                                    oValue = rule.Value.Split(',');

                                                    oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                    oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                    if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            break;

                                        #endregion

                                        #region Pasa / No pasa

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.PasaNoPasa:

                                            switch (rule.ValueType.ItemId)
                                            {
                                                #region Tipo valor: archivo

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.File:

                                                    bool oRelatedFile = !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue) ? true : false;

                                                    if (oRelatedFile)
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: texto

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                    if (!string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue))
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: booleano

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Boolean:

                                                    oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value);

                                                    oBooleanValue = !string.IsNullOrEmpty(oTextValue) && (oTextValue == "1" || oTextValue == "True" || oTextValue == "true") ? true : false;

                                                    if (oBooleanValue)
                                                    {
                                                        CommercialScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += CommercialScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = CommercialScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        CommercialScore = 0;
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            break;

                                        #endregion
                                    }
                                }

                                return true;
                            });
                        }

                        RuleScore = 0;

                        return true;
                    });
                }
                else
                {
                    oReturn.CalificatioProjectItemInfoModel = new List<CalificationProjectItemInfoBatchModel>();

                    oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.Where(cpitinf => cpitinf.Enable == true).All(cpitinf =>
                    {
                        ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Create validate to Commercial module ::: Provider public id ::: " + CompanyPublicId + " ::: RuleId ::: " + cpitinf.CalificationProjectConfigItemInfoId);

                        oCommercialProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CommercialModuleInfo(CompanyPublicId, cpitinf.Question.ItemId);

                        oCommercialProviderInfo.Where(pinf => pinf != null).All(pinf =>
                        {
                            if (RuleScore <= 0)
                            {
                                switch (cpitinf.Rule.ItemId)
                                {
                                    #region Positivo

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                        if (oIntValue >= 0)
                                        {
                                            CommercialScore = int.Parse(cpitinf.Score);

                                            RuleScore++;

                                            oTotalModuleScore += CommercialScore;

                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                            {
                                                CalificationProjectItemInfoId = 0,
                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                {
                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                },
                                                ItemInfoScore = CommercialScore,
                                                Enable = true,
                                            });
                                        }
                                        else
                                        {
                                            CommercialScore = 0;
                                        }

                                        break;

                                    #endregion

                                    #region Negativo

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                        if (oIntValue < 0)
                                        {
                                            CommercialScore = int.Parse(cpitinf.Score);

                                            RuleScore++;

                                            oTotalModuleScore += CommercialScore;

                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                            {
                                                CalificationProjectItemInfoId = 0,
                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                {
                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                },
                                                ItemInfoScore = CommercialScore,
                                                Enable = true,
                                            });
                                        }
                                        else
                                        {
                                            CommercialScore = 0;
                                        }

                                        break;

                                    #endregion

                                    #region MayorQue

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:

                                        switch (cpitinf.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue > int.Parse(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue > Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue > Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region MenorQue

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorQue:

                                        switch (cpitinf.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue < int.Parse(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue < Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue < Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region MayorOIgual

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorOIgual:

                                        switch (cpitinf.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue >= int.Parse(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue >= Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue >= Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region MenorOIgual

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorOIgual:

                                        switch (cpitinf.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue <= int.Parse(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue <= Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue <= Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region IgualQue

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.IgualQue:

                                        switch (cpitinf.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue == int.Parse(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue == Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue == Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: texto

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oTextValue == cpitinf.Value)
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);
                                                    RuleScore++;
                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region Entre

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Entre:

                                        switch (cpitinf.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                int minValue = 0;
                                                int maxValue = 0;

                                                string[] oValue = cpitinf.Value.Split(',');

                                                minValue = int.Parse(oValue[0]);
                                                maxValue = int.Parse(oValue[1]);

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue < maxValue && oIntValue > minValue)
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                DateTime oMinValue;
                                                DateTime oMaxValue;

                                                oValue = cpitinf.Value.Split(',');

                                                oMinValue = Convert.ToDateTime(oValue[0].Trim());
                                                oMaxValue = Convert.ToDateTime(oValue[1].Trim());

                                                if (oDateValue < oMaxValue && oDateValue > oMinValue)
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                double oMiniValue;
                                                double oMaxiValue;

                                                oValue = cpitinf.Value.Split(',');

                                                oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region Pasa / No pasa

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.PasaNoPasa:

                                        switch (cpitinf.ValueType.ItemId)
                                        {
                                            #region Tipo valor: archivo

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.File:

                                                bool oRelatedFile = !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue) ? true : false;

                                                if (oRelatedFile)
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: texto

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                if (!string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue))
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: booleano

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Boolean:

                                                oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value);

                                                oBooleanValue = !string.IsNullOrEmpty(oTextValue) && (oTextValue == "1" || oTextValue == "True" || oTextValue == "true") ? true : false;

                                                if (oBooleanValue)
                                                {
                                                    CommercialScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = CommercialScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion
                                }
                            }

                            return true;
                        });

                        RuleScore = 0;

                        return true;
                    });
                }

                ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("End Commercial module process::: Provider public id::: " + CompanyPublicId);
            }
            catch (Exception err)
            {
                ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Fatal error:: Commercial Module :: " + err.Message + " - " + err.StackTrace);
            }

            //Get new score
            oReturn.ItemScore = oTotalModuleScore;

            return oReturn;
        }
    }
}
