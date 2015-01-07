using SinoTrip.API.LY.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.LY.Biz
{
    public class ScenicBiz
    {
        /// <summary>
        /// 获取同程网省份信息
        /// </summary>
        /// <returns></returns>
        public string GetProvinceList()
        {
            string rs = ApiCommon.GetResult("", "GetProvinceList", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }
        /// <summary>
        /// 根据省份获取同程网城市信息
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns></returns>
        public string GetCityListByProvinceId(int Pid)
        {
            string rs = ApiCommon.GetResult("<provinceId>" + Pid + "</provinceId>", "GetCityListByProvinceId", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }

        public string GetSceneryList(int CityId)
        {
            string rs = ApiCommon.GetResult("<clientIp>127.0.0.1</clientIp><pageSize>100</pageSize><cityId>" + CityId + "</cityId>",
                "GetSceneryList", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            return rs;
        }
        //42
    }
}
