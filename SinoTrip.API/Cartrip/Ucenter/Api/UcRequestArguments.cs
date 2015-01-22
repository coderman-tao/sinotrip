using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace SinoTrip.API.Cartrip.Ucenter.Api
{
    /// <summary>
    /// Requser����
    /// Dozer ��Ȩ����
    /// �����ơ��޸ģ����뱣���ҵ���ϵ��ʽ��
    /// http://www.dozer.cc
    /// mailto:dozer.cc@gmail.com
    /// </summary>
    public class UcRequestArguments: IUcRequestArguments
    {
        /// <summary>
        /// Action
        /// </summary>
        public string Action { get;private set; }

        private string Code { get; set; }

        /// <summary>
        /// ʱ��
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// Query����
        /// </summary>
        public NameValueCollection QueryString { get; private set; }
        /// <summary>
        /// Form����
        /// </summary>
        public string FormData { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInvalidRequest { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAuthracationExpiried { get; private set; }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="request">Request</param>
        public UcRequestArguments(HttpRequest request)
        {
            Code = request.QueryString["code"];
            FormData = HttpUtility.UrlDecode(request.Form.ToString(), Encoding.GetEncoding(UcConfig.UcCharset));
            QueryString = HttpUtility.ParseQueryString(UcUtility.AuthCodeDecode(Code));
            Action = QueryString["action"];
            long time;
            if (long.TryParse(QueryString["time"], out time)) Time = time;
            IsInvalidRequest = request.QueryString.Count == 0 && UcActions.Contains(Action);
            IsAuthracationExpiried = (UcUtility.PhpTimeNow() - Time) > 0xe10;
        }
    }
}

