using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SinoTrip.FrameWork.Common;
using System.Net;
using System.IO;
using System.Text;

namespace SinoTrip.WebView.Ajax.Hotel
{
    /// <summary>
    /// GetHotel 的摘要说明
    /// </summary>
    public class GetHotel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var action = context.Request["method"];
            switch (action)
            {
                case "List":
                    List(context);
                    break;
                case "GetPrice":
                    break;
            }
        }

        void List(HttpContext context)
        {
            var page = context.Request["page"].ToInt32(0);
            var pagesize = context.Request["pagesize"].ToInt32(0);
            var lng = context.Request["lng"];
            var lat = context.Request["lat"];
            var request = (HttpWebRequest)HttpWebRequest.Create("http://open.zhuna.cn/api/gateway.php?method=hotel.search.nearby&agent_id=6921956&agent_md=cde353fc0ea399f7&lng="+lng+"&lat="+lat+"&pg="+page+"&pagesize="+pagesize);//http://open.zhuna.cn/api/gateway.php?method=chain&必选参数&可选参数
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://open.zhuna.cn/api/gateway.php?method=city&agent_id=6921956&agent_md=cde353fc0ea399f7&cityid=");
            request.Method = "Get";
            Stream getStream;
            StreamReader streamReader;
            string getString = "";
            getStream = request.GetResponse().GetResponseStream();
            streamReader = new StreamReader(getStream, Encoding.GetEncoding("utf-8"));
            getString = streamReader.ReadToEnd();
            streamReader.Close();
            getStream.Close();
            context.Response.Write(getString);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}