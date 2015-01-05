using SinoTrip.API.Ctrip.Hotel.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Ctrip.Model.Hotel.Response
{
    public class OTA_HotelDescriptiveInfoRS : BaseReturnEntity
    {
        /// <summary>
        /// 酒店静态信息
        /// </summary>
        public List<HotelDescriptiveInfo> DescriptiveInfos { set; get; }
    }
}
