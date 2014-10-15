using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.Auth
{
    public class User
    {
        public string UserPublicId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public List<UserInfo> RelatedUserInfo { get; set; }

        public List<UserProvider> RelatedUserProvider { get; set; }

        public List<ApplicationRole> RelatedApplicationRole { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
