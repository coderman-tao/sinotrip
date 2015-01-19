using MySql.Data.MySqlClient;
using SinoTrip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SinoTrip.FrameWork.Common;

namespace SinoTrip.DAL.Scenery
{
    public class QueryDZPost
    {
        public string GetPostByTag(string tag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from cartrip_forum_post where tags like @tag limit 10");
            MySqlParameter[] parameters = {
					new MySqlParameter("@tag", MySqlDbType.VarChar,255)};
            parameters[0].Value = "%" + tag + "%";
            var data = DALCore.GetDZDB().Query(strSql.ToString(), parameters).Tables[0];
            return data.ToJson();
        }
    }
}
