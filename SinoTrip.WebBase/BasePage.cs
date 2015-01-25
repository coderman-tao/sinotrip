using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using SinoTrip.FrameWork.Common;
using SinoTrip.Core;

namespace SinoTrip.WebBase
{
    public class BasePage : Page
    {
        public class PageUser
        {
            public int id { get; set; }
            public int erpid { get; set; }
            public string username { get; set; }
            public string mobile { get; set; }
            public string email { get; set; }
            public int sex { get; set; }
            public string avatar { get; set; }
            public string realname { get; set; }
            public string nickname { get; set; }
            public string idcard { get; set; }
            public DateTime lasttime { get; set; }
        }
        private PageUser qU;

        public PageUser QU
        {
            get
            {
                if (qU == null)
                {
                    // var _s = Session;
                    qU = new PageUser();
                    qU.id = Session["uid"].ToInt32(0);
                    LoggerCore.Debug(qU.id.ToString()+"---"+Session["username"]);
                    qU.username = Session["username"].ToStringEx(string.Empty);
                    qU.mobile = Session["mobile"].ToStringEx(string.Empty);
                    qU.email = Session["email"].ToStringEx(string.Empty);
                    qU.avatar = Session["avatar"].ToStringEx(string.Empty);
                    qU.realname = Session["realname"].ToStringEx(string.Empty);
                    qU.nickname = Session["nickname"].ToStringEx(string.Empty);
  
                }

                return qU;
            }

        }
    }
}
