using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SinoTrip.FrameWork.Web
{
    public class GetIP
    {
        public static string IPAddress
        {
            get
            {
                string userIP;
                // HttpRequest Request = HttpContext.Current.Request;  
                HttpRequest Request = HttpContext.Current.Request; // ForumContext.Current.Context.Request;  
                // 如果使用代理，获取真实IP  
                if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                    userIP = Request.ServerVariables["REMOTE_ADDR"];
                else
                    userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (userIP == null || userIP == "")
                    userIP = Request.UserHostAddress;
                return userIP;
            }
        }  
    }
}
