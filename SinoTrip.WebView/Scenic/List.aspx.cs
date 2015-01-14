﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.Cache;
using SinoTrip.FrameWork.Common;
using SinoTrip.API.LY.Model;
using SinoTrip.Entity.ViewModel;
using SinoTrip.API.LY.Biz;

namespace SinoTrip.WebView.Scenic
{
    public partial class List : TemplateBase
    {
        string areaName = "安徽";
        int areaId = 0;
        int cityId = 0;
        int county = 0;
        int type = 0;
        int grade = 0;
        int price = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            areaId = Context.Request["provice"].ToInt32(0);
            cityId = Context.Request["city"].ToInt32(0);
            county = Context.Request["county"].ToInt32(0);
            type = Context.Request["type"].ToInt32(0);
            grade = Context.Request["grade"].ToInt32(0);
            price = Context.Request["price"].ToInt32(0);

            QueryScenery pq = new QueryScenery();
            pq.page = 1;
            pq.pageSize = 10;
            pq.cs = 2;

            var areaData = new List<ViewArea>();
            var cityArea = new List<ViewCityArea>();
            if (areaId > 0)
            {
                areaData = AreaCache.GetAreaCache(areaId, 0, "");
                areaName = areaData.FirstOrDefault(item => item.ItemId == areaId).Name;
            }
            else
            {
                areaData = AreaCache.GetAreaCache(0, 0, "");
            }
            pq.provinceId = areaData.FirstOrDefault().OutSign.ToInt32(0);
            if (cityId > 0)
            {
                if (areaId==0)
                {
                    areaId = areaData.FirstOrDefault(r => r.Name == (AreaCache.GetCityCache(cityId, "", true).FirstOrDefault().Province)).ItemId;
                }
                pq.cityId = AreaCache.GetCityCache(cityId, "", true).FirstOrDefault().OutSign.ToInt32(0);
                cityArea = AreaCache.GetCityAreaCache(0, cityId, string.Empty);
            }
            if (areaId == 0 && cityId == 0)
            {
                cityArea = AreaCache.GetCityAreaCache(0, 0, "合肥");
            }

            var cityData = AreaCache.GetCityCache(0, areaName, true);

            if (cityData.Count > 0 && areaId > 0 && cityId == 0)
            {
                cityArea = AreaCache.GetCityAreaCache(0, 0, cityData.FirstOrDefault().Name);
            }
            if (county > 0)
            {
                var countyData=cityArea.FirstOrDefault(item=>item.ItemId==county);
                if (areaId == 0)
                {
                    areaId = areaData.FirstOrDefault(r => r.Name == (AreaCache.GetCityCache(countyData.CityID.ToInt32(0), "", true).FirstOrDefault().Province)).ItemId;
                }
                if (cityId == 0)
                {
                    cityId = AreaCache.GetCityCache(countyData.CityID.ToInt32(0), "", true).FirstOrDefault().ItemId;
                }
                pq.countryId = countyData.OutSign.ToInt32(0);
            }
            var typeData = SceneryCache.GetTypeCache(0, "");

            var data = new ScenicBiz().QueryScenery(pq);

            Vt.Put("Scenery", data);
            Vt.Put("TypeData", typeData);
            Vt.Put("CityArea", cityArea);
            Vt.Put("Area", areaData);
            Vt.Put("City", cityData);

            Vt.Put("provice", areaId);
            Vt.Put("city", cityId);
            Vt.Put("county", county);
            Vt.Put("type", type);
            Vt.Put("price", price);
            Vt.Put("grade", grade);
            Vt.Display("Scenic/List.html");
        }
    }
}