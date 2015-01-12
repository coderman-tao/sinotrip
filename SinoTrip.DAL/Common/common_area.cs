using SinoTrip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.FrameWork.Common;

namespace SinoTrip.DAL.Common
{
   public class common_area
    {
       /// <summary>
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        public List<Entity.DataBase.Common.common_area> GetList(Entity.DataBase.Common.common_area model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemId,Parent,Name,English,ABCD,Suoxie,Pinyin,Data,Status ");
            strSql.Append(" FROM common_area WHERE Status=0");
            return DALCore.GetSMDB().Query(strSql.ToString()).Tables[0].ToList<Entity.DataBase.Common.common_area>();
        }
    }
}
