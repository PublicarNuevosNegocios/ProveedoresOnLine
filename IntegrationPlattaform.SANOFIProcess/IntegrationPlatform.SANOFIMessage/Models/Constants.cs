using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlatform.SANOFIMessage.Models
{
    public class Constants
    {
        #region AppSettings

        public const string C_AppSettings_LogFile = "LogFile";

        #endregion

        #region Internal Settings

        public const string C_SettingsModuleName = "SanofiMessage";

        public const string C_Settings_Company_DefaultLogoUrl = "Company_DefaultLogoUrl";

        public const string C_Settings_Provider_ProviderUrl = "Provider_ProviderUrl";

        public const string C_Settings_SANOFI_FileUrl = "Sanofi_FileUrl";

        public const string C_Settings_SanofiProviderInfo = "SanofiProviderInfo";

        #endregion

        #region Message Agent Constants

        public const string C_POL_SANOFI_Mail_Agent = "POL_SanofiProcessFile_Mail";

        public const string C_POL_SANOFI_FileMessage = "SanofiFileMessage";

        public const string C_POL_SanofiMessage_To = "POL_SanofiMessage_To";

        #endregion
    }
}
