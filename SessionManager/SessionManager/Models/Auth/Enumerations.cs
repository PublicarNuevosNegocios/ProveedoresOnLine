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
        Email = 202,
        ProfileImage = 203,
        Gender = 204
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
