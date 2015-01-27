using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.FrameWork.Common;
using SinoTrip.Core;
using System.IO;
using SinoTrip.API.LY.Biz;

namespace SinoTrip.WebView.Scenic
{
    public partial class Dp : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int id = Context.Request["id"].ToInt32(0);
                string _outSign = Context.Request["outsign"];
                if (id <= 0 && string.IsNullOrEmpty(_outSign))
                    Context.Response.Redirect("/");
                var biz = new SinoTrip.Biz.SceneryBiz();
                var Info = biz.GetItem(id, _outSign, "");

                if (Info != null)
                {
                    //GetSignleDP(Info.ItemId, _outSign.ToInt32(0));
                    id = Info.ItemId;
                    var page = Context.Request["page"].ToInt32(0);
                    var pageSize = 20;
                    int total = 0;
                    List<SinoTrip.Entity.DataBase.Scenery.scenery_comment> rs = biz.GetCommentList(id, page, pageSize, out total);

                    Info.BuyNotie = Info.BuyNotie.Replace("同程", "驾驴");
                    Info.Intro = Info.Intro.Replace("同程", "驾驴");
                    // new ScenicBiz()var rs = new ScenicBiz().GetSceneryDetail(outSign);
                    Vt.Put("Info", Info);
                    Vt.Put("page", page);
                    Vt.Put("total", total);
                    Vt.Put("Comments", rs);

                }
                else
                {

                    //InsertOutScenery(_outSign);
                    //SceneryCache.SetSceneryCache();
                    LoggerCore.Error("库内无景点，请抓取来自同程的外部编号:" + _outSign);
                    //Update();
                }
            }
            catch (Exception ex)
            {

                //Context.Response.Redirect("/");
            }
            finally
            {

                Vt.Display("Scenic/Dp.html");
            }
        }

    }
}