using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Interfaces
{
    internal interface ICalificationProjectData
    {
        #region ProjectConfig
            #region ProjectConfig

        int CalificationProjectConfigUpsert(int CalificationProjectConfigId, string CompanyPublicId, string CalificationProjectConfigName, bool Enable);
        List<Models.CalificationProject.CalificationProjectConfigModel> CalificationProjectConfig_GetByCompanyId(string Company, bool Enable);
        List<Models.CalificationProject.CalificationProjectConfigModel> CalificationProjectConfig_GetAll();

        #endregion

            #region ConfigItem

        int CalificationProjectConfigItemUpsert(int CalificationProjectConfigId, int CalificationProjectConfigItemId, string CalificationProjectConfigItemName, int CalificationProjectConfigItemType, bool Enable);

        List<ConfigItemModel> CalificationProjectConfigItem_GetByCalificationProjectConfigId(int CalificationProjectConfigId, bool Enable);

        #endregion

            #region ConfigItemInfo

        int CalificationProjectConfigItemInfoUpsert(int CalificationProjectConfigItemId, int CalificationProjectConfigItemInfoId, int Question, int Rule, int ValueType, string Value, string Score, bool Enable);

        List<ConfigItemInfoModel> CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(int CalificationProjectConfigItemId, bool Enable);

        #endregion

            #region ConfigValidate

        int CalificationProjectConfigValidateUpsert(int CalificationProjectConfigValidateId, int CalificationProjectConfigId, int Operator, string Value, string Result, bool Enable);
        List<ConfigValidateModel> CalificationProjectConfigValidate_GetByProjectConfigId(int CalificationProjectConfigId, bool Enable);

        #endregion

        #endregion

        
    }
}
