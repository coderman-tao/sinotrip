using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinoTrip.Biz;
using SinoTrip.FrameWork.IO;
using SinoTrip.Entity.ViewModel;

namespace SinoTrip.Cache
{
    public class AreaCache
    {
        /// <summary>
        /// 设置区域缓存
        /// </summary>
        public static List<ViewArea> SetAreaCache()
        {
            var result = new Biz.Common.AreaBiz().GetAreaList(new ViewArea());
            FileCache.SetCache("Area", result, "");
            return result;
        }

        /// <summary>
        /// 获取区域缓存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public static List<ViewArea> GetAreaCache(int id, int parent, string parentName, string name)
        {
            var result = FileCache.ReadCache("Area", DateTime.MinValue, "") as List<ViewArea>;
            if (result == null)
            {
                result = SetAreaCache();
            }
            if (!string.IsNullOrEmpty(parentName))
            {
                try
                {
                    var pid = result.FirstOrDefault(item => item.Name == parentName).ItemId;
                    result = result.Where(item => item.Parent == pid).ToList();
                }
                catch (Exception)
                {

                    return null;
                }

            }
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(item => item.Name == name).ToList();
            }
            if (id > 0)
            {
                result = result.Where(item => item.ItemId == id).ToList();
            }
            if (parent > 0)
            {
                result = result.Where(item => item.Parent == parent).ToList();
            }

            return result;
        }
        /// <summary>
        /// 设置城市缓存
        /// </summary>
        /// <returns></returns>
        public static List<ViewCity> SetCityCache()
        {
            var result = new Biz.Common.AreaBiz().GetCityList(new ViewCity());
            FileCache.SetCache("City", result, "");
            return result;
        }
        /// <summary>
        /// 获取城市缓存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public static List<ViewCity> GetCityCache(int id, string ProviceName, bool isBigCity, string name)
        {
            var result = FileCache.ReadCache("City", DateTime.MinValue, "") as List<ViewCity>;
            if (result == null)
            {
                result = SetCityCache();
            }
            if (id > 0)
            {
                result = result.Where(item => item.ItemId == id).ToList();
            }
            if (!string.IsNullOrEmpty(ProviceName))
            {
                result = result.Where(item => item.Province == ProviceName).ToList();
            }
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(item => item.Name == name).ToList();
            }
            if (isBigCity)
                result = result.Where(item => item.IsBigCity == 1).ToList();
            return result;
        }


        /// <summary>
        /// 设置城市区域缓存
        /// </summary>
        /// <returns></returns>
        public static List<ViewCityArea> SetCityAreaCache()
        {
            var result = new Biz.Common.AreaBiz().GetCityAreaList(new ViewCityArea());
            FileCache.SetCache("CityArea", result, "");
            return result;
        }
        /// <summary>
        /// 获取城市下区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CityId"></param>
        /// <param name="CityName"></param>
        /// <returns></returns>
        public static List<ViewCityArea> GetCityAreaCache(int id, int CityId, string CityName)
        {
            var result = FileCache.ReadCache("CityArea", DateTime.MinValue, "") as List<ViewCityArea>;
            if (result == null)
            {
                result = SetCityAreaCache();
            }
            if (id > 0)
            {
                result = result.Where(item => item.ItemId == id).ToList();
            }
            if (CityId > 0)
            {
                result = result.Where(item => item.CityID == CityId).ToList();
            }
            if (!string.IsNullOrEmpty(CityName))
            {
                result = result.Where(item => item.CityName == CityName).ToList();
            }
            return result;
        }
    }
}
