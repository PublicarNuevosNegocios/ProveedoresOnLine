using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interfaces
{
    public interface IAuthData
    {
        string UserUpsert(string Name, string LastName, string Email, string ProviderId, SessionManager.Models.Auth.enumProvider Provider, string ProviderUrl);

        void UserInfoUpsert(int? UserInfoId, string UserPublicId, SessionManager.Models.Auth.enumUserInfoType InfoTypeId, string Value, string LargeValue);

        void ApplicationRoleCreate(SessionManager.Models.Auth.enumApplication Application, SessionManager.Models.Auth.enumRole Role, string UserEmail);

        void ApplicationRoleCreate(int ApplicationRoleId);

        SessionManager.Models.Auth.User UserGetById(string UserPublicId);
    }
}
