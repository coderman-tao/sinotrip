﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.FrameWork.Common;
using SinoTrip.Cache;
using SinoTrip.API.LY.Biz;
using System.Xml;
using System.Reflection;
using System.Data;

namespace SinoTrip.WebView.Scenic
{
    public partial class Item : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int id = Context.Request["id"].ToInt32(0);
                if(id<=0)
                    Context.Response.Redirect("/");
                var data = SceneryCache.GetSceneryCache(id, "", 0, 0, 0, "", "");
                if (data != null || data.Count > 0)
                {
                    var Info = data.FirstOrDefault();
                    Info.BuyNotie = Info.BuyNotie.Replace("同程", "驾驴");
                    Info.Intro = Info.Intro.Replace("同程", "驾驴");
                    var outSign = Info.OutSign.ToInt32(0);
                    var rs = new ScenicBiz().GetSceneryDetail(outSign);
                    Vt.Put("Info", Info);
                    Vt.Display("Scenic/Item.html");
                }
               
            }
            catch (Exception ex)
            {

                //Context.Response.Redirect("/");
            }
            
        }

        void Update()
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var dal = new SinoTrip.DAL.Common.common_scenery();
            var data = SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "");
            foreach (var id in data)
            { 
                try
                {
                    string rs = biz.GetSceneryDetail(id.OutSign.ToInt32(0));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(rs);

                    string sss = biz.GetSceneryTrafficInfo(id.OutSign.ToInt32(0));
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(sss);
                    var s = doc1.SelectSingleNode("response/body/scenery/traffic").ChildNodes[0].Value;

                    XmlNodeList nodeList = doc.SelectSingleNode("response/body/scenery").ChildNodes;
                    SinoTrip.API.LY.Model.sceneryDetail model = new API.LY.Model.sceneryDetail();
                    foreach (XmlElement item in nodeList)
                    {
                        string r = item.InnerText;
                        //item.InnerXml = item.InnerText;
                        //item.Value
                        PropertyInfo[] pInfos = typeof(SinoTrip.API.LY.Model.sceneryDetail).GetProperties();
                        foreach (PropertyInfo pInfo in pInfos)
                        {
                            if (item.Name == pInfo.Name)
                            {
                                try
                                {
                                    if (pInfo.PropertyType.IsGenericType)
                                        pInfo.SetValue(model, Convert.ChangeType(r, pInfo.PropertyType.GetGenericArguments()[0]), null);
                                    else
                                        pInfo.SetValue(model, Convert.ChangeType(r, pInfo.PropertyType), null);
                                }
                                catch
                                {
                                }
                                continue;
                            }
                        }
                    }
                    SinoTrip.Entity.DataBase.Common.common_scenery cs = new Entity.DataBase.Common.common_scenery();
                    cs.ItemId = id.ItemId;
                    cs.Intro = model.intro;
                    cs.BuyNotie = model.buyNotice;
                    cs.Traffic = s;
                    dal.UpdateIntro(cs);
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}