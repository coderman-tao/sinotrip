using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SinoTrip.FrameWork.Common;
using SinoTrip.API.LY.Model;
namespace SinoTrip.WebView.Ajax.Scenery
{
    /// <summary>
    /// GetInfo 的摘要说明
    /// </summary>
    public class GetInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var action = context.Request["method"];
            switch (action)
            {
                case "List":
                    List(context);
                    break;
            }
        }

        void List(HttpContext context)
        {
            var pq = context.FillByQueryString<QueryScenery>();
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var rs = biz.QueryScenery(pq);
            context.Response.Write(rs.ToJson());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}