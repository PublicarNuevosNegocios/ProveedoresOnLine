using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public class AreaModel
    {
        /// <summary>
        /// Current Area by request url
        /// </summary>
        public static enumSiteArea CurrentArea
        {
            get
            {
                if (oCurrentArea == null)
                {
                    oCurrentArea = GetAreaByUrl(System.Web.HttpContext.Current.Request.Url.ToString());
                }
                return oCurrentArea.Value;
            }
        }
        private static enumSiteArea? oCurrentArea;

        /// <summary>
        /// Current Area Name by request url
        /// </summary>
        public static string CurrentAreaName { get { return CurrentArea.ToString(); } }

        /// <summary>
        /// domain,area host config
        /// </summary>
        public static Dictionary<string, string> AreaHostConfig
        {
            get
            {
                if (oAreaHostConfig == null)
                {
                    oAreaHostConfig = new Dictionary<string, string>();

                    MarketPlace.Models.General.InternalSettings.Instance
                        [MarketPlace.Models.General.Constants.C_Settings_Area_HostConfig].
                            Value.
                            Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                            All(x =>
                            {
                                string[] oParams = x.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                if (oParams.Length >= 2 &&
                                    !oAreaHostConfig.ContainsKey(oParams[0].ToLower()))
                                {
                                    oAreaHostConfig.Add(oParams[0].ToLower(), oParams[1]);
                                }
                                return true;
                            });
                }

                return oAreaHostConfig;
            }
        }
        private static Dictionary<string, string> oAreaHostConfig;

        public static enumSiteArea? GetAreaByUrl(string UrlToEval)
        {
            enumSiteArea? oReturn = AreaHostConfig.
                Where(x => UrlToEval.Replace(" ", "").ToLower().IndexOf(x.Key) >= 0).
                Select(x => (enumSiteArea?)Enum.Parse(typeof(enumSiteArea), x.Value)).
                DefaultIfEmpty(null).
                FirstOrDefault();

            return oReturn;
        }
    }
}
