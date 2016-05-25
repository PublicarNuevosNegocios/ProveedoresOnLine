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

        #endregion
    }
}
