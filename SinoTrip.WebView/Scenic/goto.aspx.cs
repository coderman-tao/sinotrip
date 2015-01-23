using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.FrameWork.Common;

namespace SinoTrip.WebView.Scenic
{
    public partial class _goto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var supply = Context.Request["supply"].ToInt32(0);
            var sid = Context.Request["sid"].ToInt32(0);
            var tid = Context.Request["tid"];
            if (sid <= 0 || string.IsNullOrEmpty(tid))
            {
                Context.Response.Redirect("/");
                return;
            }
            if (supply == 2)
            {
                Context.Response.Redirect("http://www.ly.com/scenery/sceneryydorder.aspx?sceneryId=" + sid + "&ticketId=" + tid + "&refid=39610880");
                //http://www.ly.com/scenery/sceneryydorder.aspx?sceneryId=00000&ticketId=00000&refid=0000
            }
        }
    }
}