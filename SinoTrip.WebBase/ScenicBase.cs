using SinoTrip.Cache;
using SinoTrip.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

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
    }
}
