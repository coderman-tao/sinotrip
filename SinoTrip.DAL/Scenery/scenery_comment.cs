using SinoTrip.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.FrameWork.Common;

namespace SinoTrip.DAL.Scenery
{
    public class scenery_comment
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SinoTrip.Entity.DataBase.Scenery.scenery_comment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into scenery_comment(");
            strSql.Append("SceneryId,OutSign,Rank,DPTitle,Comment,Uid,UserName,DPService,DPShiGouYu,DPTraffic,ServiceScore,ServiceGrade,ConvenientScore,ConvenientGrade,DiscountScore,DiscountGrade,DPGanwu,DPTime,TempData,Status)");
            strSql.Append(" values (");
            strSql.Append("@SceneryId,@OutSign,@Rank,@DPTitle,@Comment,@Uid,@UserName,@DPService,@DPShiGouYu,@DPTraffic,@ServiceScore,@ServiceGrade,@ConvenientScore,@ConvenientGrade,@DiscountScore,@DiscountGrade,@DPGanwu,@DPTime,@TempData,@Status)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@SceneryId", MySqlDbType.Int32,11),
					new MySqlParameter("@OutSign", MySqlDbType.Int32,11),
					new MySqlParameter("@Rank", MySqlDbType.Int32,11),
					new MySqlParameter("@DPTitle", MySqlDbType.VarChar,255),
					new MySqlParameter("@Comment", MySqlDbType.Text),
					new MySqlParameter("@Uid", MySqlDbType.Int32,11),
					new MySqlParameter("@UserName", MySqlDbType.VarChar,255),
					new MySqlParameter("@DPService", MySqlDbType.Text),
					new MySqlParameter("@DPShiGouYu", MySqlDbType.Text),
					new MySqlParameter("@DPTraffic", MySqlDbType.Text),
					new MySqlParameter("@ServiceScore", MySqlDbType.Int16,6),
					new MySqlParameter("@ServiceGrade", MySqlDbType.VarChar,50),
					new MySqlParameter("@ConvenientScore", MySqlDbType.Int16,6),
					new MySqlParameter("@ConvenientGrade", MySqlDbType.VarChar,50),
					new MySqlParameter("@DiscountScore", MySqlDbType.Int16,6),
					new MySqlParameter("@DiscountGrade", MySqlDbType.VarChar,50),
					new MySqlParameter("@DPGanwu", MySqlDbType.Text),
					new MySqlParameter("@DPTime", MySqlDbType.Int32,11),
					new MySqlParameter("@TempData", MySqlDbType.VarChar,255),
					new MySqlParameter("@Status", MySqlDbType.Int32,11)};
            parameters[0].Value = model.SceneryId;
            parameters[1].Value = model.OutSign;
            parameters[2].Value = model.Rank;
            parameters[3].Value = model.DPTitle;
            parameters[4].Value = model.Comment;
            parameters[5].Value = model.Uid;
            parameters[6].Value = model.UserName;
            parameters[7].Value = model.DPService;
            parameters[8].Value = model.DPShiGouYu;
            parameters[9].Value = model.DPTraffic;
            parameters[10].Value = model.ServiceScore;
            parameters[11].Value = model.ServiceGrade;
            parameters[12].Value = model.ConvenientScore;
            parameters[13].Value = model.ConvenientGrade;
            parameters[14].Value = model.DiscountScore;
            parameters[15].Value = model.DiscountGrade;
            parameters[16].Value = model.DPGanwu;
            parameters[17].Value = model.DPTime;
            parameters[18].Value = model.TempData;
            parameters[19].Value = model.Status;

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

        public bool BatchAdd(List<SinoTrip.Entity.DataBase.Scenery.scenery_comment> list)
        {
            List<DB.DbMySQL.DbEntity> _list = new List<DB.DbMySQL.DbEntity>();
            foreach (var model in list)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into scenery_comment(");
                strSql.Append("SceneryId,OutSign,Rank,DPTitle,Comment,Uid,UserName,DPService,DPShiGouYu,DPTraffic,ServiceScore,ServiceGrade,ConvenientScore,ConvenientGrade,DiscountScore,DiscountGrade,DPGanwu,DPTime,TempData,Status)");
                strSql.Append(" values (");
                strSql.Append("@SceneryId,@OutSign,@Rank,@DPTitle,@Comment,@Uid,@UserName,@DPService,@DPShiGouYu,@DPTraffic,@ServiceScore,@ServiceGrade,@ConvenientScore,@ConvenientGrade,@DiscountScore,@DiscountGrade,@DPGanwu,@DPTime,@TempData,@Status)");
                MySqlParameter[] parameters = {
					new MySqlParameter("@SceneryId", MySqlDbType.Int32,11),
					new MySqlParameter("@OutSign", MySqlDbType.Int32,11),
					new MySqlParameter("@Rank", MySqlDbType.Int32,11),
					new MySqlParameter("@DPTitle", MySqlDbType.VarChar,255),
					new MySqlParameter("@Comment", MySqlDbType.Text),
					new MySqlParameter("@Uid", MySqlDbType.Int32,11),
					new MySqlParameter("@UserName", MySqlDbType.VarChar,255),
					new MySqlParameter("@DPService", MySqlDbType.Text),
					new MySqlParameter("@DPShiGouYu", MySqlDbType.Text),
					new MySqlParameter("@DPTraffic", MySqlDbType.Text),
					new MySqlParameter("@ServiceScore", MySqlDbType.Int16,6),
					new MySqlParameter("@ServiceGrade", MySqlDbType.VarChar,50),
					new MySqlParameter("@ConvenientScore", MySqlDbType.Int16,6),
					new MySqlParameter("@ConvenientGrade", MySqlDbType.VarChar,50),
					new MySqlParameter("@DiscountScore", MySqlDbType.Int16,6),
					new MySqlParameter("@DiscountGrade", MySqlDbType.VarChar,50),
					new MySqlParameter("@DPGanwu", MySqlDbType.Text),
					new MySqlParameter("@DPTime", MySqlDbType.Int32,11),
					new MySqlParameter("@TempData", MySqlDbType.VarChar,255),
					new MySqlParameter("@Status", MySqlDbType.Int32,11)};
                parameters[0].Value = model.SceneryId;
                parameters[1].Value = model.OutSign;
                parameters[2].Value = model.Rank;
                parameters[3].Value = model.DPTitle;
                parameters[4].Value = model.Comment;
                parameters[5].Value = model.Uid;
                parameters[6].Value = model.UserName;
                parameters[7].Value = model.DPService;
                parameters[8].Value = model.DPShiGouYu;
                parameters[9].Value = model.DPTraffic;
                parameters[10].Value = model.ServiceScore;
                parameters[11].Value = model.ServiceGrade;
                parameters[12].Value = model.ConvenientScore;
                parameters[13].Value = model.ConvenientGrade;
                parameters[14].Value = model.DiscountScore;
                parameters[15].Value = model.DiscountGrade;
                parameters[16].Value = model.DPGanwu;
                parameters[17].Value = model.DPTime;
                parameters[18].Value = model.TempData;
                parameters[19].Value = model.Status;
                _list.Add(new DB.DbMySQL.DbEntity(strSql.ToString(), parameters));
            }
            if (_list.Count > 0)
            {
                try
                {
                    int rows = DALCore.GetSMDB().BatchExcuteSql(_list);
                    if (rows != _list.Count)
                        return false;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
            return false;
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<SinoTrip.Entity.DataBase.Scenery.scenery_comment> GetList(int SceneryId, int page, int pageSize, out int total)
        {
            if (SceneryId <= 0 || pageSize > 1000)
            {
                total = 0;
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemId,SceneryId,OutSign,Rank,DPTitle,Comment,Uid,UserName,DPService,DPShiGouYu,DPTraffic,ServiceScore,ServiceGrade,ConvenientScore,ConvenientGrade,DiscountScore,DiscountGrade,DPGanwu,DPTime,TempData,Status ");
            strSql.Append(" FROM scenery_comment ");
            strSql.Append(" WHERE SceneryId=" + SceneryId);
            return DALCore.SimplePageQuery(strSql.ToString(), page, pageSize, "DPTime", true, out total, DALCore.GetSMDB(), null).ToList<SinoTrip.Entity.DataBase.Scenery.scenery_comment>();
        }
    }
}
