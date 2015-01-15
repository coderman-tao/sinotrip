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
            return result.OrderBy(item=>item.OrderNo).ToList();
        }
    }
}
