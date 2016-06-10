using System;
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
                ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.LegalModuleInfo(CompanyPublicId, oCalificationProjectItemModel.CalificationProjectConfigItemType.ItemId);

            oCalificationProjectItemModel.CalificationProjectConfigItemInfoModel.All(rule =>
            {
                switch (rule.ValueType.ItemId)
                {
                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Boolean:
                        bool oBooleanValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeBoolean(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);
                        break;
                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Numeric:
                        int oIntValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeNumeric(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);
                        break;
                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Percent:
                        double oDoubleValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypePercent(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);
                        break;
                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumValueType.Text:
                        string oTextValue = ProveedoresOnLine.CalificationBatch.Util.UtilModule.ValueTypeText(oLegalProviderInfo.ItemInfo.FirstOrDefault().Value);
                        break;
                }

                return true;
            });

            return oReturn;
        }
    }
}
