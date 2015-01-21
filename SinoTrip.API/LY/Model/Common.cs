using SinoTrip.FrameWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SinoTrip.API.LY.Model
{
    public class QueryScenery
    {
        public string clientIp { get { return "127.0.0.1"; } set { } }
        public int? cityId { get; set; }
        public int? page { get; set; }
        public int? pageSize { get; set; }
        public int? provinceId { get; set; }
        public int? countryId { get; set; }
        public int? gradeId { get; set; }
        public int? themeId { get; set; }

        public string keyword { get; set; }
        /// <summary>
        /// 0-	点评由多到少
        ///1-	景区级别
        ///2-	同程推荐
        ///3-	人气指数
        ///4-	按距离升序
        ///默认按同程推荐排序
        /// </summary>
        public string sortType { get; set; }
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
        public string searchFields { get { return "city,name,nameAlias,namePYLC,nameTKPY,nameTKFPY,summary,themeName"; } set { } } 
        /// <summary>
        /// 如0,50，表示0到50
        /// </summary>
        public string priceRange { get; set; }
        /// <summary>
        /// 1.mapbar 2.百度；3.谷歌不传默认为1
        /// </summary>
        public int? cs = 2;
    }
    [Serializable, XmlRoot("body")]
    public class imageRs
    {
        public imageList imageList;

        public extInfoOfImageListData extInfoOfImageList;
    }

    [Serializable]
    public class imageList
    {
        [XmlAttribute]
        public int page { get; set; }
        [XmlAttribute]
        public int pageSize { get; set; }
        [XmlAttribute]
        public int totalPage { get; set; }
        [XmlAttribute]
        public int totalCount { get; set; }
        [XmlElement("image")]
        public List<image> images { get; set; }

        [Serializable]
        public class image
        {
            public CDATA imagePath { set; get; }
        }


    }
    [Serializable]
    public class extInfoOfImageListData
    {
        public string imageBaseUrl { get; set; }

        public List<sizeCode> sizeCodeList { get; set; }
        public class sizeCode
        {
            [XmlAttribute]
            public int size { get; set; }
            [XmlAttribute]
            public string isDefault { get; set; }

            [XmlText]
            public string sizeInfo { get; set; }
        }
    }



}
