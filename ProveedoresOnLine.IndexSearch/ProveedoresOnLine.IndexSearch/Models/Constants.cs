using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.IndexSearch.Models
{
    public class Constants
    {
        public const string C_POL_SearchConnectionName = "POL_SearchConnection";

        #region InternalSettings

        public const string C_SettingsModuleName = "Search";

        public const string C_Settings_ElasticSearchUrl = "ElasticSearch_Url";

        public const string C_Settings_CompanyIndex = "CompanyIndex";

        #endregion

        #region AppSettings
        public const string C_AppSettings_LogFile = "LogFile";
        #endregion
    }
}
