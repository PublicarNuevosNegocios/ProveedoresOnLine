using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interfaces.Models
{
    public class AdminRolesModel
    {
        public SessionManager.Models.Auth.ApplicationRole RelatedRole { get; set; }

        public string UserEmail { get; set; }

    }
}
