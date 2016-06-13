﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationBatch.CalificationProjectModule
{
    public class LegalModule
    {
        public static List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel> LegalRule(string CompanyPublicId, ProveedoresOnLine.CalificationProject.Models.CalificationProject.ConfigItemModel oCalificationProjectItemModel)
        {
            List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel> oReturn = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>();

            ProveedoresOnLine.Company.Models.Util.GenericItemModel oLegalProviderInfo = 
                ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.LegalModuleInfo(CompanyPublicId, oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.FirstOrDefault().Question);

            oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.All(rule =>
            {
                int LegalScore = 0;
                int oIntValue = 0;

                switch (rule.Rule.ItemId)
                {
                    #region Positivo

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Positivo:

                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        if (oIntValue >= 0)
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }

                        break;

                    #endregion

                    #region Negativo

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Negativo:

                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        if (oIntValue < 0)
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }

                       

                        break;

                    #endregion

                    #region MayorQue

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorQue:

                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        if (oIntValue > Convert.ToInt32(rule.Value))
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }


                        break;

                    #endregion

                    #region MenorQue

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorQue:

                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        if (oIntValue < Convert.ToInt32(rule.Value))
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }

                        

                        break;

                    #endregion

                    #region MayorOIgual

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MayorOIgual:
                        
                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        if (oIntValue >= Convert.ToInt32(rule.Value))
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }

                        

                        break;

                    #endregion

                    #region MenorOIgual

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.MenorOIgual:
                        
                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        if (oIntValue <= Convert.ToInt32(rule.Value))
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }

                        

                        break;

                    #endregion

                    #region IgualQue

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.IgualQue:
                        
                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        if (oIntValue == Convert.ToInt32(rule.Value))
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }

                        

                        break;

                    #endregion

                    #region Entre

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.Entre:
                        
                        oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);

                        int minValue = 0;
                        int maxValue = 0;

                        string[] oValue = rule.Value.Split(',');

                        minValue = Convert.ToInt32(oValue[0]);
                        maxValue = Convert.ToInt32(oValue[1]);

                        if (oIntValue < maxValue && oIntValue > minValue)
                        {
                            LegalScore = Convert.ToInt32(rule.Score);
                        }
                        else
                        {
                            LegalScore = 0;
                        }

                        

                        break;

                    #endregion

                    #region Pasa / No pasa

                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumOperatorType.PasaNoPasa:

                        //Validación por archivo
                        if (rule.ValueType.ItemId == (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.File)
                        {
                            bool oRelatedFile = !string.IsNullOrEmpty(oLegalProviderInfo.ItemInfo.FirstOrDefault().LargeValue) || !string.IsNullOrEmpty(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value) ? true : false;

                            if (oRelatedFile)
                            {
                                LegalScore = Convert.ToInt32(rule.Score);
                            }
                            else
                            {
                                LegalScore = 0;
                            }

                            
                        }

                        break;

                    #endregion
                }

                return true;
            });

            return oReturn;
        }
    }
}
