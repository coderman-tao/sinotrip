using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.FrameWork.Common
{
    public class Constant
    {
        /// <summary>
        /// 最小日期
        /// </summary>
        public static DateTime MinDateTime = new DateTime(1970, 1, 1);
        /// <summary>
        /// 无效值，用于更新时判断
        /// </summary>
        public static int InvaildValue = -255;
    }
}
