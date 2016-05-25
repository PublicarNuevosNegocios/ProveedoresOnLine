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
        int CalificationProjectConfigUpsert(int CalificationProjectConfigId, string Company, string CalificationProjectConfigName, bool Enable);
        #endregion

        #region ConfigItem

        int CalificationProjectConfigItemUpsert(int CalificationProjectConfigId, int CalificationProjectConfigItemId, string CalificationProjectConfigItemName, int CalificationProjectConfigItemType, bool Enable);

        List<ConfigItemModel> CalificationProjectConfigItem_GetByCalificationProjectConfigId(int CalificationProjectConfigId, bool Enable);

        #endregion

        #region ConfigItemInfo

        int CalificationProjectConfigItemInfoUpsert(int CalificationProjectConfigItemId, int CalificationProjectConfigItemInfoId, int Question, int Rule, int ValueType, string Value, string Score, bool Enable);

        List<ConfigItemInfoModel> CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId(int CalificationProjectConfigItemId, bool Enable);

        #endregion
    }
}
