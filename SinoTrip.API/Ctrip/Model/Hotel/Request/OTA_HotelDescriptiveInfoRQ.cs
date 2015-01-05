using SinoTrip.API.Ctrip.Hotel.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Ctrip.Model.Hotel.Request
{
    public class OTA_HotelDescriptiveInfoRQ : BaseCallEntity
    {
        public OTA_HotelDescriptiveInfoRQ()
            : base("OTA_HotelDescriptiveInfo")
        {
            this.descriptiveInfos = new List<DescriptiveInfo>();
        }

        private List<DescriptiveInfo> descriptiveInfos;

        /// <summary>
        /// 酒店详细描述信息请求列表，可以请求多个酒店
        /// </summary>
        public List<DescriptiveInfo> DescriptiveInfos
        {
            get
            {
                return this.descriptiveInfos;
            }
            set
            {
                this.descriptiveInfos = value;
            }
        }
    }
}
