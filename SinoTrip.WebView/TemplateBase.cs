using SinoTrip.FrameWork.Web;
using SinoTrip.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using SinoTrip.WebBase;

namespace SinoTrip.WebView
{
    public class TemplateBase : HotelBase
    {
        private string templatePath;
        /// <summary>
        /// 获取当前默认模板类型
        /// </summary>
        public string TemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(templatePath))
                    templatePath = ConfigurationManager.AppSettings["WEB:TemplatePath"].ToStringEx(string.Empty) + ConfigurationManager.AppSettings["WEB:TemplateType"].ToStringEx(string.Empty);
                return templatePath;
            }
        }
        public TemplateBase()
        { }
        public TemplateBase(string path)
        {
            templatePath = Path.Combine(ConfigurationManager.AppSettings["WEB:TemplatePath"].ToStringEx(string.Empty), path);
        }
        private VelocityHelper vt;
        public VelocityHelper Vt
        {
            get
            {
                if (vt == null)
                {
                    vt = new VelocityHelper();
                    vt.Init(TemplatePath);//模板路径 
                    //vt.Put("QU", QU);
                    //vt.Put("now", DateTime.Now.ToUnixInt());
                    //vt.Put("SITE", SITE);
                    //vt.Put("LINK", LINK);
                    // Vt.Put("LoginReturn", Session[SessionKeys.ReturnUrl].ToString(string.Empty));
                    vt.Put("tool", this);
                }
                return vt;


            }

        }
        public string getPageList(string data, string currentClass, int size, int page, int pagesize, int count)
        {

            // return string.Format(a, 1);
            var pageStr = string.Empty;
            var thispage = page + 1;
            int pagecount = count / pagesize;
            if (count % pagesize > 0)
                pagecount++;

            if (pagecount <= 1)
                return string.Empty;

            int startpage = 1;
            int endpage = pagecount;


            if (pagecount > size)
                startpage = thispage - ((size % 2) > 0 ? (size / 2) + 1 : size / 2);


            if (startpage < 1)
                startpage = 1;

            endpage = startpage + size;
            if (endpage > pagecount)
                endpage = pagecount;




            for (var i = startpage; i <= endpage; i++)
            {

                var curren = string.Empty;
                if (i == thispage)
                    curren = currentClass;
                pageStr += string.Format(data, new object[] { curren, i - 1, i });
            }

            if (endpage < pagecount)
            {

                var endpagestr = string.Format(data, new object[] { string.Empty, pagecount - 1, pagecount });

                if (endpage < pagecount - 1)
                    pageStr += ("......" + endpagestr);
                else
                    pageStr += endpagestr;


            }

            if (startpage > 1)
            {

                if (startpage > 2)
                {

                    pageStr = string.Format(data, new object[] { "", 0, 1 }) + "......" + pageStr;
                }
                else
                {
                    pageStr = string.Format(data, new object[] { "", 0, 1 }) + pageStr;

                }

            }


            return pageStr;
        }
    }
}