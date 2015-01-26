using SinoTrip.WebBase;
using SinoTrip.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SinoTrip.WebView.Ajax.User
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var action = Context.Request["method"];
            switch (action)
            {
                case "GetLogin":
                    Context.Response.Write(QU.ToJson());
                    break;
            }
        }
    }
}