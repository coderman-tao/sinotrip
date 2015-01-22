using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.FrameWork.Web;
using SinoTrip.FrameWork.Common;
using SinoTrip.API;
using System.Net;
using System.IO;
using System.Text;

namespace SinoTrip.WebView.Scenic
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ip = "218.22.12.154";
            var url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip;
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "Get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var strem = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(strem, Encoding.UTF8);
                string ret = sr.ReadToEnd();
                var ipmodel = ret.JsonDeserialize<SinoTrip.API.Taobao.Model.Ip>();
                if (ipmodel.code == "1")
                {

                }
                else
                {
 
                }
                sr.Close();
            }
        }
    }
}