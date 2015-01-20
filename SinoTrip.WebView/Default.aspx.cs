using SinoTrip.API.Ctrip.Model.Hotel.Request;
using SinoTrip.API.Ctrip.SDK;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SinoTrip.FrameWork.Common;
using SinoTrip.FrameWork.Utils;
using System.Reflection;
using System.Data;
using System.Threading;
using SinoTrip.Cache;
using SinoTrip.Entity.ViewModel;
using SinoTrip.Core;

namespace SinoTrip.WebView
{
    public partial class Default : System.Web.UI.Page
    {
        private static readonly string SID = ConfigurationManager.AppSettings["ctripWebID"];
        private static readonly string UID = ConfigurationManager.AppSettings["ctripID"];
        private static readonly string KEY = ConfigurationManager.AppSettings["ctripKey"];
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetJQImage();
           // GetJQDP();
            //var rs = new SinoTrip.API.LY.Biz.ScenicBiz().GetNearbyScenery(5305, 0, 10);
            //var dal = new SinoTrip.DAL.Common.common_scenery_img();
            //var data = SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "");
            //var data1 = data.Where(item => item.ItemId > 390).ToList();
            //string vpath = "Upload/Scenery/Images/";
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vpath);
            //DirectoryInfo d = new DirectoryInfo(path);
            //DirectoryInfo[] dis = d.GetDirectories();
            //foreach (DirectoryInfo di in dis)
            //{
            //    var _id = di.Name.ToInt32(0);
            //    if (_id > 390)
            //    {
            //        DirectoryInfo[] dis1 = di.GetDirectories();
            //        foreach (var item in dis1)
            //        {
            //            FileInfo[] fis = item.GetFiles();
            //            if (fis.Count() <= 0)
            //                continue;
            //            var defaultf = fis.Where(f => f.Name.ToLower() == "350_263.jpg").FirstOrDefault();
            //            if (defaultf != null)
            //            {
            //                var imgmodel = new SinoTrip.Entity.DataBase.Common.common_scenery_img();
            //                imgmodel.Width = 350;
            //                imgmodel.Height = 263;
            //                imgmodel.SceneryId = _id;
            //                imgmodel.Folder = "/" + vpath + _id + "/" + item.Name + "/";
            //                imgmodel.Cover = "/" + vpath + _id + "/" + item.Name + "/" + defaultf.Name;
            //                imgmodel.Status = 0;
            //                imgmodel.CreateTime = DateTime.Now.ToUnixInt();
            //                dal.Add(imgmodel);
            //            }
            //            else
            //            {
            //                var imgmodel = new SinoTrip.Entity.DataBase.Common.common_scenery_img();
            //                imgmodel.Width = 350;
            //                imgmodel.Height = 263;
            //                imgmodel.SceneryId = _id;
            //                imgmodel.Folder = "/" + vpath + _id + "/" + item.Name + "/";
            //                imgmodel.Cover = "/" + vpath + _id + "/" + item.Name + "/" + fis[0].Name;
            //                imgmodel.Status = 0;
            //                imgmodel.CreateTime = DateTime.Now.ToUnixInt();
            //                dal.Add(imgmodel);
            //            }
            //        }


            //    }
                //a.Add(di.Name);
            //    Console.WriteLine(di.Name);


            //}
            //Thread.Sleep(2000);
            //var dal = new SinoTrip.DAL.Common.common_scenery_img();
            //while (true)
            //{
            //    if (_c == (6387 - 4416))
            //    {
            //        Thread.Sleep(10000);
            //        foreach (var item in modelAll)
            //        {
            //            dal.Add(item);

            //        }
            //        _c = 0;
            //    }

            //}
            //Dictionary<int, int> SizeInfo = new Dictionary<int, int>();
            //foreach (var image in model.extInfoOfImageList.sizeCodeList)
            //{
            //    if (!string.IsNullOrEmpty(image.sizeInfo))
            //    {
            //        var _index = image.sizeInfo.IndexOf("_");
            //        int width = image.sizeInfo.Substring(0, _index).ToInt32(0);
            //        int height = image.sizeInfo.Substring(_index + 1).ToInt32(0);
            //        SizeInfo.Add(width, height);
            //    }
            //}


            //var rsssss = biz.QueryScenery(new SinoTrip.API.LY.Model.QueryScenery() { provinceId = 2, cityId = 45, page = 1, pageSize = 10 });
            // List<int> ids = new List<int>();
            //ids.Add(26737);
            //var model = biz.GetSceneryPrice(3, ids);
            //GetJQDP();
            Response.Clear();
            Response.Write("");
            Response.ContentType = "text/xml";
            Response.End();
        }

        void GetCountyListByCityId()
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var _id = 1452;
            if (_id > 0)
            {
                try
                {
                    string rs = biz.GetCountyListByCityId(_id).Replace("&", "&amp;");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(rs);
                    var Node = doc.SelectSingleNode("response/body");
                    XmlNodeList nodeList = Node.ChildNodes[0].ChildNodes;
                    foreach (XmlElement item in nodeList)
                    {
                        foreach (XmlElement i in item.ChildNodes)
                        {
                            i.InnerXml = i.InnerText;
                        }

                    }
                    var model = Node.InnerXml.XmlToEntity<SinoTrip.API.LY.Model.countyList>();

                    foreach (var item in model.Citys)
                    {
                        var a = new SinoTrip.Entity.DataBase.Common.common_city_area()
                        {
                            Name = item.name,
                            English = item.enName,
                            ABCD = item.prefixLetter,
                            CityID = 1452,
                            CityName = "三沙市",
                            Status = item.id.ToInt32(0)
                        };
                        new SinoTrip.DAL.Common.common_city_area().Add(a);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
            //var cityData = AreaCache.GetCityCache(0, "", true);
            //foreach (var city in cityData)
            //{

            //}
        }

        void CtripJQ()
        {
            Encoding encoding = System.Text.Encoding.GetEncoding("GBK");
            string f = System.Web.HttpUtility.HtmlDecode("%7b%22AllianceID%22%3a%221%22%2c%22SID%22%3a%221%22%2c%22ProtocolType%22%3a1%2c%22Signature%22%3a%22817D9FF70999176DE1F887D5CC4B732B%22%2c%22TimeStamp%22%3a%221371177110%22%2c%22Channel%22%3a%22Vacations%22%2c%22Interface%22%3a%22AddressSelectorInfoSearch%22%2c%22IsError%22%3afalse%2c%22RequestBody%22%3a%22%7b%5c%22AddressSelectorIDs%5c%22%3a%5b1%2c2%5d%7d%22%2c%22ResponseBody%22%3a%22%22%2c%22ErrorMessage%22%3a%22%22%7d");
            var remote = ConfigurationManager.AppSettings["ctrip_Api"] + "/vacations/OpenServer.ashx";
            string timeStamp;
            string signature;
            GetSignature(SID, UID, KEY, out timeStamp, out signature, "TicketSenicSpotSearch");
            var reqStr = @"<ScenicSpotSearchRequest><DistributionChannel>9</DistributionChannel><PagingParameter><PageIndex>1</PageIndex><PageSize>10</PageSize></PagingParameter><SearchParameter><Keyword>上海</Keyword><SaleCityID>2</SaleCityID></SearchParameter></ScenicSpotSearchRequest>";
            string json = "{\"AllianceID\":\"" + UID + "\",\"SID\":\"" + SID + "\",\"ProtocolType\":0,\"Signature\":\"" + signature + "\",\"TimeStamp\":\"" + timeStamp + "\",\"Channel\":\"Vacations\",\"Interface\":\"TicketSenicSpotSearch\",\"IsError\":false,\"RequestBody\": \"" + reqStr + "\",\"ResponseBody\":\"\",\"ErrorMessage\":\"\"}";
            var postData = System.Web.HttpUtility.UrlEncode(json, encoding);
            remote = remote + "?RequestJson=" + postData;




            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest request = HttpWebRequest.Create(remote) as HttpWebRequest;
            request.Method = "Get";
            //request.ContentLength = data.Length;
            //request.ContentType = "text/xml; charset=gb2312";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var strem = response.GetResponseStream();
            StreamReader sr = new StreamReader(strem, Encoding.UTF8);
            string ret = sr.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ret);
            var hotelNodes = xmlDoc.SelectSingleNode("/APIProtocolPackage/ResponseBody").InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(hotelNodes);
            var lo = xmlDoc1.SelectNodes("/ScenicSpotSearchResponse/ScenicSpotListItemList/ScenicSpotListItemDTO");
            sr.Close();
            response.Close();
        }
        protected void GetSignature(string sid, string allianceID, string secretKey, out string timeStamp, out string signature, string requestType)
        {
            //string allianceID = "1";
            //string SID = "50";
            //string SecretKey = "abcDFG645354";
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            timeStamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            string MD5SharedSecret = FormsAuthentication.HashPasswordForStoringInConfigFile(secretKey, "MD5");
            signature = FormsAuthentication.HashPasswordForStoringInConfigFile(timeStamp + allianceID + MD5SharedSecret + sid + requestType, "MD5");
        }

        void GetLYJQ()
        {

            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            for (int i = 1; i < 46; i++)
            {
                string rs = biz.GetSceneryList(0, i).Replace("&", "&amp;");
                string str = GetJQRs(rs);
                var obj = XmlUtil<SinoTrip.API.LY.Model.sceneryList>.XmlToEntity(str);
                var model = str.XmlToEntity<SinoTrip.API.LY.Model.sceneryList>();
                foreach (var item in model.Scenerys)
                {
                    var s = new SinoTrip.Entity.DataBase.Common.common_scenery();
                    s.Name = item.sceneryName;
                    s.Address = item.sceneryAddress;
                    s.Summary = item.scenerySummary;
                    s.Cover = model.imgbaseURL + item.imgPath;
                    s.CityName = item.cityName;
                    s.CityId = item.cityId.ToInt32(0);
                    s.CountyId = item.countyId.ToInt32(0);
                    s.CountyName = item.countyName;
                    s.Grade = item.gradeId;
                    s.Lat = item.lat.ToDecimal();
                    s.Lng = item.lon.ToDecimal();
                    s.Status = item.sceneryId.ToInt32(0);
                    string themeName = string.Empty;
                    foreach (var a in item.themeList)
                    {
                        themeName += a.themeName + "|";
                    }
                    s.ThemeName = themeName;
                    new SinoTrip.DAL.Common.common_scenery().Add(s);
                }
            }


        }

        void GetJQDeatil()
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var ids = new SinoTrip.DAL.Common.common_scenery().GetIds();
            var dal = new SinoTrip.DAL.Common.common_scenery();
            foreach (DataRow id in ids.Rows)
            {


                string rs = biz.GetSceneryDetail(id[0].ToInt32(0)).Replace("&", "&amp;");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rs);
                try
                {
                    XmlNodeList nodeList = doc.SelectSingleNode("response/body/scenery").ChildNodes;
                    SinoTrip.API.LY.Model.sceneryDetail model = new API.LY.Model.sceneryDetail();
                    foreach (XmlElement item in nodeList)
                    {
                        string r = item.InnerText;
                        if (item.ChildNodes.Count > 0)
                        {
                            r = HttpUtility.HtmlEncode(item.ChildNodes[0].Value);
                        }
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
                    dal.Update(model);
                }
                catch (Exception)
                {

                    continue;
                }



            }
        }

        string GetJQRs(string str)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            XmlNodeList sceneryNames = doc.GetElementsByTagName("sceneryName");
            foreach (XmlElement a in sceneryNames)
            {
                a.InnerXml = a.InnerText;

            }
            XmlNodeList imgPaths = doc.GetElementsByTagName("imgPath");
            foreach (XmlElement a in imgPaths)
            {
                a.InnerXml = a.InnerText;

            }
            XmlNodeList sceneryAddress = doc.GetElementsByTagName("sceneryAddress");
            foreach (XmlElement a in sceneryAddress)
            {
                a.InnerXml = a.InnerText;

            }
            XmlNodeList scenerySummary = doc.GetElementsByTagName("scenerySummary");
            foreach (XmlElement a in scenerySummary)
            {
                a.InnerXml = a.InnerText.Replace("<", "(").Replace(">", ")");

            }
            return doc.SelectSingleNode("response/body").InnerXml;
        }

        void GetJQTraffic()
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var ids = new SinoTrip.DAL.Common.common_scenery().GetIds();
            foreach (DataRow id in ids.Rows)
            {
                string sss = biz.GetSceneryTrafficInfo(id[0].ToInt32(0)).Replace("&", "&amp;");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sss);
                try
                {
                    var s = doc.SelectSingleNode("response/body/scenery/traffic").ChildNodes[0].Value;
                    s = HttpUtility.HtmlEncode(s);
                    new SinoTrip.DAL.Common.common_scenery().UpdateTrafficInfo(s, id[0].ToInt32(0));
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
                                if(models.Count>0)
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

        void GetJQImage()
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();
            var data = SceneryCache.GetSceneryCache(0, "", 0, 0, 0, "", "",string.Empty);
            var data1 = data.Where(item => item.ItemId > 4416).ToList();
            for (int i = 0; i < 10; i++)
            {
                var _data = data1.Where(item => item.ItemId > 4416 + (i * 200) && item.ItemId <= 4616 + (i * 200)).ToList();
                if (_data.Count > 0)
                {
                    Thread th1 = new Thread(delegate()
                    {
                        AddJQImg(_data);
                    });
                    th1.IsBackground = true;
                    th1.Start();
                }
            }

        }
        List<SinoTrip.Entity.DataBase.Common.common_scenery_img> modelAll = new List<SinoTrip.Entity.DataBase.Common.common_scenery_img>();
        List<SinoTrip.API.LY.Model.imageRs> tcimgModels = new List<API.LY.Model.imageRs>();

        private static object obj = new object();
        private static int _c = 0;
        public void AddJQImg(List<ViewScenery> data)
        {
            var biz = new SinoTrip.API.LY.Biz.ScenicBiz();

            foreach (var outitem in data)
            {
                var imgrs = biz.GetSceneryImageList(outitem.OutSign.ToInt32(0), 1, 10);
                var model = new SinoTrip.API.LY.Model.imageRs();
                lock (obj)
                {
                    _c++;
                    model = imgrs.XmlToEntity<SinoTrip.API.LY.Model.imageRs>();
                }
                if (model == null || model.imageList == null || model.extInfoOfImageList == null)
                    continue;

                tcimgModels.Add(model);

                foreach (var item in model.imageList.images)
                {
                    try
                    {
                        string gpath = Guid.NewGuid().ToString();

                        string vpath = "Upload/Scenery/Images/" + outitem.ItemId + "/" + gpath + "/";
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
                                    imgmodel.SceneryId = outitem.ItemId;
                                    imgmodel.Folder = "/" + vpath;
                                    imgmodel.Cover = "/" + vpath + image.sizeInfo + "." + FileName;
                                    imgmodel.Status = 0;
                                    imgmodel.CreateTime = DateTime.Now.ToUnixInt();
                                    modelAll.Add(imgmodel);
                                    //dal.Add(imgmodel);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerCore.Error(ex.Message, ex);
                        continue;
                    }


                }
            }
        }



        void GetJQItem()
        {

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
        public class Citys
        {
        }
        public class CityDetail
        {
            //    <CityCode>YTY</CityCode>
            //<City>15</City>
            //<CityName>扬州</CityName>
            //<CityEName>Yangzhou</CityEName>
            //<Country>1</Country>
            //<Province>15</Province>
            //<Airport />
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