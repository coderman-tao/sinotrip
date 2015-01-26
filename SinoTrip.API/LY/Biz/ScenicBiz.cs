using SinoTrip.API.LY.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SinoTrip.FrameWork.Common;
using SinoTrip.API.LY.Model;
using System.Reflection;
using SinoTrip.FrameWork.Utils;

namespace SinoTrip.API.LY.Biz
{
    public class ScenicBiz
    {
        /// <summary>
        /// 获取同程网省份信息
        /// </summary>
        /// <returns></returns>
        public string GetProvinceList()
        {
            string rs = ApiCommon.GetResult("", "GetProvinceList", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }
        /// <summary>
        /// 根据省份获取同程网城市信息
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns></returns>
        public string GetCityListByProvinceId(int Pid)
        {
            string rs = ApiCommon.GetResult("<provinceId>" + Pid + "</provinceId>", "GetCityListByProvinceId", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }

        public string GetCountyListByCityId(int cid)
        {
            string rs = ApiCommon.GetResult("<cityId>" + cid + "</cityId>", "GetCountyListByCityId", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }
        /// <summary>
        /// 根据城市获取景点
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetSceneryList(int CityId, int page)
        {
            string rs = ApiCommon.GetResult("<clientIp>127.0.0.1</clientIp><page>" + page + "</page><pageSize>100</pageSize><cityId>" + CityId + "</cityId>",
                "GetSceneryList", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            return rs;
        }

        /// <summary>
        /// 查询景点列表
        /// </summary>
        /// <param name="pq"></param>
        /// <returns></returns>
        public sceneryList QueryScenery(QueryScenery pq)
        {
            string request = "";
            PropertyInfo[] pInfos = typeof(QueryScenery).GetProperties();
            foreach (var pInfo in pInfos)
            {
                //if(typeof(f).is)
                if (pInfo.GetValue(pq, null) != null)
                {
                    request += "<" + pInfo.Name + ">" + pInfo.GetValue(pq, null) + "</" + pInfo.Name + ">";
                }
            }
            string rs = ApiCommon.GetResult(request, "GetSceneryList", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx").Replace("&", "&amp;");
            rs = GetJQRs(rs);
            var model = rs.XmlToEntity<sceneryList>();
            return model;
        }

        public string GetSceneryDetail(int SceneryId)
        {
            string PostData = "<sceneryId>" + SceneryId + "</sceneryId><cs>2</cs>";
            string rs = ApiCommon.GetResult(PostData, "GetSceneryDetail", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            return rs;

        }
        /// <summary>
        /// 获取景点交通信息
        /// </summary>
        /// <param name="sceneryId"></param>
        /// <returns></returns>
        public string GetSceneryTrafficInfo(int sceneryId)
        {
            string PostData = "<sceneryId>" + sceneryId + "</sceneryId>";
            string rs = ApiCommon.GetResult(PostData, "GetSceneryTrafficInfo", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            return rs;
        }

        /// <summary>
        /// 获取景区价格
        /// </summary>
        /// <param name="showDetail">1、简单 2、详细默认为1</param>
        /// <param name="ids">可以传入多个逗号分隔，必填。 示例：3440,1360,79,…，一次最多20个</param>
        /// <returns></returns>
        public SinoTrip.API.LY.Model.SceneryPrice GetSceneryPrice(int showDetail, List<int> ids)
        {
            string sceneryIds = string.Join(",", ids);
            string PostData = "<showDetail>" + showDetail + "</showDetail><sceneryIds>" + sceneryIds + "</sceneryIds><payType>0</payType>";

            string rs = ApiCommon.GetResult(PostData, "GetSceneryPrice", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            //rs = rs.Replace("&", "&amp;");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rs);
            return doc.SelectSingleNode("response/body").InnerXml.XmlToEntity<SinoTrip.API.LY.Model.SceneryPrice>();
            //return rs;
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

        /// <summary>
        /// 获取景点图片
        /// </summary>
        /// <param name="SceneryId"></param>
        /// <returns></returns>
        public string GetSceneryImageList(int SceneryId, int page, int pageSize)
        {
            string PostData = "<sceneryId>" + SceneryId + "</sceneryId>";
            string rs = ApiCommon.GetResult(PostData, "GetSceneryImageList", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            if (!string.IsNullOrEmpty(rs))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rs);
                return "<body>" + doc.SelectSingleNode("response/body").InnerXml + "</body>";
            }
            return rs;
        }

        /// <summary>
        /// 获取周边景点
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="SceneryId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public SinoTrip.API.LY.Model.NearByScenerys GetNearbyScenery(int SceneryId, int page, int pageSize)
        {
            string PostData = "<sceneryId>" + SceneryId + "</sceneryId><page>" + page + "</page><pageSize>" + pageSize + "</pageSize>";
            string rs = ApiCommon.GetResult(PostData, "GetNearbyScenery", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            if (!string.IsNullOrEmpty(rs))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rs);
                rs = doc.SelectSingleNode("response/body").InnerXml;
            }
            return rs.XmlToEntity<SinoTrip.API.LY.Model.NearByScenerys>(); ;
        }

        public List<SinoTrip.Entity.DataBase.Common.common_scenery> DownLoadScenery(List<SinoTrip.Entity.ViewModel.ViewScenery> data, List<Entity.DataBase.Common.common_scenery_type> typeCache)
        {
            string rs = GetSceneryList(0, 1).Replace("&", "&amp;");
            string str = GetJQRs(rs);
            List<SinoTrip.Entity.DataBase.Common.common_scenery> outsigns = new List<SinoTrip.Entity.DataBase.Common.common_scenery>();
            var obj = str.XmlToEntity<SinoTrip.API.LY.Model.sceneryList>();
            if (obj != null)
            {
                try
                {
                    for (int i = 2; i <= obj.totalPage; i++)
                    {
                        str = GetJQRs(GetSceneryList(0, i).Replace("&", "&amp;"));
                        var model = str.XmlToEntity<SinoTrip.API.LY.Model.sceneryList>();
                        foreach (var item in model.Scenerys)
                        {
                            var f = data.FirstOrDefault(r => r.OutSign == item.sceneryId);
                            if (f == null)
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
                                var th = item.themeList.FirstOrDefault();
                                if (th != null)
                                {
                                    themeName = th.themeName.Trim();
                                }
                                var type = typeCache.FirstOrDefault(r => r.Name.Equals(themeName));
                                if (type != null)
                                {
                                    s.TypeId = type.ItemId;
                                }
                                s.ThemeName = themeName;
                                outsigns.Add(s);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return outsigns;
        }
        // public string GetPriceCalendar()
    }
}
