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
using SinoTrip.API.LY.Model;
using SinoTrip.API.LY.Biz;

namespace SinoTrip.WebView
{
    public partial class Default : TemplateBase
    {
        public static List<Entity.DataBase.Common.common_scenery_type> ThemeCache;
        public List<ViewScenery> _SceneryCache;
        protected string areaName = "安徽";
        protected string cityName = "合肥";
        private string provinceId = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            string _area = Context.Request["area"];
            var provices = AreaCache.GetAreaCache(0, 0, "", "");
            //string[] names = new string[] { "香港","台湾","澳门" };//         
            //provices = provices.Where(item => !names.Contains(item.Name)).ToList();
            ViewArea provice = null;
            try
            {
                if (!string.IsNullOrEmpty(_area))
                {
                    provice = provices.FirstOrDefault(item => _area == item.English);
                }
                else
                {
                    if(Session!=null)
                    {
                        _area = Session["area"].ToString();
                        provice = provices.FirstOrDefault(item => _area == item.English);
                    }
                    if (string.IsNullOrEmpty(_area))
                    {
                        var ip = SinoTrip.FrameWork.Web.GetIP.IPAddress;
                        var url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip;
                        HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                        request.Method = "Get";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        using (var strem = response.GetResponseStream())
                        {
                            StreamReader sr = new StreamReader(strem, Encoding.UTF8);
                            string ret = sr.ReadToEnd();
                            var ipmodel = ret.JsonDeserialize<API.Taobao.Model.Ip>();
                            if (ipmodel.code == "0")
                            {
                                if (ipmodel.data.country_id == "CN")
                                {
                                    areaName = ipmodel.data.region.Trim();
                                    cityName = ipmodel.data.city.Replace("市", "").Trim();
                                }
                            }

                            sr.Close();
                        }

                    }
                }
            }
            catch (Exception)
            {


            }
            if (provice == null)
            {
                provice = provices.FirstOrDefault(item => areaName.Contains(item.Name));
            }
            areaName = provice.Name;
            var cityData = AreaCache.GetCityCache(0, provice.Name, true, "");

            QueryScenery pq = new QueryScenery();
            pq.page = 1;
            pq.pageSize = 10;
            pq.cs = 2;
            pq.sortType = "0";
            if (areaName == "香港")
            {
                pq.provinceId = cityData.Where(item => item.Name == "香港").FirstOrDefault().OutSign.ToInt32(0);
            }
            else
            {
                pq.provinceId = provice.OutSign.ToInt32(0);
            }
            var cityItem = cityData.Where(item => item.Name == cityName).FirstOrDefault();
            if (string.IsNullOrEmpty(_area))
            {
                pq.cityId = cityData.Where(item => item.Name == cityName).FirstOrDefault().OutSign.ToInt32(0);
            }


            provinceId = provice.OutSign;

            var data = new ScenicBiz().QueryScenery(pq);

            if (data.totalCount < 2)
            {
                pq.provinceId = provices.FirstOrDefault(item => item.Name == "安徽").OutSign.ToInt32(0);
                pq.cityId = 0;
                data = new ScenicBiz().QueryScenery(pq);
            }
            ThemeCache = SceneryCache.GetTypeCache(0, "").OrderBy(item => item.OrderNo).ToList();
            Session["area"] = _area;
            Vt.Put("menuCity", cityData.Take(3));
            Vt.Put("areaName", areaName);
            Vt.Put("areaId", provice.ItemId);
            Vt.Put("Themes", ThemeCache);
            Vt.Put("ZBJD", data);
            Vt.Put("Provice", provices);
            Vt.Put("City", cityData);
            Vt.Put("ThisPage", this);
            Vt.Put("isIndex", 1);
            Vt.Display("default.htm");
            

        }

        public List<ViewCity> GetHotCity(int size)
        {
            //_SceneryCache = SceneryCache.GetSceneryCache(0, areaName, 0, 0, 0, "", "", "");
            var cids = AreaCache.GetCityCache(0, areaName, true, "").Select(item => item.ItemId).ToList();
            _SceneryCache = this.SecCache.Where(item => cids.Contains(item.CityId)).ToList();
            var rs = new List<ViewCity>();
            var cityData = AreaCache.GetCityCache(0, areaName, true, "");
            foreach (var item in cityData)
            {
                if (rs.Count == size)
                    break;
                if (_SceneryCache.Where(r => r.CityId == item.ItemId).Count() > 10)
                {
                    rs.Add(item);
                }
            }
            return rs;
        }

        /// <summary>
        /// 获取热门景点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public sceneryList GetHotScenery(string ids)
        {
            string[] _ids = ids.Split(',');
            List<int> PostId = new List<int>();
            foreach (var item in _ids)
            {
                var _id = item.ToInt32(0);
                if (_id > 0)
                {
                    PostId.Add(ThemeCache.FirstOrDefault(r => r.ItemId == _id).OrderNo.ToInt32(0));
                }
            }
            QueryScenery pq = new QueryScenery();
            pq.page = 1;
            pq.pageSize = 10;
            pq.cs = 2;
            pq.sortType = "0";
            pq.themeId = string.Join(",", PostId);
            return new ScenicBiz().QueryScenery(pq);
        }


    }
}