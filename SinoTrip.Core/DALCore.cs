using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MySql.Data.MySqlClient;
using SinoTrip.DB;
using SinoTrip.FrameWork.Common;


namespace cartrip.Core
{
    public abstract class DALCore
    {
        /// <summary>
        /// 荞面Sqlserver主库连接
        /// </summary>
        /// <returns></returns>
        public static DbMySQL GetSMDB()
        {
            return DBEnum.MySQLMaster();
        }

        /// <summary>
        /// 查询列表并返回总数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="start">分页开始索引</param>
        /// <param name="limit">分页结束索引</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isdesc">降序true,升序false</param>
        /// <param name="Total">总数</param>
        /// <returns></returns>
        protected static DataTable SimplePageQuery(string sql, int start, int limit, string orderBy, bool isdesc, out int Total, DbMySQL db, MySqlParameter[] parames)
        {
            if (string.IsNullOrEmpty(sql))
            {
                Total = -1;
                return null;
            }
            if ((start <= 0 && limit <= 0))
            {
                Total = -1;
                return null;
            }
            //select * from content order by id desc limit 10000, 10
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSqlCount = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderBy))
            {
                strSql.Append("order by T." + orderBy);
                if (isdesc)
                    strSql.Append(" DESC");
            }
            //else
            //{
            //    strSql.Append("order by T.id");
            //    if (isdesc)
            //        strSql.Append(" DESC");
            //}
            strSql.Append(")AS Row, T.*  from (");
            strSql.Append(sql);
            strSql.Append(")T ) TT");
            strSql.AppendFormat(" WHERE TT.Row> {0} and  TT.Row<={1}", start, start + limit);
            strSqlCount.Append("select COUNT(*) FROM (" + sql + ") as t");
            if (db == null)
            {
                db = GetSMDB();
            }
            DataSet ds = db.Query(strSql.ToString() + ";" + strSqlCount.ToString(), parames);
            try
            {
                Total = ds.Tables[1].Rows[0][0].ToInt32(0);
                return ds.Tables[0];

            }
            catch (Exception)
            {
                Total = -1;
                return null;
            }
        }
    }
}
