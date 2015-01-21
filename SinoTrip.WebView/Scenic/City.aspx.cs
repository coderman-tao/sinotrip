using SinoTrip.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.FrameWork.Common;

namespace SinoTrip.WebView.Scenic
{
    public partial class City : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var citydata = AreaCache.GetCityCache(0, "", true, "");
            Vt.Put("Citys", citydata.ToJson());
            Vt.Display("Scenic/City.html");
        }
    }
}