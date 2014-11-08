using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderFileLogModel
    {
        public string User
        {
            get
            {
                string oReturn = string.Empty;

                if (RelatedLog != null)
                {
                    oReturn = RelatedLog.User;
                }

                return oReturn;
            }
        }

        public string CreateDate
        {
            get
            {
                string oReturn = string.Empty;

                if (RelatedLog != null)
                {
                    oReturn = RelatedLog.CreateDate.ToString("yyyy-MM-dd HH:mm");
                }

                return oReturn;
            }
        }

        public string Url
        {
            get
            {
                string oReturn = string.Empty;

                if (RelatedLog != null && RelatedLog.RelatedLogInfo != null && RelatedLog.RelatedLogInfo.Count > 0)
                {
                    oReturn = RelatedLog.RelatedLogInfo.
                        Where(x => x.LogInfoType == "LargeValue").
                        Select(x => x.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();
                }

                return oReturn;
            }
        }

        public LogManager.Models.LogModel RelatedLog { get; private set; }

        public ProviderFileLogModel(LogManager.Models.LogModel oLogModel)
        {
            RelatedLog = oLogModel;
        }
    }
}
