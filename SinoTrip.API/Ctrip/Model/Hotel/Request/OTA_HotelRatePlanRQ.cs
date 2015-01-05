using SinoTrip.API.Ctrip.Hotel.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Ctrip.Model.Hotel.Request
{
    public class OTA_HotelRatePlanRQ : BaseCallEntity
    {
        private List<HotelRatePlanRQ> hotelRatePlanList;
        private DateTime timeStamp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OTA_HotelRatePlanRQ()
            : base("OTA_HotelRatePlan")
        {
            this.timeStamp = DateTime.Now;
            this.hotelRatePlanList = new List<HotelRatePlanRQ>();

        }

        /// <summary>
        /// 价格计划查询列表（价格计划对应Ctrip子房型）,
        /// 可罗列多个酒店进行查询
        /// </summary>
        public List<HotelRatePlanRQ> HotelRatePlanList
        {
            get
            {
                return this.hotelRatePlanList;
            }
            set
            {
                this.hotelRatePlanList = value;
            }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                return this.timeStamp;
            }
            set
            {
                this.timeStamp = value;
            }
        }
    }
}
