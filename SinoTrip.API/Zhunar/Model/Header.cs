using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Zhunar.Model
{
    [Serializable]
    public class Header
    {
        public retHeader retHeader;
        public string retcode;
        public string retmsg;
    }

    [Serializable]
    public class retHeader
    {
        public long totalput;
    }
    
}
