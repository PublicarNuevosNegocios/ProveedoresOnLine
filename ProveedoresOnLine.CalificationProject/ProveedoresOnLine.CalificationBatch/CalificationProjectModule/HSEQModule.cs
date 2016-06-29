using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;

namespace ProveedoresOnLine.CalificationBatch.CalificationProjectModule
{
    public class HSEQModule
    {
        public static CalificationProjectItemBatchModel HSEQRule(string CompanyPublicId, ConfigItemModel oCalificationProjectItemModel, CalificationProjectItemBatchModel oRelatedCalificationProjectItemModel)
        {
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
            
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oHSEQProviderInfo;

            #region Variables

            int oTotalModuleScore = 0;
            int HSEQScore = 0;
            int RuleScore = 0;
            int oIntValue = 0;
            bool oBooleanValue = false;
            double oPercentValue = 0;
            string oTextValue = "";
            DateTime oDateValue = new DateTime();

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
                        if (oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Any(mprule => mprule.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId == rule.CalificationProjectConfigItemInfoId))
                        {
                            oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Where(mprule => mprule.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId == rule.CalificationProjectConfigItemInfoId).All(mprule =>
                            {
                                //add mp rule
                                oHSEQProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CertificationModuleInfo(CompanyPublicId, rule.Question.ItemId);

                                oHSEQProviderInfo.Where(pinf => pinf != null).All(pinf =>
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
                                                    HSEQScore = Convert.ToInt32(rule.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    mprule.ItemInfoScore = HSEQScore;
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Negativo

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue < 0)
                                                {
                                                    HSEQScore = Convert.ToInt32(rule.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    mprule.ItemInfoScore = HSEQScore;
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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

                                                        if (oIntValue > Convert.ToInt32(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue > Convert.ToDateTime(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue > Convert.ToDouble(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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

                                                        if (oIntValue < Convert.ToInt32(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue < Convert.ToDateTime(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue < Convert.ToDouble(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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

                                                        if (oIntValue >= Convert.ToInt32(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue >= Convert.ToDateTime(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue >= Convert.ToDouble(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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

                                                        if (oIntValue <= Convert.ToInt32(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue <= Convert.ToDateTime(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue <= Convert.ToDouble(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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

                                                        if (oIntValue == Convert.ToInt32(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: fecha

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                        oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oDateValue == Convert.ToDateTime(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oPercentValue == Convert.ToDouble(rule.Value))
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: texto

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                        oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                        if (oTextValue == rule.Value)
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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

                                                        minValue = Convert.ToInt32(oValue[0]);
                                                        maxValue = Convert.ToInt32(oValue[1]);

                                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                        if (oIntValue < maxValue && oIntValue > minValue)
                                                        {
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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
                                                            HSEQScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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
                                                            HSEQScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: texto

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                        if (!string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value.Trim()) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue.Trim()))
                                                        {
                                                            HSEQScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: booleano

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Boolean:

                                                        oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                        oBooleanValue = !string.IsNullOrEmpty(oTextValue) && (oTextValue == "1" || oTextValue == "True" || oTextValue == "true") ? true : false;

                                                        if (oBooleanValue)
                                                        {
                                                            HSEQScore = int.Parse(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += HSEQScore;

                                                            mprule.ItemInfoScore = HSEQScore;
                                                        }
                                                        else
                                                        {
                                                            HSEQScore = 0;
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
                            oHSEQProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CertificationModuleInfo(CompanyPublicId, rule.Question.ItemId);

                            oHSEQProviderInfo.Where(pinf => pinf != null).All(pinf =>
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
                                                HSEQScore = Convert.ToInt32(rule.Score);

                                                RuleScore++;

                                                oTotalModuleScore += HSEQScore;

                                                oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                {
                                                    CalificationProjectItemInfoId = 0,
                                                    CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                    {
                                                        CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                    },
                                                    ItemInfoScore = HSEQScore,
                                                    Enable = true,
                                                });
                                            }
                                            else
                                            {
                                                HSEQScore = 0;
                                            }

                                            break;

                                        #endregion

                                        #region Negativo

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                            oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                            if (oIntValue < 0)
                                            {
                                                HSEQScore = Convert.ToInt32(rule.Score);

                                                RuleScore++;

                                                oTotalModuleScore += HSEQScore;

                                                oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                {
                                                    CalificationProjectItemInfoId = 0,
                                                    CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                    {
                                                        CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                    },
                                                    ItemInfoScore = HSEQScore,
                                                    Enable = true,
                                                });
                                            }
                                            else
                                            {
                                                HSEQScore = 0;
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

                                                    if (oIntValue > Convert.ToInt32(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue > Convert.ToDateTime(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    if (oPercentValue > Convert.ToDouble(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
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

                                                    if (oIntValue < Convert.ToInt32(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue < Convert.ToDateTime(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    if (oPercentValue < Convert.ToDouble(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
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

                                                    if (oIntValue >= Convert.ToInt32(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue >= Convert.ToDateTime(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    if (oPercentValue >= Convert.ToDouble(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
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

                                                    if (oIntValue <= Convert.ToInt32(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue <= Convert.ToDateTime(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    if (oPercentValue <= Convert.ToDouble(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
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

                                                    if (oIntValue == Convert.ToInt32(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                    if (oDateValue == Convert.ToDateTime(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    if (oPercentValue == Convert.ToDouble(rule.Value))
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: texto

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                    oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    if (oTextValue == rule.Value)
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);
                                                        RuleScore++;
                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
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

                                                    minValue = Convert.ToInt32(oValue[0]);
                                                    maxValue = Convert.ToInt32(oValue[1]);

                                                    oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                    if (oIntValue < maxValue && oIntValue > minValue)
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: fecha

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                    oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    DateTime oMinValue;
                                                    DateTime oMaxValue;

                                                    oValue = rule.Value.Split(',');

                                                    oMinValue = Convert.ToDateTime(oValue[0].Trim());
                                                    oMaxValue = Convert.ToDateTime(oValue[1].Trim());

                                                    if (oDateValue < oMaxValue && oDateValue > oMinValue)
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: porcentaje

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                    oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    double oMiniValue;
                                                    double oMaxiValue;

                                                    oValue = rule.Value.Split(',');

                                                    oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                    oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                    if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                    {
                                                        HSEQScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
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
                                                        HSEQScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: texto

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                    if (!string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value.Trim()) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue.Trim()))
                                                    {
                                                        HSEQScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
                                                    }

                                                    break;

                                                #endregion

                                                #region Tipo valor: booleano

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Boolean:

                                                    oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                    oBooleanValue = !string.IsNullOrEmpty(oTextValue) && (oTextValue == "1" || oTextValue == "True" || oTextValue == "true") ? true : false;

                                                    if (oBooleanValue)
                                                    {
                                                        HSEQScore = int.Parse(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += HSEQScore;

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = HSEQScore,
                                                            Enable = true,
                                                        });
                                                    }
                                                    else
                                                    {
                                                        HSEQScore = 0;
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
                        oHSEQProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CertificationModuleInfo(CompanyPublicId, cpitinf.Question.ItemId);

                        oHSEQProviderInfo.Where(pinf => pinf != null).All(pinf =>
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
                                            HSEQScore = Convert.ToInt32(cpitinf.Score);

                                            RuleScore++;

                                            oTotalModuleScore += HSEQScore;

                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                            {
                                                CalificationProjectItemInfoId = 0,
                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                {
                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                },
                                                ItemInfoScore = HSEQScore,
                                                Enable = true,
                                            });
                                        }
                                        else
                                        {
                                            HSEQScore = 0;
                                        }

                                        break;

                                    #endregion

                                    #region Negativo

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                        if (oIntValue < 0)
                                        {
                                            HSEQScore = Convert.ToInt32(cpitinf.Score);

                                            RuleScore++;

                                            oTotalModuleScore += HSEQScore;

                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                            {
                                                CalificationProjectItemInfoId = 0,
                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                {
                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                },
                                                ItemInfoScore = HSEQScore,
                                                Enable = true,
                                            });
                                        }
                                        else
                                        {
                                            HSEQScore = 0;
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

                                                if (oIntValue > Convert.ToInt32(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue > Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue > Convert.ToDouble(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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

                                                if (oIntValue < Convert.ToInt32(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue < Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue < Convert.ToDouble(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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

                                                if (oIntValue >= Convert.ToInt32(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue >= Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue >= Convert.ToDouble(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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

                                                if (oIntValue <= Convert.ToInt32(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue <= Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue <= Convert.ToDouble(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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

                                                if (oIntValue == Convert.ToInt32(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue == Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue == Convert.ToDouble(cpitinf.Value))
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: texto

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oTextValue == cpitinf.Value)
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });                                                
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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

                                                minValue = Convert.ToInt32(oValue[0]);
                                                maxValue = Convert.ToInt32(oValue[1]);

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue < maxValue && oIntValue > minValue)
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                DateTime oMinValue;
                                                DateTime oMaxValue;

                                                oValue = cpitinf.Value.Split(',');

                                                oMinValue = Convert.ToDateTime(oValue[0].Trim());
                                                oMaxValue = Convert.ToDateTime(oValue[1].Trim());

                                                if (oDateValue < oMaxValue && oDateValue > oMinValue)
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                double oMiniValue;
                                                double oMaxiValue;

                                                oValue = cpitinf.Value.Split(',');

                                                oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                {
                                                    HSEQScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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
                                                    HSEQScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: texto

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:

                                                if (!string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value.Trim()) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue.Trim()))
                                                {
                                                    HSEQScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
                                                }

                                                break;

                                            #endregion

                                            #region Tipo valor: booleano

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Boolean:

                                                oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                oBooleanValue = !string.IsNullOrEmpty(oTextValue) && (oTextValue == "1" || oTextValue == "True" || oTextValue == "true") ? true : false;

                                                if (oBooleanValue)
                                                {
                                                    HSEQScore = int.Parse(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += HSEQScore;

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = HSEQScore,
                                                        Enable = true,
                                                    });
                                                }
                                                else
                                                {
                                                    HSEQScore = 0;
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

                ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Se validaron las reglas de certificaciones del proveedor " + CompanyPublicId);
            }
            catch (Exception err)
            {
                ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Fatal error:: HSEQ Module :: " + err.Message + " - " + err.StackTrace);
            }

            //Get new score
            oReturn.ItemScore = oTotalModuleScore;

            return oReturn;
        }
    }
}
