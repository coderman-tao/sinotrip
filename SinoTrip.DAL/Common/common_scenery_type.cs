using SinoTrip.Core;
using SinoTrip.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.DAL.Common
{
    public class common_scenery_type
    {
        public List<Entity.DataBase.Common.common_scenery_type> GetList(Entity.DataBase.Common.common_scenery_type model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemId,Name,OrderNo,Status ");
            strSql.Append(" FROM common_scenery_type ");
            return DALCore.GetSMDB().Query(strSql.ToString()).Tables[0].ToList<Entity.DataBase.Common.common_scenery_type>();
        }
    }
}
