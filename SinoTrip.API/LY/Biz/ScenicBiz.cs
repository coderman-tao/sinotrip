using SinoTrip.API.LY.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.LY.Biz
{
    public class ScenicBiz
    {

        public string GetProvinceList()
        {
            string rs = ApiCommon.GetResult("", "GetProvinceList", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }

        public string GetCityListByProvinceId(int Pid)
        {
            string rs = ApiCommon.GetResult("<provinceId>"+Pid+"</provinceId>", "GetCityListByProvinceId", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }

        //42
    }
}
