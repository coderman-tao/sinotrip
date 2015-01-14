using SinoTrip.Entity.DataBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.ViewModel
{
    public class ViewCity : common_city
    {
        public string OutSign { get; set; }
        public int Supply { get; set; }
    }
}
