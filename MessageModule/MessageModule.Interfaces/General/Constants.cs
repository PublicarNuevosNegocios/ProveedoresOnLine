namespace MessageModule.Interfaces.General
{
    public class Constants
    {
        public const string C_MS_MessageConnection = "MS_MessageConnection";

        #region AppSettings

        public const string C_AppSettings_LogFile = "LogFile";

        #endregion

        #region Internal Settings

        public const string C_SettingsModuleName = "MessageModule";

        public const string C_Settings_Agent = "Agent";

        public const string C_Settings_AgentConfig = "AgentConfig_{AgentName}";

        #endregion

        #region Agent Constants

        public const string C_Agent_Assemblie = "Assemblie";
        public const string C_Agent_To = "To";

        #region Agent AWS

        public const string C_Agent_AWS_AccessKeyId = "AccessKeyId";
        public const string C_Agent_AWS_SecretAccessKey = "SecretAccessKey";
        public const string C_Agent_AWS_RegionEndpoint = "RegionEndpoint";

        public const string C_Agent_AWS_Subject = "Subject";
        public const string C_Agent_AWS_From = "From";
        public const string C_Agent_AWS_MessageBody = "MessageBody";

        #endregion

        #endregion
    }
}

