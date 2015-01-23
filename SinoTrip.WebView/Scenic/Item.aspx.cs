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
using System.Net;
using System.Text;

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
                var Info = new Biz.SceneryBiz().GetItem(id, _outSign,"");
                if (Info != null)
                {
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
                    List<int> ids = new List<int>();

                    ids.Add(_outSign.ToInt32(0));

                    var model = new SinoTrip.API.LY.Biz.ScenicBiz().GetSceneryPrice(2, ids);
                    if (model != null && model.scenery != null)
                    {
                        Vt.Put("Price", model.scenery.FirstOrDefault());
                    }
                }
                else
                {

                    InsertOutScenery(_outSign);
                    SceneryCache.SetSceneryCache();
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

        void InsertOutScenery(string outsign)
        {
            //1、插入主库，Status存储外部标识
            var typeData = SceneryCache.GetTypeCache(0, "").OrderBy(item => item.OrderNo).ToList();
            var ids = new SinoTrip.API.LY.Biz.ScenicBiz().DownLoadScenery(outsign.ToInt32(0), SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "", ""), typeData);
            var dal = new SinoTrip.DAL.Common.common_scenery();

            foreach (var item in ids)
            {
                dal.Add(item);
            }
            //2、插入外部标识表
            if (dal.InsertOutSignByStatus(2))
            {
                Update();
                GetJQDP();
            }
        }

        /// <summary>
        /// 更新交通信息以及购票须知等
        /// </summary>
        void Update()
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var dal = new SinoTrip.DAL.Common.common_scenery();
            var ids = new SinoTrip.DAL.Common.common_scenery().GetIds();
            foreach (DataRow row in ids.Rows)
            {
                try
                {
                    var outsign = row[1].ToInt32(0);
                    var id = row[0].ToInt32(0);
                    GetImg(id, row[1].ToString());

                    //string rs = biz.GetSceneryDetail(outsign);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(rs);

                    //string sss = biz.GetSceneryTrafficInfo(outsign);
                    //XmlDocument doc1 = new XmlDocument();
                    //doc1.LoadXml(sss);
                    //var s = doc1.SelectSingleNode("response/body/scenery/traffic").ChildNodes[0].Value;

                    //XmlNodeList nodeList = doc.SelectSingleNode("response/body/scenery").ChildNodes;
                    //SinoTrip.API.LY.Model.sceneryDetail model = new API.LY.Model.sceneryDetail();
                    //foreach (XmlElement item in nodeList)
                    //{
                    //    string r = item.InnerText;
                    //    //item.InnerXml = item.InnerText;
                    //    //item.Value
                    //    PropertyInfo[] pInfos = typeof(SinoTrip.API.LY.Model.sceneryDetail).GetProperties();
                    //    foreach (PropertyInfo pInfo in pInfos)
                    //    {
                    //        if (item.Name == pInfo.Name)
                    //        {
                    //            try
                    //            {
                    //                if (pInfo.PropertyType.IsGenericType)
                    //                    pInfo.SetValue(model, Convert.ChangeType(r, pInfo.PropertyType.GetGenericArguments()[0]), null);
                    //                else
                    //                    pInfo.SetValue(model, Convert.ChangeType(r, pInfo.PropertyType), null);
                    //            }
                    //            catch
                    //            {
                    //            }
                    //            continue;
                    //        }
                    //    }
                    //}
                    //SinoTrip.Entity.DataBase.Common.common_scenery cs = new Entity.DataBase.Common.common_scenery();
                    //cs.ItemId = id;
                    //cs.Alias = model.sceneryAlias;
                    //cs.Intro = model.intro;
                    //cs.BuyNotie = model.buyNotice;
                    //cs.Traffic = s;
                    //dal.UpdateIntro(cs);
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }

        void GetJQDP()
        {
            var ids = new SinoTrip.DAL.Common.common_scenery().GetIds();
            var dal1 = new SinoTrip.DAL.Scenery.scenery_comment();
            var dal2 = new SinoTrip.DAL.Scenery.scenery_comment_img();
            foreach (DataRow row in ids.Rows)
            {
                try
                {
                    var _id = row[0].ToInt32(0);
                    var outsign = row[1].ToInt32(0);
                    string postUrl = "http://www.ly.com/Scenery/AjaxHelper/AjaxCall.aspx?action=GetDPList&sort=3&mon=0&type=0&sId=" + outsign + "&page=1&tagsulgId=0&isImg=0&sceneryType=20301&iid=0.7431465585250407&size=1000";
                    HttpWebRequest request = HttpWebRequest.Create(postUrl) as HttpWebRequest;
                    request.Method = "Get";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (var strem = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(strem, Encoding.UTF8);
                        string ret = sr.ReadToEnd();
                        DPData ss = ret.JsonDeserialize<DPData>();
                        sr.Close();
                        for (int i = 1; i <= ss.ShowList.PageCount; i++)
                        {
                            postUrl = "http://www.ly.com/Scenery/AjaxHelper/AjaxCall.aspx?action=GetDPList&sort=3&mon=0&type=0&sId=" + outsign + "&page=" + i + "&tagsulgId=0&isImg=0&sceneryType=20301&iid=0.7431465585250407&size=1000";
                            HttpWebRequest request1 = HttpWebRequest.Create(postUrl) as HttpWebRequest;
                            request1.Method = "Get"; ;
                            HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                            using (var strem1 = response1.GetResponseStream())
                            {
                                StreamReader sr1 = new StreamReader(strem1, Encoding.UTF8);
                                ret = sr1.ReadToEnd();
                                DPData dpdata = ret.JsonDeserialize<DPData>();
                                sr1.Close();
                                List<SinoTrip.Entity.DataBase.Scenery.scenery_comment> models = new List<Entity.DataBase.Scenery.scenery_comment>();
                                foreach (var item in dpdata.ShowList.DestinationCommentList)
                                {
                                    var tempdata = Guid.NewGuid().ToString();
                                    var m1 = new SinoTrip.Entity.DataBase.Scenery.scenery_comment()
                                    {
                                        SceneryId = _id,
                                        OutSign = outsign,
                                        Rank = DptypeToInt(item.DPType),
                                        DPTitle = item.DPTitle,
                                        Comment = item.DPContent,
                                        Uid = 0,
                                        UserName = item.DPUser,
                                        DPService = item.DPService,
                                        DPShiGouYu = item.DPShiGouYu,
                                        DPTraffic = item.DPTraffic,
                                        ServiceScore = item.ServiceScore,
                                        ServiceGrade = item.ServiceGrade,
                                        ConvenientScore = item.ConvenientScore,
                                        ConvenientGrade = item.ConvenientGrade,
                                        DiscountScore = item.DiscountScore,
                                        DiscountGrade = item.DiscountGrade,
                                        DPGanwu = item.DPGanWu,
                                        DPTime = DateTime.Parse(item.DPTime).ToUnixInt(),
                                        TempData = tempdata,
                                        Status = 0
                                    };
                                    models.Add(m1);
                                    if (item.PicList != null)
                                    {
                                        foreach (var pic in item.PicList)
                                        {
                                            var m2 = new SinoTrip.Entity.DataBase.Scenery.scenery_comment_img()
                                            {
                                                TempData = tempdata,
                                                ImgUrl = pic.WholePath
                                            };
                                            dal2.Add(m2);
                                        }
                                    }

                                }
                                if (models.Count > 0)
                                {
                                    dal1.BatchAdd(models);
                                }
                            }
                            //Thread.Sleep(1000);

                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }

            }
        }

        int DptypeToInt(string DPType)
        {
            switch (DPType)
            {
                case "好评":
                    return 1;
                case "中评":
                    return 2;
                case "差评":
                    return 3;
                default:
                    return 1;
            }
        }

        [Serializable]
        public class DPData
        {
            public int state { get; set; }
            public ShowInfo ShowList { get; set; }
        }
        [Serializable]
        public class ShowInfo
        {
            public int TotalNum { get; set; }
            public int GoodNum { get; set; }
            public int MidNum { get; set; }
            public int BadNum { get; set; }
            public int PageCount { get; set; }
            public int SceneryId { get; set; }
            public List<DestinationCommentList> DestinationCommentList { get; set; }
        }
        [Serializable]
        public class DestinationCommentList
        {
            //"DestinationCommentList":[{"LittleTool":"自驾车","PicList":null,"IsValid":0,"SerialId":null,"DCJQResponse":"","QueryNum":0,"FromSite":20402,"leve":1}]
            public string DPUser { get; set; }
            public string DPType { get; set; }
            public string DPUserPhoto { get; set; }
            public string DPTime { get; set; }
            public int ServiceScore { get; set; }
            public string ServiceGrade { get; set; }
            public int ConvenientScore { get; set; }
            public string ConvenientGrade { get; set; }
            public int DiscountScore { get; set; }
            public string DiscountGrade { get; set; }
            public string DPTitle { get; set; }
            public string DPContent { get; set; }
            public string DPService { get; set; }
            public string DPShiGouYu { get; set; }
            public string DPTraffic { get; set; }
            public string DPGanWu { get; set; }
            public List<PicList> PicList;
        }
        [Serializable]
        public class PicList
        {
            // [{"Id":666967,"IsCheckup":1,"Depiction":"","AddTime":"2014-12-3 18:25:00","SimplePath":"2014/12/03/26/201412031825007144113.jpg","WholePath":"http://upload.17u.com/uploadfile/2014/12/03/26/201412031825007144113.jpg"},{"Id":666968,"IsCheckup":1,"Depiction":"","AddTime":"2014-12-3 18:25:06","SimplePath":"2014/12/03/26/20141203182506636177.jpg","WholePath":"http://upload.17u.com/uploadfile/2014/12/03/26/20141203182506636177.jpg"},{"Id":666969,"IsCheckup":1,"Depiction":"","AddTime":"2014-12-3 18:25:17","SimplePath":"2014/12/03/26/20141203182517344702.jpg","WholePath":"http://upload.17u.com/uploadfile/2014/12/03/26/20141203182517344702.jpg"}]
            public string WholePath { get; set; }
        }
    }
}