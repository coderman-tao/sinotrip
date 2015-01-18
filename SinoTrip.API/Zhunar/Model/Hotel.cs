using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Zhunar.Model
{
    [Serializable]
    public class Hotel : Header
    {
        public List<HotelInfo> reqdata;
    }
    [Serializable]
    public class HotelInfo
    {
        public int id;
        public string hotelname;
        /// <summary>
        ///  酒店地址
        /// </summary>
        public string address;
        /// <summary>
        /// 酒店代表图
        /// </summary>
        public string picture;
        /// <summary>
        /// 经度
        /// </summary>
        public int x;
        /// <summary>
        /// 纬度
        /// </summary>
        public int y;
        /// <summary>
        /// 最低价格
        /// </summary>
        public int min_jiage;
        /// <summary>
        /// 最大价格
        /// </summary>
        public int max_jiangjin;
        /// <summary>
        /// 旧商业区(已废弃)
        /// </summary>
        public string cbd;
        /// <summary>
        /// 旧商业区id(已废弃)
        /// </summary>
        public int cid;
        /// <summary>
        /// 连锁酒店名称
        /// </summary>
        public string liansuo;
        /// <summary>
        ///   酒店星级
        /// </summary>
        public int xingji;
        /// <summary>
        /// 酒店服务(以|分隔，如：adsl|card，推荐使用service字段)
        /// </summary>
        public string fuwu;
        /// <summary>
        ///  开业时间
        /// </summary>
        public string kaiye;
        /// <summary>
        ///  装修时间
        /// </summary>
        public string zhuangxiu;
        /// <summary>
        /// 早餐价格
        /// </summary>
        public int zaocanPrice;
        /// <summary>
        /// 交通位置
        /// </summary>
        public string traffic;
        /// <summary>
        /// 服务
        /// </summary>
        public string service;
        /// <summary>
        ///  餐饮信息
        /// </summary>
        public string canyin;
        /// <summary>
        ///  所支持的银行卡((以|分隔，如：牡丹卡,金穗卡,长城卡,龙卡,太平洋卡,东方卡)
        /// </summary>
        public string card;
        /// <summary>
        /// 城市id (不推荐)
        /// </summary>
        public int cityid;
        /// <summary>
        /// 城市id
        /// </summary>
        public string ecityid;
        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityname;
        /// <summary>
        /// 评分（好评+中评-差评=总分）
        /// </summary>
        public string df_scores;
        /// <summary>
        /// 评价（以$分隔，如：37$13$2，代表好评$中评$差评）
        /// </summary>
        public string df_haoping;
        /// <summary>
        /// 酒店简介
        /// </summary>
        public string content;
        /// <summary>
        /// 经度
        /// </summary>
        public float lng;
        /// <summary>
        /// 纬度
        /// </summary>
        public float lat;
        /// <summary>
        /// 百度地图经度
        /// </summary>
        public float baidu_lng;
        /// <summary>
        /// 百度地图纬度
        /// </summary>
        public float baidu_lat;
        /// <summary>
        /// 酒店tags（以，分隔，如：安静,经济,出行方便,繁华地区,优质服务）
        /// </summary>
        public string tags;
        /// <summary>
        ///  商业区id
        /// </summary>
        public string esdid;
        /// <summary>
        /// 连锁酒店id
        /// </summary>
        public int liansuoid;
        /// <summary>
        /// 行政区域id
        /// </summary>
        public string eareaid;
        /// <summary>
        ///  商业区名称
        /// </summary>
        public string esdname;
        /// <summary>
        /// 距离（单位米，如果经纬度不为空，则显示此字段）
        /// </summary>
        public int juli;
        /// <summary>
        /// 娱乐信息
        /// </summary>
        public string yulejianshen;
        /// <summary>
        /// 交通指南
        /// </summary>
        public string traffic_zhinan;
        /// <summary>
        /// 酒店简述
        /// </summary>
        public string jianshu;
        /// <summary>
        /// 酒店荣誉（特色）
        /// </summary>
        public string teshe;
        /// <summary>
        /// 1为客栈 0为酒店
        /// </summary>
        public int is_kezhan;
    }
}
