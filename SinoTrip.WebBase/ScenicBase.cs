using SinoTrip.Cache;
using SinoTrip.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using SinoTrip.FrameWork.Common;

namespace SinoTrip.WebBase
{
    public class ScenicBase : BasePage
    {

        private static List<ViewScenery> _SecCache;

        public List<ViewScenery> SecCache
        {
            get
            {
                if (_SecCache == null)
                {
                    _SecCache = SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "", string.Empty);
                }
                return _SecCache;
            }
            set {
                _SecCache = value;
            }
        }
        /// <summary>
        /// Unix时间戳格式化成指定格式时间
        /// </summary>
        /// <param name="ts">时间戳</param>
        /// <param name="format">如yyyy-MM-dd</param>
        /// <returns></returns>
        public string UnixFormat(int ts, string format)
        {
            if (ts > 0)
                return ts.UnixIntToDT().ToString(format);
            return "-";
        }
    }
}
