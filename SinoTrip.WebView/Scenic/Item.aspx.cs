using System;
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
using System.IO;
using SinoTrip.Core;

namespace SinoTrip.WebView.Scenic
{
    public partial class Item : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int id = Context.Request["id"].ToInt32(0);
                string _outSign = Context.Request["outsign"];
                if (id <= 0 && string.IsNullOrEmpty(_outSign))
                    Context.Response.Redirect("/");
                var data = SceneryCache.GetSceneryCache(id, "", 0, 0, 0, "", "", _outSign);
                if (data != null || data.Count > 0)
                {
                    var Info = data.FirstOrDefault();
                    id = Info.ItemId;
                    Info.BuyNotie = Info.BuyNotie.Replace("同程", "驾驴");
                    Info.Intro = Info.Intro.Replace("同程", "驾驴");
                    var outSign = Info.OutSign.ToInt32(0);
                    // new ScenicBiz()var rs = new ScenicBiz().GetSceneryDetail(outSign);
                    List<SinoTrip.Entity.DataBase.Common.common_scenery_img> imgs = new SinoTrip.Biz.SceneryBiz().GetImage(id, 7);
                  

                    if (imgs.Count == 0)
                    {
                        imgs = GetImg(id, _outSign);
                    }
                    if (imgs.Count < 7)
                    {
                        for (int i = 0; i <= 7 - imgs.Count; i++)
                        {
                            imgs.Add(new SinoTrip.Entity.DataBase.Common.common_scenery_img() { Cover = "http://www.cartrip.cc/data/attachment/block/61/61b23c840791af8254616892874ddc88.jpg" });
                        }
                    }
                    Vt.Put("Imgs", imgs);
                    Vt.Put("Info", Info);
                }

            }
            catch (Exception ex)
            {

                //Context.Response.Redirect("/");
            }
            finally
            {
                Vt.Display("Scenic/Item.html");
            }

        }

        List<SinoTrip.Entity.DataBase.Common.common_scenery_img> GetImg(int id, string _outSign)
        {

            var imgrs = new ScenicBiz().GetSceneryImageList(_outSign.ToInt32(0), 1, 10);
            var model = imgrs.XmlToEntity<SinoTrip.API.LY.Model.imageRs>();
            if (model != null)
            {
                if (model.imageList != null || model.extInfoOfImageList != null)
                {
                    var dal = new SinoTrip.DAL.Common.common_scenery_img();
                    foreach (var item in model.imageList.images)
                    {
                        string gpath = Guid.NewGuid().ToString();

                        string vpath = "Upload/Scenery/Images/" + id + "/" + gpath + "/";
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vpath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        foreach (var image in model.extInfoOfImageList.sizeCodeList)
                        {
                            if (!string.IsNullOrEmpty(image.sizeInfo))
                            {
                                var _index = image.sizeInfo.IndexOf("_");
                                int width = image.sizeInfo.Substring(0, _index).ToInt32(0);
                                int height = image.sizeInfo.Substring(_index + 1).ToInt32(0);
                                var url = model.extInfoOfImageList.imageBaseUrl + image.sizeInfo + "/" + item.imagePath.InnerXml;

                                System.Net.WebClient web = new System.Net.WebClient();

                                int nPos = url.LastIndexOf('.');
                                string FileName = url.Substring(nPos + 1);
                                web.DownloadFile(url, path + image.sizeInfo + "." + FileName);

                                if (image.isDefault == "True")
                                {
                                    var imgmodel = new SinoTrip.Entity.DataBase.Common.common_scenery_img();
                                    imgmodel.Width = width;
                                    imgmodel.Height = height;
                                    imgmodel.SceneryId = id;
                                    imgmodel.Folder = "/" + vpath;
                                    imgmodel.Cover = "/" + vpath + image.sizeInfo + "." + FileName;
                                    imgmodel.Status = 0;
                                    imgmodel.CreateTime = DateTime.Now.ToUnixInt();
                                    //modelAll.Add(imgmodel);
                                    dal.Add(imgmodel);
                                }
                            }
                        }
                    }
                }

            }
            return new SinoTrip.Biz.SceneryBiz().GetImage(id, 7);
        }

        void Update()
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var dal = new SinoTrip.DAL.Common.common_scenery();
            var data = SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "", string.Empty);
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