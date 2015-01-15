using SinoTrip.Entity.DataBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.ViewModel
{
    [Serializable]
    public class ViewScenery : common_scenery
    {
        public string OutSign { get; set; }

        public int Supply { get; set; }
    }
}
