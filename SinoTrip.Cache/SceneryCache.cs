using SinoTrip.Entity.ViewModel;
using SinoTrip.FrameWork.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Cache
{
    public class SceneryCache
    {
        /// <summary>
        /// 获取景区类型
        /// </summary>
        /// <returns></returns>
        public static List<Entity.DataBase.Common.common_scenery_type> SetTypeCache()
        {
            var result = new Biz.SceneryBiz().GetTypeList(new Entity.DataBase.Common.common_scenery_type());
            FileCache.SetCache("TypeCache", result, "scenery");
            return result;
        }


        /// <summary>
        /// 获取景区类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CityId"></param>
        /// <param name="CityName"></param>
        /// <returns></returns>
        public static List<Entity.DataBase.Common.common_scenery_type> GetTypeCache(int id, string Name)
        {
            var result = FileCache.ReadCache("TypeCache", DateTime.MinValue, "scenery") as List<Entity.DataBase.Common.common_scenery_type>;
            if (result == null)
            {
                result = SetTypeCache();
            }
            if (id > 0)
            {
                result = result.Where(item => item.ItemId == id).ToList();
            }

            if (!string.IsNullOrEmpty(Name))
            {
                result = result.Where(item => item.Name == Name).ToList();
            }
            return result.OrderBy(item => item.OrderNo).ToList();
        }

        /// <summary>
        /// 设置区域缓存
        /// </summary>
        /// <returns></returns>
        public static List<ViewScenery> SetSceneryCache()
        {
            var data = new SinoTrip.Biz.SceneryBiz().QueryAllSecery();
            FileCache.SetCache("SeceryCache", data, "scenery");
            return data;
        }

        public static List<ViewScenery> GetSceneryCache(int id, string proviceName, int cityId, int countyId, int typeId, string grade, string name,string outSign)
        {
            var result = FileCache.ReadCache("SeceryCache", DateTime.MinValue, "scenery") as List<ViewScenery>;
            if (result == null)
            {
                result = SetSceneryCache();
            }
            if (id > 0)
            {
                result = result.Where(item => item.ItemId == id).ToList();
                return result;
            }
            if (!string.IsNullOrEmpty(proviceName))
            {
                var cids = AreaCache.GetCityCache(0, proviceName, true).Select(item => item.ItemId).ToList();
                result = result.Where(item => cids.Contains(item.CityId)).ToList();
            }
            if (cityId > 0)
            {
                result = result.Where(item => item.CityId == cityId).ToList();
            }
            if (countyId > 0)
            {
                result = result.Where(item => item.CountyId == countyId).ToList();
            }
            if (typeId > 0)
            {
                result = result.Where(item => item.TypeId == typeId).ToList();
            }
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(item => item.Name.Contains(name) || name.Contains(item.Name) || item.Alias.Contains(name) || item.CityName.Contains(name) || name.Contains(item.CityName)).ToList();
            }
            if (!string.IsNullOrEmpty(outSign))
            {
                result = result.Where(item => item.OutSign == outSign).ToList();
            }
            return result;
        }
    }
}
