using Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Auth.Interfaces.Models;
using SessionManager.Models.Auth;

namespace Auth.DAL.MySqlDao
{
    public class Auth_MySqlDao : IAuthData
    {

        private ADO.Interfaces.IADO DataInstance;

        public Auth_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(Interfaces.Constants.C_AuthConnectionName);
        }

        #region AdminRoles

        public List<AdminRolesModel> ListUserRoles()
        {
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                    CommandText = "A_GetAllUsers",
                    CommandType = System.Data.CommandType.StoredProcedure
                });

            List<AdminRolesModel> oRetorno = null;
            

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetorno = (from au in response.DataTableResult.AsEnumerable()
                            select new AdminRolesModel()
                            {
                                RelatedRole = new SessionManager.Models.Auth.ApplicationRole()
                                {
                                    ApplicationRoleId = au.Field<int>("ApplicationRoleId"),
                                    Application = au.Field<enumApplication>("ApplicationId"),
                                    Role = au.Field<enumRole>("RoleId"),
                                    CreateDate = au.Field<DateTime>("CreateDate"),
                                },
                                UserEmail = au.Field<string>("UserEmail")
                            }).ToList();
            }
            return oRetorno;
        }

        public string CreateUserRolesUpsert(
                                      int AplicationId,
                                      int RoleId,
                                      string UserEmail)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vApplicationId", AplicationId));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleId", RoleId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserEmail", UserEmail));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                    CommandText = "A_ApplicationRole_Create",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });

            if (response.ScalarResult != null)
            {
                return response.ScalarResult.ToString();
            }
            else
            {
                return null;
            }
        }

        public void DeleteUserRoles(int AplicationRoleId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vApplicationRoleId", AplicationRoleId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
                {
                    CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                    CommandText = "A_ApplicationRole_Delete",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Parameters = lstParams
                });
        }

        #endregion

        #region Implemented methods

        public string UserUpsert
                    (string Name,
                    string LastName,
                    string Email,
                    string ProviderId,
                    SessionManager.Models.Auth.enumProvider Provider,
                    string ProviderUrl)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vName", Name));
            lstParams.Add(DataInstance.CreateTypedParameter("vLastName", LastName));

            lstParams.Add(DataInstance.CreateTypedParameter("vEmail", Email));

            lstParams.Add(DataInstance.CreateTypedParameter("vProviderId", ProviderId));
            lstParams.Add(DataInstance.CreateTypedParameter("vProvider", (int)Provider));
            lstParams.Add(DataInstance.CreateTypedParameter("vProviderUrl", ProviderUrl));


            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "UI_User_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            if (response.ScalarResult != null)
                return response.ScalarResult.ToString();
            else
                return null;
        }

        public void UserInfoUpsert
                    (int? UserInfoId,
                    string UserPublicId,
                    SessionManager.Models.Auth.enumUserInfoType InfoTypeId,
                    string Value,
                    string LargeValue)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserInfoId", UserInfoId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserPublicId", UserPublicId));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserInfoType", (int)InfoTypeId));
            lstParams.Add(DataInstance.CreateTypedParameter("vValue", Value));
            lstParams.Add(DataInstance.CreateTypedParameter("vLargeValue", LargeValue));


            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "UI_UserInfo_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void ApplicationRoleCreate
                    (SessionManager.Models.Auth.enumApplication Application,
                    SessionManager.Models.Auth.enumRole Role,
                    string UserEmail)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vApplicationId", (int)Application));
            lstParams.Add(DataInstance.CreateTypedParameter("vRoleId", (int)Role));
            lstParams.Add(DataInstance.CreateTypedParameter("vUserEmail", UserEmail));


            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "A_ApplicationRole_Create",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public void ApplicationRoleCreate(int ApplicationRoleId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vApplicationRoleId", ApplicationRoleId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "A_ApplicationRole_Delete",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });
        }

        public SessionManager.Models.Auth.User UserGetById(string UserPublicId)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vUserPublicId", UserPublicId));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "UI_User_GetById",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams
            });

            SessionManager.Models.Auth.User oRetorno = null;

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                oRetorno = new SessionManager.Models.Auth.User()
                {
                    UserPublicId = response.DataTableResult.Rows[0].Field<string>("UserPublicId"),
                    Name = response.DataTableResult.Rows[0].Field<string>("Name"),
                    LastName = response.DataTableResult.Rows[0].Field<string>("LastName"),
                    Email = response.DataTableResult.Rows[0].Field<string>("Email"),

                    RelatedUserInfo =
                        (from ui in response.DataTableResult.AsEnumerable()
                         where !ui.IsNull("UserInfoId")
                         group ui by
                         new
                         {
                             UserInfoId = ui.Field<int>("UserInfoId"),
                             UserInfoType = (SessionManager.Models.Auth.enumUserInfoType)ui.Field<int>("UserInfoType"),
                             Value = ui.Field<string>("Value"),
                             LargeValue = ui.Field<string>("LargeValue"),
                         } into uig
                         select new SessionManager.Models.Auth.UserInfo()
                         {
                             UserInfoId = uig.Key.UserInfoId,
                             UserInfoType = uig.Key.UserInfoType,
                             Value = uig.Key.Value,
                             LargeValue = uig.Key.LargeValue,
                         }).ToList(),

                    RelatedUserProvider =
                        (from up in response.DataTableResult.AsEnumerable()
                         where !up.IsNull("ProviderId")
                         group up by
                         new
                         {
                             ProviderId = up.Field<string>("ProviderId"),
                             Provider = (SessionManager.Models.Auth.enumProvider)up.Field<int>("Provider"),
                             ProviderUrl = up.Field<string>("ProviderUrl"),
                         } into upg
                         select new SessionManager.Models.Auth.UserProvider()
                         {
                             ProviderId = upg.Key.ProviderId,
                             Provider = upg.Key.Provider,
                             ProviderUrl = upg.Key.ProviderUrl,
                         }).ToList(),

                    RelatedApplicationRole =
                        (from ar in response.DataTableResult.AsEnumerable()
                         where !ar.IsNull("ApplicationRoleId")
                         group ar by
                         new
                         {
                             ApplicationRoleId = ar.Field<int>("ApplicationRoleId"),
                             Application = (SessionManager.Models.Auth.enumApplication)ar.Field<int>("ApplicationId"),
                             Role = (SessionManager.Models.Auth.enumRole)ar.Field<int>("RoleId"),
                             UserEmail = ar.Field<string>("Email"),
                         } into arg
                         select new SessionManager.Models.Auth.ApplicationRole()
                         {
                             ApplicationRoleId = arg.Key.ApplicationRoleId,
                             Application = arg.Key.Application,
                             Role = arg.Key.Role,
                         }).ToList(),
                };

            }
            return oRetorno;
        }

        #endregion

    }
}
