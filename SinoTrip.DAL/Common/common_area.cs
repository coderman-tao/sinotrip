using SinoTrip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.FrameWork.Common;
using SinoTrip.Entity.ViewModel;

namespace SinoTrip.DAL.Common
{
   public class common_area
    {
       /// <summary>
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public List<ViewArea> GetList(ViewArea model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select A.OutSign,A.Supply,B.ItemId,B.Parent,B.Name,B.English,B.ABCD,B.Suoxie,B.Pinyin,B.Data,B.Status ");
            strSql.Append(" from common_area_outsign AS A LEFT JOIN common_area AS B ON A.AreaId=B.ItemId");
            strSql.Append(" WHERE B.Status=0");
            return DALCore.GetSMDB().Query(strSql.ToString()).Tables[0].ToList<ViewArea>();
        }
    }
}
