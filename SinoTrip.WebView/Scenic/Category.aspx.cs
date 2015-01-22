using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.FrameWork.Common;
using SinoTrip.Cache;


namespace SinoTrip.WebView.Scenic
{
    public partial class Category : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var typeData = SceneryCache.GetTypeCache(0, "").OrderBy(item => item.OrderNo).ToList();
            Vt.Put("Categorys", typeData.ToJson());
            Vt.Put("Scenerys", this.SecCache.Select(item => new { Name = item.Name, ItemId = item.ItemId, OutSign = item.OutSign, TypeId = item.TypeId }).ToJson());
            Vt.Display("Scenic/Category.html");
        }
    }
}