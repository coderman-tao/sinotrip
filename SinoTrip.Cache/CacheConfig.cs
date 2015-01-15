using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinoTrip.FrameWork.Common;
using System.Configuration;

namespace SinoTrip.Cache
{
    public class CacheConfig
    {
        public static int District { get { return -ConfigurationManager.AppSettings["Area_Timeout"].ToInt32(0); } }

    }
}
