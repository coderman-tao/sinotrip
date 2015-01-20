using SinoTrip.FrameWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SinoTrip.API.LY.Model
{

    [Serializable, XmlRoot("sceneryList")]
    public class SceneryPrice
    {
        [XmlElement("scenery")]
        public List<PriceItem> scenery { get; set; }
    }
    [Serializable]
    public class PriceItem
    {
        public string sceneryId { get; set; }

        public aheadInfo ahead { get; set; }

        [Serializable]
        public class aheadInfo
        {
            /// <summary>
            /// 提前预订天数
            /// </summary>
            public int day { set; get; }

            /// <summary>
            /// 提前预订时间格式:HH:mm
            /// </summary>
            public string time { set; get; }
        }

        public Policy policy { set; get; }
        [Serializable]
        public class Policy
        {
            [XmlElement("p")]
            public List<SceneryPolicy> Policys{ set; get; }

            /// <summary>
            /// 价格策略
            /// </summary>
            [Serializable]
            public class SceneryPolicy
            {
                /// <summary>
                /// 价格策略id
                /// </summary>
                public int policyId { set; get; }

                /// <summary>
                /// 价格策略名称
                /// </summary>
                public CDATA policyName { set; get; }

                /// <summary>
                /// 门票说明
                /// </summary>
                public CDATA remark { set; get; }

                /// <summary>
                /// 门市价格
                /// </summary>
                public string price { set; get; }

                /// <summary>
                /// 同程价格
                /// </summary>
                public string tcPrice { set; get; }

                /// <summary>
                /// 支付方式
                /// </summary>
                public int pMode { set; get; }

                /// <summary>
                /// 取票方式
                /// </summary>
                public CDATA gMode { set; get; }

                /// <summary>
                /// 最小票数
                /// </summary>
                public int minT { set; get; }

                /// <summary>
                /// 最大票数
                /// </summary>
                public int maxT { set; get; }

                /// <summary>
                /// 最大可用现金券
                /// </summary>
                public decimal dpPrize { set; get; }

                /// <summary>
                /// 预订跳转
                /// </summary>
                public CDATA orderUrl { set; get; }

                /// <summary>
                /// 是否实名制 
                /// </summary>
                public int realName { set; get; }

                /// <summary>
                /// 是否使用二代身份证
                /// </summary>
                public int useCard { set; get; }

                /// <summary>
                /// 门票类型Id
                /// </summary>
                public int ticketId { set; get; }

                /// <summary>
                /// 门票类型名称
                /// </summary>
                public CDATA ticketName { set; get; }

                /// <summary>
                /// 开始时间
                /// 格式：1900-00-00 0000:00:000
                /// </summary>
                public string bDate { set; get; }

                /// <summary>
                /// 结束时间
                /// 格式：1900-00-00 0000:00:000
                /// </summary>
                public string eDate { set; get; }

                /// <summary>
                /// 价格策略有效期类型
                /// 3-特殊日,2-按周,1-按月,0-普通
                /// </summary>
                public int openDateType { set; get; }

                /// <summary>
                /// 价格策略具体有效期
                /// 只针对按周或月有效(已去除前后逗号),
                ///poenDateType=3时 还是取 bDate-eDate
                /// </summary>
                public string openDateValue { set; get; }

                /// <summary>
                /// 价格策略里面的屏蔽节假日
                /// </summary>
                public string closeDate { set; get; }
                /// <summary>
                /// 提前预订天数
                /// </summary>
                public int advanceDay { set; get; }
                /// <summary>
                /// 提前预订时间格式:HH:mm
                /// </summary>
                public string timeLimit { set; get; }
                /// <summary>
                /// 景区包含
                /// </summary>
                public string containItems { set; get; }
                /// <summary>
                /// 下单验证时间间隔 时间间隔，1-天，2-周，3-月
                /// </summary>
                public string timeInterval { set; get; }
                /// <summary>
                /// 下单验证次数次数，例如：2表示某个时间段限制最多2次
                /// </summary>
                // public int timeLimit { set; get; }
                /// <summary>
                /// 下单验证票数 票数，例如：3表示某个时间段限制最多3张票
                /// </summary>
                public string ticketLimit { set; get; }
                /// <summary>
                /// 下单验证方式验证方式，逗号分隔，60501-取票人手机 60502-身份证 60503-会员帐号ID 60504-手机识别码
                /// </summary>
                public string verifyType { set; get; }
                /// <summary>
                /// 是否需要邮箱
                /// </summary>
                public string isNeedMail { set; get; }
            }
        }

        public NoticeData notice { set; get; }

        public class NoticeData
        {
            [XmlElement("n")]
            public List<SceneryNotice> Notices { set; get; }
        }

        /// <summary>
        /// 景区购票须知
        /// </summary>
        [Serializable]
        public class SceneryNotice
        {
            /// <summary>
            /// 类型
            /// </summary>
            public int nType { set; get; }

            /// <summary>
            /// 类型名称
            /// </summary>
            public CDATA nTypeName;

            /// <summary>
            /// 须知列表
            /// </summary>
            public NoticeInfo nInfo { set; get; }


            [Serializable]
            public class NoticeInfo
            {
                [XmlElement("info")]
                public List<NoticeItem> NoticeInfos;
                [Serializable]
                /// <summary>
                /// 须知明细
                /// </summary>
                public class NoticeItem
                {
                    /// <summary>
                    /// 须知排序
                    /// </summary>
                    public int nId { set; get; }

                    /// <summary>
                    /// 须知名称
                    /// </summary>
                    public CDATA nName { set; get; }

                    /// <summary>
                    /// 须知内容
                    /// </summary>
                    public CDATA nContent { set; get; }
                }
            }

        }
    }


    [Serializable, XmlRoot("sceneryList")]
    public class NearByScenerys
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
        public List<Item> Items { get; set; }

        [Serializable]
        public class Item
        {
            public int id { get; set; }
            public string name { get; set; }
            public decimal amount { get; set; }
            public string grade { get; set; }
            public CDATA adress { get; set; }
            public decimal amountAdvice { get; set; }
            public CDATA imgpath { get; set; }
            public decimal distance { get; set; }
//            <adress>
//- <![CDATA[ 安徽省黄山市黄山区汤口镇山岔村
//  ]]> 
//  </adress>
//  <amountAdvice>49.00</amountAdvice> 
//- <imgpath>
//- <![CDATA[ 2014/05/27/2/201405271538368361196.jpg
//  ]]> 
//  </imgpath>
//  <distance>9800382.5209505</distance> 
//  </scenery>

        }
    }
}
