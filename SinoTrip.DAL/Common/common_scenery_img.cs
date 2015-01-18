using MySql.Data.MySqlClient;
using SinoTrip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.FrameWork.Common;
namespace SinoTrip.DAL.Common
{
    public class common_scenery_img
    {
        public bool Add(SinoTrip.Entity.DataBase.Common.common_scenery_img model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into common_scenery_img(");
            strSql.Append("SceneryId,Width,Height,FileSize,FileName,ExName,Folder,Cover,Status,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@SceneryId,@Width,@Height,@FileSize,@FileName,@ExName,@Folder,@Cover,@Status,@CreateTime)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@SceneryId", MySqlDbType.Int32,11),
					new MySqlParameter("@Width", MySqlDbType.Int32,11),
					new MySqlParameter("@Height", MySqlDbType.Int32,11),
					new MySqlParameter("@FileSize", MySqlDbType.Int32,11),
					new MySqlParameter("@FileName", MySqlDbType.VarChar,255),
					new MySqlParameter("@ExName", MySqlDbType.VarChar,10),
					new MySqlParameter("@Folder", MySqlDbType.VarChar,1000),
					new MySqlParameter("@Cover", MySqlDbType.VarChar,1000),
					new MySqlParameter("@Status", MySqlDbType.Int24,6),
					new MySqlParameter("@CreateTime", MySqlDbType.Int32,11)};
            parameters[0].Value = model.SceneryId;
            parameters[1].Value = model.Width;
            parameters[2].Value = model.Height;
            parameters[3].Value = model.FileSize;
            parameters[4].Value = model.FileName;
            parameters[5].Value = model.ExName;
            parameters[6].Value = model.Folder;
            parameters[7].Value = model.Cover;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateTime;
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
        /// 获得数据列表
        /// </summary>
        public List<SinoTrip.Entity.DataBase.Common.common_scenery_img> GetList(int SceneryId, int top)
        {
            if (SceneryId <= 0)
                return null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemId,SceneryId,Width,Height,FileSize,FileName,ExName,Folder,Cover,Status,CreateTime ");
            strSql.Append(" FROM common_scenery_img WHERE STATUS=0 AND SceneryId=" + SceneryId + " Order By CreateTime Desc Limit " + top);
            return DALCore.GetSMDB().Query(strSql.ToString()).Tables[0].ToList<SinoTrip.Entity.DataBase.Common.common_scenery_img>();
        }
    }
}
