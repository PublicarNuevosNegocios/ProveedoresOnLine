namespace SessionManager.Models.Auth
{
    public enum enumProvider
    {
        InternalLogin = 101,
        Facebook = 102,
        Google = 103,
        WindowsLive = 104,
    }

    public enum enumUserInfoType
    {
        Birthday = 201,
        ProfileImage = 202,
        Gender = 203
    }

    public enum enumGender
    {
        Male = 301,
        Female = 302
    }

    public enum enumApplication
    {
        DocumentManagement = 401,
        Backoffice = 402,
        Marketplace = 403
    }

    public enum enumRole
    {
        SystemAdministrator = 501,
        Certifier = 502,
        Marketing = 503
    }

}
