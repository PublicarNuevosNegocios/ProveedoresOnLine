namespace MarketPlace.Models.General
{
    public static class Constants
    {
        #region Internal Settings

        public const string C_SettingsModuleName = "MarketPlace";

        #region Area Config

        public const string C_Settings_Area_HostConfig = "Area_HostConfig";

        #endregion

        #region LoginModule

        public const string C_Settings_Login_InternalLogin = "Login_InternalLogin";

        public const string C_Settings_Login_UserNotAutorized = "Login_UserNotAutorized";

        public const string C_Settings_Login_DefaultUserUrl = "Login_DefaultUserUrl";

        #endregion

        #region Generic params

        public const string C_Settings_Grid_RowCountDefault = "Grid_RowCountDefault";

        public const string C_Settings_DateFormat_Kendo = "DateFormat_Kendo";

        public const string C_Settings_DateFormat_KendoToServer = "DateFormat_KendoToServer";

        public const string C_Settings_DateFormat_Server = "DateFormat_Server";

        public const string C_Settings_CurrencyExchange_USD = "CurrencyExchange_USD";
        public const string C_Settings_CurrencyExchange_COP = "CurrencyExchange_COP";

        public const string C_Settings_ProviderStatus_Certified = "ProviderStatus_Certified";

        public const string C_Settings_ProviderStatus_Certified_Logo = "ProviderStatus_Certified_Logo";

        public const string C_Settings_Analytics = "{AreaName}_Analytics";

        #endregion

        #region File

        public const string C_Settings_File_TempDirectory = "File_TempDirectory";

        public const string C_Settings_File_RemoteDirectory = "File_RemoteDirectory";

        #endregion

        #region Company

        public const string C_Settings_Company_DefaultLogoUrl = "Company_DefaultLogoUrl";
        public const string C_Settings_Company_Customer_DefaultLogoUrl = "Company_Customer_DefaultLogoUrl";

        #endregion

        #region Project

        public const string C_Settings_Project_Approval_MailAgent = "Project_Approval_MailAgent";

        public const string C_Settings_Project_ApproveArea_MailAgent = "Project_ApproveArea_MailAgent";

        public const string C_Settings_Project_RejectArea_MailAgent = "Project_RejectArea_MailAgent";

        #endregion

        #endregion

        #region Routes

        public const string C_Routes_Default = "Default";

        #endregion

        #region ViewData

        public const string C_ViewData_UserNotAutorizedText = "UserNotAutorizedText";

        #endregion

        #region Program

        public const string C_Program_Compare_ColumnItem = "width:{Width},field:{Field},headerTemplate:{HeaderTemplate},template:{Template},locked:{Locked},";

        public const string C_Program_Compare_Value_EvaluationArea = "Name:{Name},Type:{Type},";

        public const string C_Program_Compare_Value_Item = "Value_{i}:{Value},Unit_{i}:{After:{After},Before:{Before}},";

        #endregion
    }
}
