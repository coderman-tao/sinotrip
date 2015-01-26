using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using SinoTrip.FrameWork.Common;
using SinoTrip.API.LY.Biz;
using SinoTrip.Cache;
using System.Data;
using System.Xml;
using System.Reflection;
using SinoTrip.Core;

namespace SinoTrip.WebBase
{
    public class WebTask
    {
        #region 创建单例
        // 定义一个静态变量来保存类的实例
        private static WebTask uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private WebTask()
        { }
        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static WebTask GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            lock (locker)
            {
                // 如果类的实例不存在则创建，否则直接返回
                if (uniqueInstance == null)
                {
                    uniqueInstance = new WebTask();
                }
            }

            return uniqueInstance;
        }
        #endregion

        private Thread thSecenery;

        public void GetLYSecenery()
        {
            if (thSecenery == null)
            {
                thSecenery = new Thread(delegate()
               {
                   while (true)
                   {
                       if (DateTime.Now.ToString("HH:mm:ss") == "00:00:00")
                       {
                           Thread.Sleep(1000);
                           GetNewDP();
                           DownLoad();
                           SceneryCache.SetSceneryCache();
                       }
                   }
               });
                thSecenery.Name = "GetLYSecenery";
                thSecenery.IsBackground = true;
                thSecenery.Start();
            }
            else
            {
                //if (thSecenery.IsAlive)
                //    thSecenery.Abort();
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

        void DownLoad()
        {
            //1、插入主库，Status存储外部标识
            var typeData = SceneryCache.GetTypeCache(0, "").OrderBy(item => item.OrderNo).ToList();
            var ids = new SinoTrip.API.LY.Biz.ScenicBiz().DownLoadScenery(SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "", ""), typeData);
            var dal = new SinoTrip.DAL.Common.common_scenery();

            foreach (var item in ids)
            {
                dal.Add(item);
            }
            //2、插入外部标识表
            if (dal.InsertOutSignByStatus(2))
            {
                Update();
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

                    string rs = biz.GetSceneryDetail(outsign);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(rs);

                    string sss = biz.GetSceneryTrafficInfo(outsign);
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
                    cs.ItemId = id;
                    cs.Alias = model.sceneryAlias;
                    cs.Intro = model.intro;
                    cs.BuyNotie = model.buyNotice;
                    cs.Traffic = s;
                    dal.UpdateIntro(cs);
                    dal.UpdateOutSignStatus(id);
                    GetJQDP(id, outsign);
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }

        void GetJQDP(int _id, int outsign)
        {
            ;
            var dal1 = new SinoTrip.DAL.Scenery.scenery_comment();
            var dal2 = new SinoTrip.DAL.Scenery.scenery_comment_img();
            try
            {
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
                LoggerCore.Error("抓取景点点评出错，景点ID:" + _id + "，外部标识：" + outsign);
            }

        }


        void GetNewDP()
        {
            var data = SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "", "");
            //var ids = new SinoTrip.DAL.Common.common_scenery().GetIds();
            var dal1 = new SinoTrip.DAL.Scenery.scenery_comment();
            var dal2 = new SinoTrip.DAL.Scenery.scenery_comment_img();
            foreach (var sec in data)
            {
                try
                {
                    var _id = sec.ItemId;
                    var outsign = sec.OutSign.ToInt32(0);
                    string postUrl = "http://www.ly.com/Scenery/AjaxHelper/AjaxCall.aspx?action=GetDPList&sort=3&mon=0&type=0&sId=" + outsign + "&page=1&tagsulgId=0&isImg=0&sceneryType=20301&iid=0.7431465585250407&size=100";
                    HttpWebRequest request = HttpWebRequest.Create(postUrl) as HttpWebRequest;
                    request.Method = "Get";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (var strem = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(strem, Encoding.UTF8);
                        string ret = sr.ReadToEnd();
                        DPData ss = ret.JsonDeserialize<DPData>();
                        sr.Close();
                        List<SinoTrip.Entity.DataBase.Scenery.scenery_comment> models = new List<Entity.DataBase.Scenery.scenery_comment>();
                        foreach (var item in ss.ShowList.DestinationCommentList)
                        {
                            var tempdata = Guid.NewGuid().ToString();
                            var dptime = DateTime.Parse(item.DPTime).ToString("yyyy-MM-dd");
                            if (dptime == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                            {
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
                        }

                        if (models.Count > 0)
                        {
                            dal1.BatchAdd(models);
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
