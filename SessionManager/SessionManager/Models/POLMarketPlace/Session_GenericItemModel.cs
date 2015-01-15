using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.POLMarketPlace
{
    public class Session_GenericItemModel
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public List<Session_GenericItemInfoModel> ItemInfo { get; set; }

        public Session_GenericItemModel ParentItem { get; set; }

    }
}
