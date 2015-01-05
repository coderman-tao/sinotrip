using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.FrameWork.Web
{
    public class AjaxMsg
    {
        /// <summary>
        /// Method Not Allowed
        /// </summary>
        public static string MSG_405 = "权限不足，无法访问";

        public static string MSG_500 = "出现错误";

        /// <summary>
        /// AJAX标准返回成功json
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string MSG_SUCCESS(string msg)
        {
            return "{success:true,msg:'" + msg + "'}";
        }

        /// <summary>
        /// AJAX标准返回失败json
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string MSG_ERROR(string msg)
        {
            return "{success:false,msg:'" + msg + "'}";
        }

        public static string MSG_RESULT(string msg, string json)
        {
            return "{success:true,msg:\"" + msg + "\",result:" + json + "}";
        }

        /// <summary>
        /// AJAX标准返回失败json
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string MSG_ERROR(string msg, int status)
        {
            return "{success:false,msg:\"" + msg + "\",code:" + status + "}";
        }
        /// <summary>
        /// AJAX标准返回失败json
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string MSG_ERROR(string msg, string code, int status)
        {
            return "{success:false,msg:\"" + msg + "\",code:\"" + code + "\",status:" + status + "}";
        }
    }
}
