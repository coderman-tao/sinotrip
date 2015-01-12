using SinoTrip.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.Core;
using SinoTrip.FrameWork.Common;
using System.Data;

namespace SinoTrip.DAL.Common
{
    public class common_scenery
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SinoTrip.Entity.DataBase.Common.common_scenery model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into common_scenery(");
            strSql.Append("Name,Address,Summary,Cover,CityId,CityName,CountyId,CountyName,Grade,ThemeName,Lat,Lng,Intro,BuyNotie,Alias,Traffic,NearId,Status)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Address,@Summary,@Cover,@CityId,@CityName,@CountyId,@CountyName,@Grade,@ThemeName,@Lat,@Lng,@Intro,@BuyNotie,@Alias,@Traffic,@NearId,@Status)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@Name", MySqlDbType.VarChar,255),
					new MySqlParameter("@Address", MySqlDbType.Text),
					new MySqlParameter("@Summary", MySqlDbType.Text),
					new MySqlParameter("@Cover", MySqlDbType.VarChar,2000),
					new MySqlParameter("@CityId", MySqlDbType.Int32,11),
					new MySqlParameter("@CityName", MySqlDbType.VarChar,255),
					new MySqlParameter("@CountyId", MySqlDbType.Int32,11),
					new MySqlParameter("@CountyName", MySqlDbType.VarChar,255),
					new MySqlParameter("@Grade", MySqlDbType.VarChar,50),
					new MySqlParameter("@ThemeName", MySqlDbType.VarChar,500),
					new MySqlParameter("@Lat", MySqlDbType.Decimal,18),
					new MySqlParameter("@Lng", MySqlDbType.Decimal,18),
					new MySqlParameter("@Intro", MySqlDbType.Text),
					new MySqlParameter("@BuyNotie", MySqlDbType.Text),
					new MySqlParameter("@Alias", MySqlDbType.VarChar,255),
					new MySqlParameter("@Traffic", MySqlDbType.Text),
					new MySqlParameter("@NearId", MySqlDbType.VarChar,255),
					new MySqlParameter("@Status", MySqlDbType.Int32,11)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Address;
            parameters[2].Value = model.Summary;
            parameters[3].Value = model.Cover;
            parameters[4].Value = model.CityId;
            parameters[5].Value = model.CityName;
            parameters[6].Value = model.CountyId;
            parameters[7].Value = model.CountyName;
            parameters[8].Value = model.Grade;
            parameters[9].Value = model.ThemeName;
            parameters[10].Value = model.Lat;
            parameters[11].Value = model.Lng;
            parameters[12].Value = model.Intro;
            parameters[13].Value = model.BuyNotie;
            parameters[14].Value = model.Alias;
            parameters[15].Value = model.Traffic;
            parameters[16].Value = model.NearId;
            parameters[17].Value = model.Status;

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

        public DataTable GetIds()
        {
            string sql = "SELECT OutSign FROM common_scenery_outsign";
            return DALCore.GetSMDB().Query(sql).Tables[0];
        }

        public bool Update(API.LY.Model.sceneryDetail model)
        {
            string sql = "UPDATE common_scenery SET Intro='" + model.intro + "',BuyNotie='" + model.buyNotice + "',Alias='" + model.sceneryAlias + "' where Status=" + model.sceneryId + "";
            int rows = DALCore.GetSMDB().ExecuteSql(sql);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //UPDATE common_scenery SET Intro=a,BuyNotie=b,Alias=C where `Status`=C
        }

        public bool UpdateTrafficInfo(string info, int id)
        {
            string sql = "UPDATE common_scenery SET Traffic='" + info + "' where Status=" + id + "";
            int rows = DALCore.GetSMDB().ExecuteSql(sql);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetThemeNames()
        {
            string sql = "select DISTINCT ThemeName from common_scenery where LENGTH(ThemeName)>0";
            var dt = DALCore.GetSMDB().Query(sql).Tables[0];
            List<string> rs = new List<string>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string[] s = row[0].ToString().Split('|');
                    foreach (var _s in s)
                    {
                        if (!string.IsNullOrEmpty(_s)&&!rs.Contains(_s))
                            rs.Add(_s);
                    }
                }
            }
            return rs;
        }
    }
}
