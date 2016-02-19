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

        public const string C_Settings_Paginator_PagesLimit = "Paginator_PagesLimit";
        
        public const string C_Settings_Grid_AutoCompleteRow = "MP_AutoCompleteRow";

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

        public const string C_Settings_File_ThirdKnowledgeRemoteDirectory = "File_ThirdKnowledgeRemoteDirectory";        

        #endregion

        #region Company

        public const string C_Settings_Company_DefaultLogoUrl = "Company_DefaultLogoUrl";
        public const string C_Settings_Company_Customer_DefaultLogoUrl = "Company_Customer_DefaultLogoUrl";

        #endregion

        #region Error

        public const string C_Settings_Error_Gear1 = "Company_Error_Gear1";
        public const string C_Settings_Error_Gear2 = "Company_Error_Gear2";
        public const string C_Settings_Error_Gear3 = "Company_Error_Gear3";        

        #endregion

        #region Project

        public const string C_Settings_Project_Approval_MailAgent = "Project_Approval_MailAgent";

        public const string C_Settings_Project_ApproveArea_MailAgent = "Project_ApproveArea_MailAgent";

        public const string C_Settings_Project_RejectArea_MailAgent = "Project_RejectArea_MailAgent";

        #endregion

        #region Manuals

        public const string C_Manual_SurveyManual = "Manual_SurveyManual";

        public const string C_Manual_ProjectManual = "Manual_ProjectManual";

        public const string C_Manual_TkManual = "Manual_TktManual";

        public const string C_Manual_ClientPolipropileno = "Manual_ClientPolipropileno";

        #endregion

        #region Customers

        public const string CC_CompanyPublicId_Publicar = "CompanyPublicId_Publicar";

        public const string CC_CompanyPublicId_ClientePolipropileno = "CompanyPublicId_ClientePolipropileno";

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

        #region Report

        public const string MP_CP_ReportPath = "MP_CP_ReportPath";

        public const string MP_Report_ExcelReportIcon = "Report_ExcelReportIcon";

        public const string MP_Report_PdfReportIcon = "Report_PdfReportIcon";

        public const string MP_TK_TextImage = "TK_TextImage";

        #endregion

        #region ThirdKnowledge

        public const string MP_CP_UploadTemplate = "MP_CP_UploadTemplate";

        public const string MP_CP_ColPersonType = "MP_CP_ColPersonType";

        public const string MP_CP_ColIdNumber = "MP_CP_ColIdNumber";

        public const string MP_CP_ColIdName = "MP_CP_ColIdName";

        public const string C_Settings_TK_UploadSuccessFileAgent = "TK_UploadSuccessFileAgent";

        public const string TK_Research_Image = "TK_Research_Image";

        #endregion

        #region Notifications

        #region Messages

        public const string N_ThirdKnowledgeUploadMassiveMessage = "N_ThirdKnowledgeUploadMassiveMessage";

        public const string N_ThirdKnowledgeEndMassiveMessage = "N_ThirdKnowledgeEndMassiveMessage";

        public const string N_ThirdKnowledgeSingleMessage = "N_ThirdKnowledgeSingleMessage";

        #endregion

        #region Url redirect

        public const string N_UrlThirdKnowledgeQuery = "N_UrlThirdKnowledgeQuery";

        #endregion

        #region NotificationIcon

        public const string N_NewNotification = "N_NewNotification";

        public const string N_OldNotification = "N_OldNotification";

        #endregion

        #endregion

    }
}
