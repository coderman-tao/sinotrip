using SinoTrip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.FrameWork.Common;
using SinoTrip.Entity.ViewModel;
using System.Data;

namespace SinoTrip.DAL.Common
{
    public class common_city
    {
        public List<ViewCity> GetList(ViewCity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select A.OutSign,A.Supply,B.ItemId,B.Country,B.Province,B.Name,B.ABCD,B.Suoxie,B.Pinyin,B.Lat,B.Lng,B.IsBigCity,B.Status ");
            strSql.Append("from common_city_outsign AS A LEFT JOIN common_city AS B ON A.CityId=B.ItemId");
            strSql.Append(" WHERE B.Status=0");
            return DALCore.GetSMDB().Query(strSql.ToString()).Tables[0].ToList<ViewCity>();
        }

       
    }
}
