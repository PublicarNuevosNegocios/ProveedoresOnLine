using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.SurveyBatch.Models
{
    public class Constants
    {
        #region AppSettings

        public const string C_AppSettings_LogFile = "LogFile";

        #endregion

        #region Internal Settings

        public const string C_SettingsModuleName = "SurveyBatch";

        public const string C_Settings_Company_DefaultLogoUrl = "Company_DefaultLogoUrl";

        public const string C_Settings_Provider_ProviderUrl = "Provider_ProviderUrl";

        public const string C_Settings_Survey_SurveyUrl = "Survey_SurveyUrl";

        #endregion

        #region Message Agent Constants

        public const string C_POL_SurveyWriteBackNotification_Mail_Agent = "POL_SurveyWriteBackNotification_Mail";
        public const string C_POL_SurveyReminder_Mail_Agent = "POL_SurveyReminder_Mail";

        #endregion
    }
}
