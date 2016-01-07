using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.General
{
    public static class SessionModel
    {
        public static SessionManager.Models.Auth.User CurrentLoginUser { get { return SessionManager.SessionController.Auth_UserLogin; } }

        public static bool UserIsLoggedIn { get { return (CurrentLoginUser != null); } }

        public static bool UserIsAutorized
        {
            get
            {
                return CurrentLoginUser.
                            RelatedApplicationRole.
                            Any(x => x.Application == SessionManager.Models.Auth.enumApplication.DocumentManagement);
            }
        }

        public static bool ShowChanges { get; set; }
    }
}
