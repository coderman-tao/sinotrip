using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SinoTrip.API.LY.Model
{
    [Serializable]
    public class provinceList
    {
        [XmlElement("province")]
        public List<AreaInfo> Province { get; set; }
    }
    [Serializable]
    public class AreaInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string enName { get; set; }
        public string prefixLetter { get; set; }
        //        <id>2</id>
        //<name>安徽</name>
        //<enName>AnHui</enName>
        //<prefixLetter>A</prefixLetter>
    }
    [Serializable]
    public class cityList
    {
        [XmlAttribute]
        public int totalCount { get; set; }
        [XmlElement("city")]
        public List<AreaInfo> Citys { get; set; }
    }

}
