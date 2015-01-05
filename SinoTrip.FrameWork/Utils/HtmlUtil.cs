using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SinoTrip.FrameWork.Utils
{
    public static class HtmlUtil
    {

        #region 清理HTML标签

        /// <summary>
        /// 清除所有
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearAll(string str)
        {
            if (str == null || str == string.Empty)
                return string.Empty;

            str = ClearWordStyle(str);

            str = ClearElement(str);

            string regexstr = @"<[^>]+>|</[^>]+>";

            str = Regex.Replace(str, regexstr, "", RegexOptions.IgnoreCase);

            return str;
        }



        /// <summary>
        /// 清理HTML标签的多余样式；如<div style="color:#454353">示例</div>;换成<div>示例</div>
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="element">要清除的标签</param>
        /// <returns></returns>
        public static string ClearElement(string str, string element)
        {
            if (str == null || str == string.Empty)
                return string.Empty;
            str = str.Replace("\n", "").Replace("\r", "");
            string old = @"<" + element + "[^>]+>";
            string rep = "<" + element + ">";
            // Regex re = new Regex(@"class\s*=(['""\s]?)[^'""]*?\1|style\s*=(['""\s]?)[^'""]*?\1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //<([a-z]+?)\s+?.*?>
            Regex re = new Regex(@"<([a-z]+?)\s+?.*?>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(element))
                str = Regex.Replace(str, old, rep, RegexOptions.IgnoreCase);
            else
                str = re.Replace(str, "<$1>");

            return str;
        }

        public static string ClearElement(string str)
        {
            str = ClearElement(str, string.Empty);
            return str;
        }


        /// <summary>
        /// 清除HTML标签；如<div style="color:#454353">示例</div>;换成：示例
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="element">要清除的标签</param>
        /// <returns></returns>
        public static string ReMoveElement(string str, string element, bool isall)
        {
            if (str == null || str == string.Empty)
                return string.Empty;

            string regFront = string.Empty;
            if (isall)
                regFront = @"<" + element + "[^>]*>";
            else
                regFront = @"<" + element + ">";
            string regAfter = "</" + element + ">";
            str = Regex.Replace(str, regFront, "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, regAfter, "", RegexOptions.IgnoreCase);
            return str;
        }



        /// <summary>
        /// 清理指定字符串，大小写不敏感
        /// </summary>
        /// <param name="strText">原始文本</param>
        /// <param name="strOld">要替换的字符串，支持正则表达式，大小写不敏感</param>
        /// <param name="strNew">替换后的字符串</param>
        /// <returns></returns>
        public static string RegexReplace(string strText, string strOld, string strNew)
        {
            if (strText == null || strText == string.Empty)
                return string.Empty;

            strText = Regex.Replace(strText, strOld, strNew, RegexOptions.IgnoreCase);
            return strText;
        }
        /// <summary>
        /// 清理Word的样式，主要是一些带冒号的标签，如o:p
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string ClearWordStyle(string strText)
        {
            if (strText == null || strText == string.Empty)
                return string.Empty;

            string regFront = @"<\w+:[^>]*>";
            string regAfter = @"</\w+:[^>]*>";
            string regword = @"<!--([\S\s]*?)-->";
            strText = Regex.Replace(strText, regFront, "", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, regAfter, "", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, regword, "", RegexOptions.IgnoreCase);
            return strText;

        }




















        /// <summary>
        /// 清除style样式，保留指定的标签
        /// </summary>
        /// <param name="str"></param>
        /// <param name="el">指定保留的标签</param>
        /// <returns></returns>
        public static string HtmlClear(string str, string[] el)
        {
            var elstr = string.Empty;
            str = ClearWordStyle(str);	//清除word样式
            str = ClearElement(str); //清除style样式
            for (var i = 0; i < el.Length; i++)
            {

                elstr = elstr + "/?" + el[i];
                if (i < el.Length - 1)
                    elstr += "|";
            }

            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(elstr))
                return str;
            elstr = @"</?(?!br|" + elstr + ")[^>]*>";

            Regex regEx = new Regex(elstr, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            str = regEx.Replace(str, "");
            str = str.Replace(" ", "");
            return str;
        }

        /// <summary>
        /// 清除word格式与指定的样式与标签;
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="clearEl">需要清除掉样式的标签，clearEl为null时默认清除"p", "div", "table", "tr", "td", "ul", "li" 标签的样式</param>
        /// <param name="moveEl">需要清除掉的标签，moveEl为null时默认清除"b","i","u","span", "strong", "font", "h1", "tbody", "ul","a"</param>
        /// <returns></returns>
        public static string HtmlClear(string str, string[] clearEl, string[] moveEl)
        {
            if (str == "" || str == null || string.IsNullOrEmpty(str))
                return "";
            //清理word标签，如o:p之类，带冒号的
            str = ClearWordStyle(str);
            //清理样式	
            string[] el = new string[] { };

            if (clearEl != null)
                el = new string[] { "p", "div", "table", "tr", "td", "ul", "li" };
            else
                el = clearEl;
            foreach (string s in el)
            {
                try
                {
                    str = ClearElement(str, s);
                }
                catch
                {
                    continue;
                }
            }
            //清除样式
            if (moveEl != null)
                el = moveEl;
            else
                el = new string[] { "b", "i", "u", "span", "strong", "font", "h1", "tbody", "ul", "a" };
            foreach (string s in el)
            {
                try
                {
                    str = ReMoveElement(str, s, false);

                }
                catch
                {
                    continue;
                }
            }
            str = RegexReplace(str, " ", "");
            str = str.Replace(" ", "");
            return str;
        }
        #endregion

        /// <summary>
        ///  Html转换成Textarea输入框格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlToTextarea(this string str)
        {
            str = RegexReplace(str, "<b>", "");
            str = RegexReplace(str, "</b>", "");
            str = RegexReplace(str, "<br>", "\r\n");
            str = RegexReplace(str, "</br>", "\r\n");
            str = RegexReplace(str, "<p>", "");
            str = RegexReplace(str, "</p>", "\r\n");
            str = RegexReplace(str, "<div>", "");
            str = RegexReplace(str, "</div>", "\r\n");
            str = RegexReplace(str, "<li>", "");
            str = RegexReplace(str, "</li>", "\r\n");
            str = RegexReplace(str, "&nbsp;", " ");
            return str;
        }
        /// <summary>
        /// Textarea回车转成html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TextareaToHtml(this string str)
        {
            str = RegexReplace(str, "\r\n", "<br>");
            str = RegexReplace(str, "\n", "<br>");
            return str;
        }




    }
}
