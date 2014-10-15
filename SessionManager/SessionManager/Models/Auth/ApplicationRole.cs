using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.Auth
{
    public class ApplicationRole
    {
        public int ApplicationRoleId { get; set; }

        public enumApplication Application { get; set; }

        public enumRole Role { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
