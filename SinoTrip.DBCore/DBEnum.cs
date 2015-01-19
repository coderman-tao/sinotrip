using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTrip.DB
{
    public abstract class DBEnum
    {
        /// <summary>
        /// MySql主库连接
        /// </summary>
        /// <returns></returns>
        public static DbMySQL MySQLMaster()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["cartrip"].ConnectionString;
            return new DbMySQL(connectionString);
        }
        /// <summary>
        /// 论坛数据库
        /// </summary>
        /// <returns></returns>
        public static DbMySQL MySQLDZ()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["cartrip_dz"].ConnectionString;
            return new DbMySQL(connectionString);
        }
    }
}
