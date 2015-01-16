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
                case "GetPrice":
                    GetPrice(context);
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

        void GetPrice(HttpContext context)
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var idStr = context.Request.QueryString["ids"];
            string[] _ids = idStr.Split(',');
            
            List<int> ids = new List<int>();

            foreach (var id in _ids)
            {
                var i = id.ToInt32(0);
                if(i>0)
                {
                    ids.Add(i);
                }
            }
           
            var model = biz.GetSceneryPrice(2, ids);
            context.Response.Write(model.ToJson());
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