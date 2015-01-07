using SinoTrip.API.Ctrip.Model.Hotel.Request;
using SinoTrip.API.Ctrip.SDK;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SinoTrip.FrameWork.Common;

namespace SinoTrip.WebView
{
    public partial class Default : System.Web.UI.Page
    {
        private static readonly string SID = ConfigurationManager.AppSettings["ctripWebID"];
        private static readonly string UID = ConfigurationManager.AppSettings["ctripID"];
        private static readonly string KEY = ConfigurationManager.AppSettings["ctripKey"];
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string rs = new SinoTrip.API.LY.Biz.ScenicBiz().GetProvinceList();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rs);
            var str = "<provinceList>" + doc.SelectSingleNode("response/body/provinceList").InnerXml + "</provinceList>";
            var model = str.XmlToEntity<SinoTrip.API.LY.Model.provinceList>();
            foreach (var item in model.Province)
            {
                sb.Append("INSERT INTO common_area(Parent,Name,English,ABCD,Pinyin,Data) Values(59,'" + item.name + "','" + item.enName + "','" + item.prefixLetter + "','" + item.enName + "'," + item.id + ");");
            }
            Response.Clear();
            Response.Write(sb.ToString());
            Response.ContentType = "text/xml";
            Response.End();
        }

        void CtripJQ()
        {
            Encoding encoding = System.Text.Encoding.GetEncoding("GBK");
            string f = System.Web.HttpUtility.HtmlDecode("%7b%22AllianceID%22%3a%221%22%2c%22SID%22%3a%221%22%2c%22ProtocolType%22%3a1%2c%22Signature%22%3a%22817D9FF70999176DE1F887D5CC4B732B%22%2c%22TimeStamp%22%3a%221371177110%22%2c%22Channel%22%3a%22Vacations%22%2c%22Interface%22%3a%22AddressSelectorInfoSearch%22%2c%22IsError%22%3afalse%2c%22RequestBody%22%3a%22%7b%5c%22AddressSelectorIDs%5c%22%3a%5b1%2c2%5d%7d%22%2c%22ResponseBody%22%3a%22%22%2c%22ErrorMessage%22%3a%22%22%7d");
            var remote = ConfigurationManager.AppSettings["ctrip_Api"] + "/vacations/OpenServer.ashx";
            string timeStamp;
            string signature;
            GetSignature(SID, UID, KEY, out timeStamp, out signature, "TicketSenicSpotSearch");
            var reqStr = @"<ScenicSpotSearchRequest><DistributionChannel>9</DistributionChannel><PagingParameter><PageIndex>1</PageIndex><PageSize>10</PageSize></PagingParameter><SearchParameter><Keyword>上海</Keyword><SaleCityID>2</SaleCityID></SearchParameter></ScenicSpotSearchRequest>";
            string json = "{\"AllianceID\":\"" + UID + "\",\"SID\":\"" + SID + "\",\"ProtocolType\":0,\"Signature\":\"" + signature + "\",\"TimeStamp\":\"" + timeStamp + "\",\"Channel\":\"Vacations\",\"Interface\":\"TicketSenicSpotSearch\",\"IsError\":false,\"RequestBody\": \"" + reqStr + "\",\"ResponseBody\":\"\",\"ErrorMessage\":\"\"}";
            var postData = System.Web.HttpUtility.UrlEncode(json, encoding);
            remote = remote + "?RequestJson=" + postData;




            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest request = HttpWebRequest.Create(remote) as HttpWebRequest;
            request.Method = "Get";
            //request.ContentLength = data.Length;
            //request.ContentType = "text/xml; charset=gb2312";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var strem = response.GetResponseStream();
            StreamReader sr = new StreamReader(strem, Encoding.UTF8);
            string ret = sr.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ret);
            var hotelNodes = xmlDoc.SelectSingleNode("/APIProtocolPackage/ResponseBody").InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(hotelNodes);
            var lo = xmlDoc1.SelectNodes("/ScenicSpotSearchResponse/ScenicSpotListItemList/ScenicSpotListItemDTO");
            sr.Close();
            response.Close();
        }
        protected void GetSignature(string sid, string allianceID, string secretKey, out string timeStamp, out string signature, string requestType)
        {
            //string allianceID = "1";
            //string SID = "50";
            //string SecretKey = "abcDFG645354";
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            timeStamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            string MD5SharedSecret = FormsAuthentication.HashPasswordForStoringInConfigFile(secretKey, "MD5");
            signature = FormsAuthentication.HashPasswordForStoringInConfigFile(timeStamp + allianceID + MD5SharedSecret + sid + requestType, "MD5");
        }

        [Serializable]
        public class Citys
        {
        }
        public class CityDetail
        {
            //    <CityCode>YTY</CityCode>
            //<City>15</City>
            //<CityName>扬州</CityName>
            //<CityEName>Yangzhou</CityEName>
            //<Country>1</Country>
            //<Province>15</Province>
            //<Airport />
        }

        
    }
}