using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.Auth
{
    public class UserProvider
    {
        public string ProviderId { get; set; }

        public enumProvider Provider { get; set; }

        public string ProviderUrl { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
