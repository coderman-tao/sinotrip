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
            var tid = Context.Request["tid"].ToStringEx("");
            var method = Context.Request["method"].ToStringEx("");
            if (sid <= 0 || supply <= 0)
            {
                Context.Response.Redirect("/");
                return;
            }

            if (supply == 2)
            {
                var url = "http://www.ly.com/scenery/sceneryydorder.aspx?sceneryId=" + sid + "&ticketId=" + tid + "&refid=39610880";
                if (string.IsNullOrEmpty(tid))
                {
                    url = "http://www.ly.com/scenery/SceneryTickets.aspx?sceneryid=" + sid + "&refid=39610880";
                }
                Context.Response.Redirect(url);
                //http://www.ly.com/scenery/sceneryydorder.aspx?sceneryId=00000&ticketId=00000&refid=0000
            }
        }
    }
}