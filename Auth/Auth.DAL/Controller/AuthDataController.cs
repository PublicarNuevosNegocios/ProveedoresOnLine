using Auth.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.Controller
{
    public class AuthDataController : Auth.Interfaces.IAuthData
    {
        #region Singleton instance

        private static Auth.Interfaces.IAuthData oInstance;
        public static Auth.Interfaces.IAuthData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new AuthDataController();
                return oInstance;
            }
        }

        private Auth.Interfaces.IAuthData DataFactory;

        #endregion

        #region constructor

        public AuthDataController()
        {
            AuthDataFactory factory = new AuthDataFactory();
            DataFactory = factory.GetDataInstance();
        }

        #endregion

        #region AdminRoles

        public List<AdminRolesModel> ListUserRoles()
        {
            return DataFactory.ListUserRoles();
        }

        public string CreateUserRolesUpsert(int vAplicationId, int vRoleId, string vUserEmail)
        {
            return DataFactory.CreateUserRolesUpsert(vAplicationId, vRoleId, vUserEmail);
        }

        public void DeleteUserRoles(int AplicationRoleId)
        {
            DataFactory.DeleteUserRoles(AplicationRoleId);
        }

        #endregion

        #region Implemented methods

        public string UserUpsert(string Name, string LastName, string Email, string ProviderId, SessionManager.Models.Auth.enumProvider Provider, string ProviderUrl)
        {
            return DataFactory.UserUpsert(Name, LastName, Email, ProviderId, Provider, ProviderUrl);
        }

        public void UserInfoUpsert(int? UserInfoId, string UserPublicId, SessionManager.Models.Auth.enumUserInfoType InfoTypeId, string Value, string LargeValue)
        {
            DataFactory.UserInfoUpsert(UserInfoId, UserPublicId, InfoTypeId, Value, LargeValue);
        }

        public void ApplicationRoleCreate(SessionManager.Models.Auth.enumApplication Application, SessionManager.Models.Auth.enumRole Role, string UserEmail)
        {
            DataFactory.ApplicationRoleCreate(Application, Role, UserEmail);
        }

        public void ApplicationRoleCreate(int ApplicationRoleId)
        {
            DataFactory.ApplicationRoleCreate(ApplicationRoleId);
        }

        public SessionManager.Models.Auth.User UserGetById(string UserPublicId)
        {
            return DataFactory.UserGetById(UserPublicId);
        }

        #endregion

    }
}
