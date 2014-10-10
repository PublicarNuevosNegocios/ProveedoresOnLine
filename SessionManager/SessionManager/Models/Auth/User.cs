using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.Auth
{
    public class User
    {
        public int UserId { get; set; }
        public string UserPublicId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public bool? Gender { get; set; }
        public List<UserInfo> ExtraData { get; set; }
        public List<UserProvider> UserLogins { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
