using SinoTrip.API.Ctrip.Hotel.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Ctrip.Model.Hotel.Response
{
    public class OTA_HotelRatePlanRS : BaseReturnEntity
    {
        private List<HotelRatePlan> hotelRatePlanList = new List<HotelRatePlan>();

        /// <summary>
        /// 酒店价格计划列表
        /// </summary>
        public List<HotelRatePlan> HotelRatePlanList
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
    }
}
