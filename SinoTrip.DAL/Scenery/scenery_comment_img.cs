using SinoTrip.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.DAL.Scenery
{
    public class scenery_comment_img
    {
        public bool Add(SinoTrip.Entity.DataBase.Scenery.scenery_comment_img model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into scenery_comment_img(");
            strSql.Append("CommentId,SceneryId,ImgUrl,Status,TempData)");
            strSql.Append(" values (");
            strSql.Append("@CommentId,@SceneryId,@ImgUrl,@Status,@TempData)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@CommentId", MySqlDbType.Int32,11),
					new MySqlParameter("@SceneryId", MySqlDbType.Int32,11),
					new MySqlParameter("@ImgUrl", MySqlDbType.Text),
					new MySqlParameter("@Status", MySqlDbType.Int32,11),
					new MySqlParameter("@TempData", MySqlDbType.VarChar,255)};
            parameters[0].Value = model.CommentId;
            parameters[1].Value = model.SceneryId;
            parameters[2].Value = model.ImgUrl;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.TempData;

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
