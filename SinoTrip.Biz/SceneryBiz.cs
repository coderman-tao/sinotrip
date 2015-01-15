using SinoTrip.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Biz
{
    public class SceneryBiz
    {
        public List<Entity.DataBase.Common.common_scenery_type> GetTypeList(Entity.DataBase.Common.common_scenery_type model)
        {
            return new DAL.Common.common_scenery_type().GetList(model);
        }
        /// <summary>
        /// 查询所有有效景点
        /// </summary>
        /// <returns></returns>
        public List<ViewScenery> QueryAllSecery()
        {
            return new DAL.Common.common_scenery().QueryAll();
        }
    }
}
