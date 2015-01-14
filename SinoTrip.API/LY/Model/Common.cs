using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.LY.Model
{
    public class QueryScenery
    {
        public string clientIp = "127.0.0.1";
        public int? cityId;
        public int? page;
        public int? pageSize;
        public int? provinceId;
        public int? countryId;
        public int? gradeId;
        public int? themeId;
        /// <summary>
        /// 0-	点评由多到少
        ///1-	景区级别
        ///2-	同程推荐
        ///3-	人气指数
        ///4-	按距离升序
        ///默认按同程推荐排序
        /// </summary>
        public string sortType;
        /// <summary>
        /// 当有keyword时必传入，多个用英文逗号隔开
        ///如：
        ///field1,field2
        ///有以下字段:
        ///•	city 城市
        ///•	name
        ///•	nameAlias 别名
        ///•	namePYLC 景区拼音(全拼)
        ///•	nameTKPY  景区名分词全拼
        ///•	nameTKFPY 景区名分词首字母
        ///•	summary
        ///•	themeName 主题
        ///未指定的话按默认定的字段搜索
        /// </summary>
        public string searchFields = "city,name,nameAlias,namePYLC,nameTKPY,nameTKFPY,summary,themeName";
        /// <summary>
        /// 如0,50，表示0到50
        /// </summary>
        public string priceRange;
        /// <summary>
        /// 1.mapbar 2.百度；3.谷歌不传默认为1
        /// </summary>
        public int? cs;
    }
}
