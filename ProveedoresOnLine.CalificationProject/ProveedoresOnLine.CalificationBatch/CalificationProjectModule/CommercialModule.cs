﻿using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;
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

            if (oRelatedCalificationProjectItemModel != null &&
                oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel != null &&
                oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Count > 0)
            {
                oReturn.CalificatioProjectItemInfoModel = oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel;
            }

            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCommercialProviderInfo;

            #region Variables

            int oTotalModuleScore = 0;
            int CommercialScore = 0;
            int RuleScore = 0;
            int oIntValue = 0;
            bool oBooleanValue = true;
            double oPercentValue = 0;
            DateTime oDateValue = new DateTime();

            #endregion

            try
            {
                if (oReturn.CalificatioProjectItemInfoModel != null &&
                    oReturn.CalificatioProjectItemInfoModel.Count > 0)
                {
                    //Get last calification
                    oReturn.CalificatioProjectItemInfoModel.Where(cpitinf => cpitinf.CalificationProjectConfigItemInfoModel.LastModify <= cpitinf.LastModify).All(cpitinf =>
                    {
                        oTotalModuleScore += cpitinf.ItemInfoScore;
                        return true;
                    });

                    oReturn.CalificatioProjectItemInfoModel.Where(cpitinf => cpitinf.CalificationProjectConfigItemInfoModel.LastModify > cpitinf.LastModify).All(cpitinf =>
                    {
                        oCommercialProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CommercialModuleInfo(CompanyPublicId, cpitinf.CalificationProjectConfigItemInfoModel.Question);

                        oCommercialProviderInfo.Where(pinf => pinf != null).All(pinf =>
                        {
                            if (RuleScore <= 0)
                            {
                                switch (cpitinf.CalificationProjectConfigItemInfoModel.Rule.ItemId)
                                {
                                    #region Positivo

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                        if (oIntValue > 0)
                                        {
                                            CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                            RuleScore++;

                                            oTotalModuleScore += CommercialScore;
                                        }
                                        else
                                        {
                                            CommercialScore = 0;
                                        }

                                        cpitinf.ItemInfoScore = CommercialScore;

                                        break;

                                    #endregion

                                    #region Negativo

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                        if (oIntValue < 0)
                                        {
                                            CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                            RuleScore++;

                                            oTotalModuleScore += CommercialScore;
                                        }
                                        else
                                        {
                                            CommercialScore = 0;
                                        }

                                        cpitinf.ItemInfoScore = CommercialScore;

                                        break;

                                    #endregion

                                    #region MayorQue

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:

                                        switch (cpitinf.CalificationProjectConfigItemInfoModel.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue > Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oDateValue > Convert.ToDateTime(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue > Convert.ToDouble(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region MenorQue

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorQue:

                                        switch (cpitinf.CalificationProjectConfigItemInfoModel.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue < Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oDateValue < Convert.ToDateTime(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue < Convert.ToDouble(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region MayorOIgual

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorOIgual:

                                        switch (cpitinf.CalificationProjectConfigItemInfoModel.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue >= Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oDateValue >= Convert.ToDateTime(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue >= Convert.ToDouble(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region MenorOIgual

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorOIgual:

                                        switch (cpitinf.CalificationProjectConfigItemInfoModel.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue <= Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oDateValue <= Convert.ToDateTime(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue <= Convert.ToDouble(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region IgualQue

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.IgualQue:

                                        switch (cpitinf.CalificationProjectConfigItemInfoModel.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue == Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oDateValue == Convert.ToDateTime(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oPercentValue == Convert.ToDouble(cpitinf.CalificationProjectConfigItemInfoModel.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region Entre

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Entre:

                                        switch (cpitinf.CalificationProjectConfigItemInfoModel.ValueType.ItemId)
                                        {
                                            #region Tipo valor: numérico

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                int minValue = 0;
                                                int maxValue = 0;

                                                string[] oValue = cpitinf.CalificationProjectConfigItemInfoModel.Value.Split(',');

                                                minValue = Convert.ToInt32(oValue[0]);
                                                maxValue = Convert.ToInt32(oValue[1]);

                                                oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                                if (oIntValue < maxValue && oIntValue > minValue)
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value);

                                                DateTime oMinValue;
                                                DateTime oMaxValue;

                                                oValue = cpitinf.CalificationProjectConfigItemInfoModel.Value.Split(',');

                                                oMinValue = Convert.ToDateTime(oValue[0].Trim());
                                                oMaxValue = Convert.ToDateTime(oValue[1].Trim());

                                                if (oDateValue < oMaxValue && oDateValue > oMinValue)
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value);

                                                double oMiniValue;
                                                double oMaxiValue;

                                                oValue = cpitinf.CalificationProjectConfigItemInfoModel.Value.Split(',');

                                                oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                cpitinf.ItemInfoScore = CommercialScore;

                                                break;

                                            #endregion
                                        }

                                        break;

                                    #endregion

                                    #region Pasa / No pasa

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.PasaNoPasa:

                                        //Validación por archivo
                                        if (cpitinf.CalificationProjectConfigItemInfoModel.ValueType.ItemId == (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.File)
                                        {
                                            bool oRelatedFile = !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue) ? true : false;

                                            if (oRelatedFile)
                                            {
                                                CommercialScore = Convert.ToInt32(cpitinf.CalificationProjectConfigItemInfoModel.Score);

                                                RuleScore++;

                                                oTotalModuleScore += CommercialScore;
                                            }
                                            else
                                            {
                                                CommercialScore = 0;
                                            }
                                        }

                                        cpitinf.ItemInfoScore = CommercialScore;

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
                    oReturn.CalificatioProjectItemInfoModel = new List<CalificationProjectItemInfoBatchModel>();

                    oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.All(cpitinf =>
                    {
                        oCommercialProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CommercialModuleInfo(CompanyPublicId, cpitinf.Question);

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
                                            CommercialScore = Convert.ToInt32(cpitinf.Score);

                                            RuleScore++;

                                            oTotalModuleScore += CommercialScore;
                                        }
                                        else
                                        {
                                            CommercialScore = 0;
                                        }

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

                                        break;

                                    #endregion

                                    #region Negativo

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(pinf.ItemInfo.FirstOrDefault().Value);

                                        if (oIntValue < 0)
                                        {
                                            CommercialScore = Convert.ToInt32(cpitinf.Score);

                                            RuleScore++;

                                            oTotalModuleScore += CommercialScore;
                                        }
                                        else
                                        {
                                            CommercialScore = 0;
                                        }

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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue > Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue > Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue < Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue < Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue >= Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue >= Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue <= Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue <= Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: fecha

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Date:

                                                oDateValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeDate(pinf.ItemInfo.FirstOrDefault().Value.ToString());

                                                if (oDateValue == Convert.ToDateTime(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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

                                                break;

                                            #endregion

                                            #region Tipo valor: porcentaje

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(pinf.ItemInfo.FirstOrDefault().Value.Trim());

                                                if (oPercentValue == Convert.ToDouble(cpitinf.Value))
                                                {
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
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
                                                    CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += CommercialScore;
                                                }
                                                else
                                                {
                                                    CommercialScore = 0;
                                                }

                                                break;

                                            #endregion
                                        }

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

                                        break;

                                    #endregion

                                    #region Pasa / No pasa

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.PasaNoPasa:

                                        //Validación por archivo
                                        if (cpitinf.ValueType.ItemId == (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.File)
                                        {
                                            bool oRelatedFile = !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().LargeValue) || !string.IsNullOrEmpty(pinf.ItemInfo.FirstOrDefault().Value) ? true : false;

                                            if (oRelatedFile)
                                            {
                                                CommercialScore = Convert.ToInt32(cpitinf.Score);

                                                RuleScore++;

                                                oTotalModuleScore += CommercialScore;
                                            }
                                            else
                                            {
                                                CommercialScore = 0;
                                            }
                                        }

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

                ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Se validaron las reglas Comerciales del proveedor " + CompanyPublicId);
            }
            catch (Exception err)
            {
                ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }

            //Get new score
            oReturn.ItemScore = oTotalModuleScore;

            return oReturn;
        }
    }
}
