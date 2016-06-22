using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.CalificationProjectModule
{
    public class BalanceModule
    {
        public static CalificationProjectItemBatchModel BalanceRule(string CompanyPublicId, ConfigItemModel oCalificationProjectItemModel, CalificationProjectItemBatchModel oRelatedCalificationProjectItemModel)
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

            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> oBalanceProviderInfo;
            
            #region Variables

            int oTotalModuleScore = 0;
            int BalanceScore = 0;
            int RuleScore = 0;
            int oIntValue = 0;
            decimal oDecimalValue = 0;
            bool oBooleanValue = true;
            double oPercentValue = 0;
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

                    //Get last calification
                    //oReturn.CalificatioProjectItemInfoModel.Where(cpitinf => cpitinf.CalificationProjectConfigItemInfoModel.LastModify <= cpitinf.LastModify).All(cpitinf =>
                    //{
                    //    oTotalModuleScore += cpitinf.ItemInfoScore;
                    //    return true;
                    //});

                    oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.Where(rule => rule.Enable == true).All(rule =>
                    {
                        if (oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Any(mprule => mprule.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId == rule.CalificationProjectConfigItemInfoId))
                        {
                            oRelatedCalificationProjectItemModel.CalificatioProjectItemInfoModel.Where(mprule => mprule.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId == rule.CalificationProjectConfigItemInfoId).All(mprule =>
                            {
                                //add mp rule
                                oBalanceProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.BalanceModuleInfo(CompanyPublicId, rule.Question);

                                oBalanceProviderInfo.All(f =>
                                {
                                    if (f.BalanceSheetInfo != null &&
                                        f.BalanceSheetInfo.Count > 0)
                                    {
                                        f.BalanceSheetInfo.All(bs =>
                                        {
                                            switch (rule.Rule.ItemId)
                                            {
                                                #region Positivo

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                                                    oDecimalValue = bs.Value;

                                                    if (oDecimalValue > 0)
                                                    {
                                                        BalanceScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += BalanceScore;
                                                    }
                                                    else
                                                    {
                                                        BalanceScore = 0;
                                                    }

                                                    mprule.ItemInfoScore = BalanceScore;

                                                    break;

                                                #endregion

                                                #region Negativo

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                                    oDecimalValue = bs.Value;

                                                    if (oDecimalValue < 0)
                                                    {
                                                        BalanceScore = Convert.ToInt32(rule.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += BalanceScore;
                                                    }
                                                    else
                                                    {
                                                        BalanceScore = 0;
                                                    }

                                                    mprule.ItemInfoScore = BalanceScore;

                                                    break;

                                                #endregion

                                                #region MayorQue

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:

                                                    switch (rule.ValueType.ItemId)
                                                    {
                                                        #region Tipo valor: numérico

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue > Convert.ToDecimal(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue > Convert.ToDouble(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue < Convert.ToDecimal(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue < Convert.ToDouble(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue >= Convert.ToDecimal(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue >= Convert.ToDouble(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue <= Convert.ToDecimal(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue <= Convert.ToDouble(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue == Convert.ToDecimal(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue == Convert.ToDouble(rule.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

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

                                                            decimal minValue = 0;
                                                            decimal maxValue = 0;

                                                            string[] oValue = rule.Value.Split(',');

                                                            minValue = Convert.ToDecimal(oValue[0]);
                                                            maxValue = Convert.ToDecimal(oValue[1]);

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue < maxValue && oDecimalValue > minValue)
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            double oMiniValue;
                                                            double oMaxiValue;

                                                            oValue = rule.Value.Split(',');

                                                            oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                            oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                            if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                            {
                                                                BalanceScore = Convert.ToInt32(rule.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            mprule.ItemInfoScore = BalanceScore;

                                                            break;

                                                        #endregion
                                                    }

                                                    break;

                                                #endregion
                                            }

                                            return true;
                                        });
                                    }

                                    return true;
                                });

                                RuleScore = 0;

                                return true;
                            });
                        }
                        else
                        {
                            oBalanceProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.BalanceModuleInfo(CompanyPublicId, rule.Question);

                            oBalanceProviderInfo.Where(f => f != null).All(f =>
                            {
                                f.BalanceSheetInfo.All(bs =>
                                {
                                    if (RuleScore <= 0)
                                    {
                                        switch (rule.Rule.ItemId)
                                        {
                                            #region Positivo

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                                                oDecimalValue = bs.Value;

                                                if (oDecimalValue >= 0)
                                                {
                                                    BalanceScore = Convert.ToInt32(rule.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += BalanceScore;
                                                }
                                                else
                                                {
                                                    BalanceScore = 0;
                                                }

                                                oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                {
                                                    CalificationProjectItemInfoId = 0,
                                                    CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                    {
                                                        CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                    },
                                                    ItemInfoScore = BalanceScore,
                                                    Enable = true,
                                                });

                                                break;

                                            #endregion

                                            #region Negativo

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                                oDecimalValue = bs.Value;

                                                if (oDecimalValue < 0)
                                                {
                                                    BalanceScore = Convert.ToInt32(rule.Score);

                                                    RuleScore++;

                                                    oTotalModuleScore += BalanceScore;
                                                }
                                                else
                                                {
                                                    BalanceScore = 0;
                                                }

                                                oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                {
                                                    CalificationProjectItemInfoId = 0,
                                                    CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                    {
                                                        CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                    },
                                                    ItemInfoScore = BalanceScore,
                                                    Enable = true,
                                                });

                                                break;

                                            #endregion

                                            #region MayorQue

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:

                                                switch (rule.ValueType.ItemId)
                                                {
                                                    #region Tipo valor: numérico

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:

                                                        oDecimalValue = bs.Value;

                                                        if (oDecimalValue > Convert.ToDecimal(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                        if (oPercentValue > Convert.ToDouble(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

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

                                                        oDecimalValue = bs.Value;

                                                        if (oDecimalValue < Convert.ToDecimal(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                        if (oPercentValue < Convert.ToDouble(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

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

                                                        oDecimalValue = bs.Value;

                                                        if (oDecimalValue >= Convert.ToDecimal(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                        if (oPercentValue >= Convert.ToDouble(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

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

                                                        oDecimalValue = bs.Value;

                                                        if (oDecimalValue <= Convert.ToDecimal(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                        if (oPercentValue <= Convert.ToDouble(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

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

                                                        oDecimalValue = bs.Value;

                                                        if (oDecimalValue == Convert.ToDecimal(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oDecimalValue = bs.Value;

                                                        if (oPercentValue == Convert.ToDouble(rule.Value))
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                        {
                                                            CalificationProjectItemInfoId = 0,
                                                            CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                            {
                                                                CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                            },
                                                            ItemInfoScore = BalanceScore,
                                                            Enable = true,
                                                        });

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

                                                        decimal minValue = 0;
                                                        decimal maxValue = 0;

                                                        string[] oValue = rule.Value.Split(',');

                                                        minValue = Convert.ToDecimal(oValue[0]);
                                                        maxValue = Convert.ToDecimal(oValue[1]);

                                                        oDecimalValue = bs.Value;

                                                        if (oDecimalValue < maxValue && oDecimalValue > minValue)
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        break;

                                                    #endregion

                                                    #region Tipo valor: porcentaje

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                        oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                        double oMiniValue;
                                                        double oMaxiValue;

                                                        oValue = rule.Value.Split(',');

                                                        oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                        oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                        if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                        {
                                                            BalanceScore = Convert.ToInt32(rule.Score);

                                                            RuleScore++;

                                                            oTotalModuleScore += BalanceScore;
                                                        }
                                                        else
                                                        {
                                                            BalanceScore = 0;
                                                        }

                                                        break;

                                                    #endregion
                                                }

                                                oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                {
                                                    CalificationProjectItemInfoId = 0,
                                                    CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                    {
                                                        CalificationProjectConfigItemInfoId = rule.CalificationProjectConfigItemInfoId,
                                                    },
                                                    ItemInfoScore = BalanceScore,
                                                    Enable = true,
                                                });

                                                break;

                                            #endregion
                                        }
                                    }

                                    return true;
                                });

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

                    oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.All(cpitinf =>
                    {
                        oBalanceProviderInfo = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.BalanceModuleInfo(CompanyPublicId, cpitinf.Question);

                        if (oBalanceProviderInfo != null &&
                            oBalanceProviderInfo.Count > 0)
                        {
                            oBalanceProviderInfo.All(f =>
                            {
                                if (f.BalanceSheetInfo != null &&
                                    f.BalanceSheetInfo.Count > 0)
                                {
                                    f.BalanceSheetInfo.All(bs =>
                                    {
                                        if (RuleScore <= 0)
                                        {
                                            switch (cpitinf.Rule.ItemId)
                                            {
                                                #region Positivo

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                                                    oDecimalValue = bs.Value;

                                                    if (oDecimalValue >= 0)
                                                    {
                                                        BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += BalanceScore;
                                                    }
                                                    else
                                                    {
                                                        BalanceScore = 0;
                                                    }

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = BalanceScore,
                                                        Enable = true,
                                                    });

                                                    break;

                                                #endregion

                                                #region Negativo

                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                                                    oDecimalValue = bs.Value;

                                                    if (oDecimalValue < 0)
                                                    {
                                                        BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                        RuleScore++;

                                                        oTotalModuleScore += BalanceScore;
                                                    }
                                                    else
                                                    {
                                                        BalanceScore = 0;
                                                    }

                                                    oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                    {
                                                        CalificationProjectItemInfoId = 0,
                                                        CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                        {
                                                            CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                        },
                                                        ItemInfoScore = BalanceScore,
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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue > Convert.ToDecimal(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
                                                                Enable = true,
                                                            });

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue > Convert.ToDouble(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue < Convert.ToDecimal(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
                                                                Enable = true,
                                                            });

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue < Convert.ToDouble(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue >= Convert.ToDecimal(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
                                                                Enable = true,
                                                            });

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue >= Convert.ToDouble(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue <= Convert.ToDecimal(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
                                                                Enable = true,
                                                            });

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            if (oPercentValue <= Convert.ToDouble(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
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

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue == Convert.ToDecimal(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
                                                                Enable = true,
                                                            });

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oDecimalValue = bs.Value;

                                                            if (oPercentValue == Convert.ToDouble(cpitinf.Value))
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            oReturn.CalificatioProjectItemInfoModel.Add(new CalificationProjectItemInfoBatchModel()
                                                            {
                                                                CalificationProjectItemInfoId = 0,
                                                                CalificationProjectConfigItemInfoModel = new ConfigItemInfoModel()
                                                                {
                                                                    CalificationProjectConfigItemInfoId = cpitinf.CalificationProjectConfigItemInfoId,
                                                                },
                                                                ItemInfoScore = BalanceScore,
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

                                                            decimal minValue = 0;
                                                            decimal maxValue = 0;

                                                            string[] oValue = cpitinf.Value.Split(',');

                                                            minValue = Convert.ToDecimal(oValue[0]);
                                                            maxValue = Convert.ToDecimal(oValue[1]);

                                                            oDecimalValue = bs.Value;

                                                            if (oDecimalValue < maxValue && oDecimalValue > minValue)
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
                                                            }

                                                            break;

                                                        #endregion

                                                        #region Tipo valor: porcentaje

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:

                                                            oPercentValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(bs.Value.ToString());

                                                            double oMiniValue;
                                                            double oMaxiValue;

                                                            oValue = cpitinf.Value.Split(',');

                                                            oMiniValue = Convert.ToDouble(oValue[0].Trim());
                                                            oMaxiValue = Convert.ToDouble(oValue[1].Trim());

                                                            if (oPercentValue < oMaxiValue && oPercentValue > oMiniValue)
                                                            {
                                                                BalanceScore = Convert.ToInt32(cpitinf.Score);

                                                                RuleScore++;

                                                                oTotalModuleScore += BalanceScore;
                                                            }
                                                            else
                                                            {
                                                                BalanceScore = 0;
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
                                                        ItemInfoScore = BalanceScore,
                                                        Enable = true,
                                                    });

                                                    break;

                                                #endregion
                                            }
                                        }

                                        return true;
                                    });
                                }

                                return true;
                            });
                        }

                        RuleScore = 0;

                        return true;
                    });
                }

                ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Se validaron las reglas del balance financiero del proveedor " + CompanyPublicId);
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
