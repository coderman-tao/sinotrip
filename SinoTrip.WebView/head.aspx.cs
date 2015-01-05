using SinoTrip.FrameWork.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SinoTrip.WebView
{
    public partial class head : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            VelocityHelper vt = new VelocityHelper();
            vt.Init(TemplatePath);//模板路径 
            //vt.Put("user", QU);
            vt.Display("head.html");
            //if (Session[SessionKeys.Uid].ToInt32(0) > 0)
            //{

            //}
            //var html = "@";
        }
    }
}