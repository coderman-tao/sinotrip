using SinoTrip.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.Core;
using SinoTrip.FrameWork.Common;
using System.Data;
using SinoTrip.Entity.ViewModel;

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
            strSql.Append("Name,Address,Summary,TypeId,Cover,CityId,CityName,CountyId,CountyName,Grade,ThemeName,Lat,Lng,Intro,BuyNotie,Alias,Traffic,NearId,Status)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Address,@Summary,@TypeId,@Cover,@CityId,@CityName,@CountyId,@CountyName,@Grade,@ThemeName,@Lat,@Lng,@Intro,@BuyNotie,@Alias,@Traffic,@NearId,@Status)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@Name", MySqlDbType.VarChar,255),
					new MySqlParameter("@Address", MySqlDbType.Text),
					new MySqlParameter("@Summary", MySqlDbType.Text),
					new MySqlParameter("@TypeId", MySqlDbType.Int32,11),
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
            parameters[3].Value = model.TypeId;
            parameters[4].Value = model.Cover;
            parameters[5].Value = model.CityId;
            parameters[6].Value = model.CityName;
            parameters[7].Value = model.CountyId;
            parameters[8].Value = model.CountyName;
            parameters[9].Value = model.Grade;
            parameters[10].Value = model.ThemeName;
            parameters[11].Value = model.Lat;
            parameters[12].Value = model.Lng;
            parameters[13].Value = model.Intro;
            parameters[14].Value = model.BuyNotie;
            parameters[15].Value = model.Alias;
            parameters[16].Value = model.Traffic;
            parameters[17].Value = model.NearId;
            parameters[18].Value = model.Status;

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

        /// <summary>
        /// 查询所有景点信息
        /// </summary>
        /// <param name="pq"></param>
        /// <returns></returns>
        public List<ViewScenery> QueryAll()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT A.OutSign,A.Supply,B.ItemId,B.Name,B.Address,B.Summary,B.TypeId,B.Cover,B.CityId,B.CityName,");
            strSql.Append("B.CountyId,B.CountyName,B.Grade,B.ThemeName,B.Lat,B.Lng,B.Intro,B.BuyNotie,B.Alias,B.Traffic,B.NearId,B.Status from common_scenery_outsign as A ");
            strSql.Append("LEFT JOIN common_scenery as B ON A.SceneryId=B.ItemId WHERE B.Status=0");
            return DALCore.GetSMDB().Query(strSql.ToString()).Tables[0].ToList<ViewScenery>();
        }

        public DataTable GetIds()
        {
            string sql = "SELECT OutSign FROM common_scenery_outsign where OutSign not in (42,70,79,86,93,102,104,111,112,122,142,159,163,178,181,186,191,194,206,212)";
            return DALCore.GetSMDB().Query(sql).Tables[0];
        }

        public bool Update(SinoTrip.Entity.DataBase.Common.common_scenery model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update common_scenery set ");
            strSql.Append("Name=@Name,");
            strSql.Append("Address=@Address,");
            strSql.Append("Summary=@Summary,");
            strSql.Append("TypeId=@TypeId,");
            strSql.Append("Cover=@Cover,");
            strSql.Append("CityId=@CityId,");
            strSql.Append("CityName=@CityName,");
            strSql.Append("CountyId=@CountyId,");
            strSql.Append("CountyName=@CountyName,");
            strSql.Append("Grade=@Grade,");
            strSql.Append("ThemeName=@ThemeName,");
            strSql.Append("Lat=@Lat,");
            strSql.Append("Lng=@Lng,");
            strSql.Append("Intro=@Intro,");
            strSql.Append("BuyNotie=@BuyNotie,");
            strSql.Append("Alias=@Alias,");
            strSql.Append("Traffic=@Traffic,");
            strSql.Append("NearId=@NearId,");
            strSql.Append("Status=@Status");
            strSql.Append(" where ItemId=@ItemId");
            MySqlParameter[] parameters = {
					new MySqlParameter("@Name", MySqlDbType.VarChar,255),
					new MySqlParameter("@Address", MySqlDbType.Text),
					new MySqlParameter("@Summary", MySqlDbType.Text),
					new MySqlParameter("@TypeId", MySqlDbType.Int32,11),
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
					new MySqlParameter("@Status", MySqlDbType.Int32,11),
					new MySqlParameter("@ItemId", MySqlDbType.Int32,11)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Address;
            parameters[2].Value = model.Summary;
            parameters[3].Value = model.TypeId;
            parameters[4].Value = model.Cover;
            parameters[5].Value = model.CityId;
            parameters[6].Value = model.CityName;
            parameters[7].Value = model.CountyId;
            parameters[8].Value = model.CountyName;
            parameters[9].Value = model.Grade;
            parameters[10].Value = model.ThemeName;
            parameters[11].Value = model.Lat;
            parameters[12].Value = model.Lng;
            parameters[13].Value = model.Intro;
            parameters[14].Value = model.BuyNotie;
            parameters[15].Value = model.Alias;
            parameters[16].Value = model.Traffic;
            parameters[17].Value = model.NearId;
            parameters[18].Value = model.Status;
            parameters[19].Value = model.ItemId;

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

        public bool UpdateIntro(SinoTrip.Entity.DataBase.Common.common_scenery model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update common_scenery set ");
            strSql.Append("Intro=@Intro,");
            strSql.Append("BuyNotie=@BuyNotie,");
            strSql.Append("Traffic=@Traffic");
            strSql.Append(" where ItemId=@ItemId");
            MySqlParameter[] parameters = {
					new MySqlParameter("@Intro", MySqlDbType.Text),
					new MySqlParameter("@BuyNotie", MySqlDbType.Text),
                    new MySqlParameter("@Traffic", MySqlDbType.Text),
					new MySqlParameter("@ItemId", MySqlDbType.Int32,11)};
            parameters[0].Value = model.Intro;
            parameters[1].Value = model.BuyNotie;
            parameters[2].Value = model.Traffic;
            parameters[3].Value = model.ItemId;

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
