using SinoTrip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.FrameWork.Common;
using MySql.Data.MySqlClient;
using SinoTrip.Entity.ViewModel;

namespace SinoTrip.DAL.Common
{
   public class common_city_area
    {
       public List<ViewCityArea> GetList(ViewCityArea model)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("select A.OutSign,A.Supply,B.ItemId,B.Name,B.CityID,B.CityName,B.Status FROM common_city_area_outsign AS A");
           strSql.Append(" LEFT JOIN common_city_area AS B ON A.AreaId=B.ItemId");
           strSql.Append(" WHERE B.Status=0");
           return DALCore.GetSMDB().Query(strSql.ToString()).Tables[0].ToList<ViewCityArea>();
       }

       public bool Add(Entity.DataBase.Common.common_city_area model)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("insert into common_city_area(");
           strSql.Append("Name,English,ABCD,CityID,CityName,Status)");
           strSql.Append(" values (");
           strSql.Append("@Name,@English,@ABCD,@CityID,@CityName,@Status)");
           MySqlParameter[] parameters = {
					new MySqlParameter("@Name", MySqlDbType.VarChar,255),
					new MySqlParameter("@English", MySqlDbType.VarChar,255),
					new MySqlParameter("@ABCD", MySqlDbType.VarChar,1),
					new MySqlParameter("@CityID", MySqlDbType.Int32,11),
					new MySqlParameter("@CityName", MySqlDbType.VarChar,255),
					new MySqlParameter("@Status", MySqlDbType.Int16,6)};
           parameters[0].Value = model.Name;
           parameters[1].Value = model.English;
           parameters[2].Value = model.ABCD;
           parameters[3].Value = model.CityID;
           parameters[4].Value = model.CityName;
           parameters[5].Value = model.Status;

           int rows = DALCore.GetSMDB().ExecuteSql(strSql.ToString(), parameters);
           if (rows > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
    }
}
