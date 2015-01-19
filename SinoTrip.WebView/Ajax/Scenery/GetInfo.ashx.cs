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
                case "GetComment":
                    GetComment(context);
                    break;
                case "GetYJ":
                    GetYJ(context);
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
                if (i > 0)
                {
                    ids.Add(i);
                }
            }

            var model = biz.GetSceneryPrice(2, ids);
            context.Response.Write(model.ToJson());
        }

        void GetComment(HttpContext context)
        {
            var biz = new SinoTrip.Biz.SceneryBiz();
            var id = context.Request["id"].ToInt32(0);
            if (id <= 0)
                return;
            var page = context.Request["page"].ToInt32(0);
            var pageSize = context.Request["pageSize"].ToInt32(0);
            int total = 0;
            List<SinoTrip.Entity.DataBase.Scenery.scenery_comment> rs = biz.GetCommentList(id, page, pageSize, out total);
            if (rs.Count > 0)
            {
                rs[0].Total = total;

                context.Response.Write(rs.ToJson());
            }
        }

        void GetYJ(HttpContext context)
        {
            string tag = context.Request["tag"];
            if (string.IsNullOrEmpty(tag))
                return;
            context.Response.Write(new SinoTrip.Biz.SceneryBiz().GetPostByTag(tag));
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