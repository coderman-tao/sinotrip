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
    }
}
