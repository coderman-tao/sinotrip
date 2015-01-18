using SinoTrip.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SinoTrip.API.LY.SDK
{
    public class ApiCommon
    {

        public static string GetResult(string bodyXml, string ServiceName, string url)
        {

            Hashtable ht = new Hashtable();                                 //将参数放入Hashtable中，便于操作
            ht.Add("Version", "20111128102912");                            //接口协议版本号，详见接口协议文档
            ht.Add("AccountID", "4bc54d24-a31a-4b1b-a19b-98e32ac35e3e");                                        //API帐户ID，待申请审批通过后发放
            ht.Add("AccountKey", "caf9126d19922aa3");                                       //API帐户密钥，待申请审批通过后发放
            ht.Add("ServiceName", ServiceName);                          //调用接口的方法名称
            ht.Add("ReqTime", DateTime.Now);                                //当前日期时间

            StringBuilder sbRequest = new StringBuilder();
            sbRequest.Append("<request>");
            sbRequest.Append("<header>");
            sbRequest.AppendFormat("<version>{0}</version>", ht["Version"]);
            sbRequest.AppendFormat("<accountID>{0}</accountID>", ht["AccountID"]);
            sbRequest.AppendFormat("<serviceName>{0}</serviceName>", ht["ServiceName"]);
            sbRequest.AppendFormat("<digitalSign>{0}</digitalSign>", CreateDigitalSign(ht));
            sbRequest.AppendFormat("<reqTime>{0}</reqTime>", ((DateTime)ht["ReqTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            sbRequest.Append("</header>");
            sbRequest.Append("<body>");
            sbRequest.Append(bodyXml);
            sbRequest.Append("</body>");
            sbRequest.Append("</request>");
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http:", StringComparison.CurrentCultureIgnoreCase))
            {
                string strResponse = PostDataToServer(url, sbRequest.ToString(), "POST");
                return strResponse;

            }
            return "";
        }

        #region 创建数字签名

        /// <summary>
        /// 创建数字签名
        /// </summary>
        /// <param name="htValidationParams">存放数字签名参数的Hashtable</param>
        /// <returns>DigitalSign</returns>
        public static string CreateDigitalSign(Hashtable htValidationParams)
        {
            if (!htValidationParams.Contains("AccountKey")
                || String.IsNullOrWhiteSpace(htValidationParams["AccountKey"].ToString()))
            {
                throw new ApplicationException("缺少API帐户密钥");
            }

            string accountKey = htValidationParams["AccountKey"].ToString().Trim();   //API帐户密钥

            List<string> stringList = new List<string>();
            if (htValidationParams.Contains("Version"))
                stringList.Add(string.Format("Version={0}", htValidationParams["Version"].ToString()));
            if (htValidationParams.Contains("AccountID"))
                stringList.Add(string.Format("AccountID={0}", Guid.Parse(htValidationParams["AccountID"].ToString())));
            if (htValidationParams.Contains("ServiceName"))
                stringList.Add(string.Format("ServiceName={0}", htValidationParams["ServiceName"].ToString()));
            if (htValidationParams.Contains("ReqTime")
                && htValidationParams["ReqTime"] != null)
                stringList.Add(string.Format("ReqTime={0}", ((DateTime)htValidationParams["ReqTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff")));

            string[] originalArray = stringList.ToArray();
            string[] sortedArray = BubbleSort(originalArray);
            string digitalSing = GetMD5ByArray(sortedArray, accountKey, "utf-8");

            return digitalSing;
        }

        #endregion

        #region 排序

        /// <summary>
        /// 数组排序（冒泡排序法）
        /// </summary>
        /// <param name="originalArray">待排序字符串数组</param>
        /// <returns>经过冒泡排序过的字符串数组</returns>
        public static string[] BubbleSort(string[] originalArray)
        {
            int i, j; //交换标志 
            string temp;
            bool exchange;

            for (i = 0; i < originalArray.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = originalArray.Length - 2; j >= i; j--)
                {
                    if (String.CompareOrdinal(originalArray[j + 1], originalArray[j]) < 0)　//交换条件
                    {
                        temp = originalArray[j + 1];
                        originalArray[j + 1] = originalArray[j];
                        originalArray[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
            return originalArray;
        }

        #endregion

        #region MD5加密

        /// <summary>
        /// 获取字符数组的MD5哈希值
        /// </summary>
        /// <param name="sortedArray">待计算MD5哈希值的输入字符数组</param>
        /// <param name="key">密钥</param>
        /// <param name="charset">输入字符串的字符集</param>
        /// <returns>输入字符数组的MD5哈希值</returns>
        public static string GetMD5ByArray(string[] sortedArray, string key, string charset)
        {
            //构造待md5摘要字符串
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < sortedArray.Length; i++)
            {
                if (i == sortedArray.Length - 1)
                {
                    builder.Append(sortedArray[i]);
                }
                else
                {
                    builder.Append(sortedArray[i] + "&");
                }
            }
            builder.Append(key);
            return GetMD5(builder.ToString(), charset);
        }

        /// <summary>
        /// MD5 哈希运算
        /// </summary>
        /// <param name="input">待计算MD5哈希值的输入字符串</param>
        /// <param name="charset">输入字符串的字符集</param>
        /// <returns>输入字符串的MD5哈希值</returns>
        public static string GetMD5(string input, string charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(Encoding.GetEncoding(charset).GetBytes(input));
            StringBuilder builder = new StringBuilder(32);
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }
            return builder.ToString();
        }

        #endregion

        /// <summary>
        /// 向服务器提交XML数据
        /// </summary>
        /// <param name="url">远程访问的地址</param>
        /// <param name="data">参数</param>
        /// <param name="method">Http页面请求方法</param>
        /// <returns>远程页面调用结果</returns>
        public static string PostDataToServer(string url, string data, string method)
        {
            HttpWebRequest request = null;

            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                //request.Timeout = 3000;

                switch (method)
                {
                    case "GET":
                        request.Method = "GET";
                        break;
                    case "POST":
                        {
                            request.Method = "POST";

                            byte[] bdata = Encoding.UTF8.GetBytes(data);
                            request.ContentType = "application/xml;charset=utf-8";
                            request.ContentLength = bdata.Length;

                            Stream streamOut = request.GetRequestStream();
                            streamOut.Write(bdata, 0, bdata.Length);
                            streamOut.Close();
                        }
                        break;
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamIn = response.GetResponseStream();

                StreamReader reader = new StreamReader(streamIn);
                string result = reader.ReadToEnd();
                reader.Close();
                streamIn.Close();
                response.Close();

                return result;
            }
            catch (Exception ex)
            {
                LoggerCore.Error(ex.Message, ex);
                return string.Empty;
            }
            finally
            {
            }
        }
    }
}