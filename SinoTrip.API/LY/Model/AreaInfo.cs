using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SinoTrip.API.LY.Model
{
    [Serializable]
    public class provinceList
    {
        [XmlElement("province")]
        public List<AreaInfo> Province { get; set; }
    }
    [Serializable]
    public class AreaInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string enName { get; set; }
        public string prefixLetter { get; set; }
        //        <id>2</id>
        //<name>安徽</name>
        //<enName>AnHui</enName>
        //<prefixLetter>A</prefixLetter>
    }
    [Serializable]
    public class cityList
    {
        [XmlAttribute]
        public int totalCount { get; set; }
        [XmlElement("city")]
        public List<AreaInfo> Citys { get; set; }
    }
     [Serializable]
    public class countyList
    {
        [XmlAttribute]
        public int totalCount { get; set; }
        [XmlElement("county")]
        public List<AreaInfo> Citys { get; set; }
    }

    //<sceneryList page="1" pageSize="100" totalPage="1" totalCount="19" imgbaseURL="http://upload.17u.com/uploadfile/scenerypic_common/134_100/">
    [Serializable]
    public class sceneryList
    {
        [XmlAttribute]
        public int page { get; set; }
        [XmlAttribute]
        public int pageSize { get; set; }
        [XmlAttribute]
        public int totalPage { get; set; }
        [XmlAttribute]
        public int totalCount { get; set; }
        [XmlAttribute]
        public string imgbaseURL { get; set; }
        [XmlElement("scenery")]
        public List<scenery> Scenerys { get; set; }
    }
    [Serializable]
    public class scenery
    {
        #region 属性

        /// <summary> 
        /// sceneryName
        /// </summary> 
        public string sceneryName
        {
            get;
            set;
        }
        /// <summary> 
        /// imgPath
        /// </summary> 
        public string imgPath
        {
            get;
            set;
        }
        /// <summary> 
        /// sceneryId
        /// </summary> 
        ///
        public string sceneryId
        {
            get;
            set;
        }
        /// <summary> 
        /// sceneryAddress
        /// </summary> 
        public string sceneryAddress
        {
            get;
            set;
        }
        /// <summary> 
        /// scenerySummary
        /// </summary> 
        public string scenerySummary
        {
            get;
            set;
        }
        /// <summary> 
        /// provinceId
        /// </summary> 
        public string provinceId
        {
            get;
            set;
        }
        /// <summary> 
        /// provinceName
        /// </summary> 
        public string provinceName
        {
            get;
            set;
        }
        /// <summary> 
        /// cityId
        /// </summary> 
        public string cityId
        {
            get;
            set;
        }
        /// <summary> 
        /// cityName
        /// </summary> 
        public string cityName
        {
            get;
            set;
        }
        /// <summary> 
        /// countyId
        /// </summary> 
        public string countyId
        {
            get;
            set;
        }
        /// <summary> 
        /// countyName
        /// </summary> 
        public string countyName
        {
            get;
            set;
        }
        /// <summary> 
        /// 点评数
        /// </summary> 
        public string commentCount
        {
            get;
            set;
        }
        /// <summary> 
        ///问答数
        /// </summary> 
        public string questionCount
        {
            get;
            set;
        }
        /// <summary> 
        /// 博客数量
        /// </summary> 
        public string blogCount
        {
            get;
            set;
        }
        /// <summary> 
        /// 浏览次数
        /// </summary> 
        public string viewCount
        {
            get;
            set;
        }
        /// <summary> 
        /// 可预订情况 -1：暂时下线0：不可预订1：可预订
        /// </summary> 
        public string bookFlag
        {
            get;
            set;
        }
        /// <summary> 
        /// sceneryAmount
        /// </summary> 
        public string sceneryAmount
        {
            get;
            set;
        }
        /// <summary> 
        /// adviceAmount
        /// </summary> 
        public string adviceAmount
        {
            get;
            set;
        }
        /// <summary> 
        /// 等级ID 5星为：AAAAA，4星：AAAA，以此类推
        /// </summary> 
        public string gradeId
        {
            get;
            set;
        }
        /// <summary> 
        /// amount
        /// </summary> 
        public string amount
        {
            get;
            set;
        }
        /// <summary> 
        /// amountAdv
        /// </summary> 
        public string amountAdv
        {
            get;
            set;
        }
        /// <summary> 
        /// lon
        /// </summary> 
        public string lon
        {
            get;
            set;
        }
        /// <summary> 
        /// lat
        /// </summary> 
        public string lat
        {
            get;
            set;
        }
        /// <summary> 
        /// 距离和查询标签或坐标的距离(单位:米)注意 不精确(当使用范围限定参数时才有效)

        /// </summary> 
        public string distance
        {
            get;
            set;
        }

        public List<theme> themeList = new List<theme>();

        [Serializable]
        [XmlRoot("themeList")]
        public class theme
        {
            public int themeId { get; set; }
            public string themeName { get; set; }
        }
        public List<na> naList = new List<na>();
        [Serializable]
        public class na
        {
            public string name { get; set; }
        }

        #endregion

    }

    [Serializable]
    [XmlRoot("scenery")]
    public class sceneryDetail
    {
        public int sceneryId { get; set; }
        public string sceneryName
        {
            get;
            set;
        }
        /// <summary> 
        /// 等级ID 5星为：AAAAA，4星：AAAA，以此类推
        /// </summary> 
        public string gradeId
        {
            get;
            set;
        }
        public string address
        {
            get;
            set;
        }
        /// <summary>
        /// 景点简介
        /// </summary>
        public string intro { get; set; }
        /// <summary>
        /// 购票须知
        /// </summary>
        public string buyNotice { get; set; }
        /// <summary>
        /// 付款类型
        /// </summary>
        public string payMode { get; set; }
        public string lon { get; set; }
        public string lat { get; set; }
        /// <summary>
        /// 景点别名多个已“|”隔开
        /// </summary>
        public string sceneryAlias { get; set; }
        /// <summary>
        /// 该景点的最低价格，可能是儿童价
        /// </summary>
        public int amountAdvice { get; set; }
        /// <summary>
        /// 是否需要证件号0.不需要1.需要
        /// </summary>
        public string ifUseCard { get; set; }
    }

    [Serializable, XmlRoot("scenery")]
    public class SceneryTraffic
    {
        public int sceneryId { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string traffic { get; set; }
    }

 //<scenery>
 //     <sceneryId>{sceneryId}</sceneryId>
 //     <sceneryName>{sceneryName}</sceneryName>
 //     <gradeId>{grade}</gradeId>
 //     <address><![CDATA[ {address} ]]></address>
 //     <city id="{id}">{city}</city>
 //     <province id="{id}">{province}</province>
 //     <intro><![CDATA[ {intro} ]]></intro>
 //     <buyNotice><![CDATA[ {buyNotice} ]]></buyNotice>
 //     <payMode>{payMode}</payMode>
 //     <lon>{lon}</lon>
 //     <lat>{lat}</lat>
 //     <sceneryAlias>{sceneryAlias}</sceneryAlias>
 //     <amountAdvice>{amountAdvice}</amountAdvice>
 //     <ifUseCard>{ifUseCard}</ifUseCard>
 //   </scenery>

}
