using SinoTrip.API.LY.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SinoTrip.FrameWork.Common;

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

        public string GetCountyListByCityId(int cid)
        {
            string rs = ApiCommon.GetResult("<cityId>" + cid + "</cityId>", "GetCountyListByCityId", "http://tcopenapi.17usoft.com/Handlers/General/AdministrativeDivisionsHandler.ashx");
            return rs;
        }
        /// <summary>
        /// 根据城市获取景点
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetSceneryList(int CityId, int page)
        {
            string rs = ApiCommon.GetResult("<clientIp>127.0.0.1</clientIp><page>" + page + "</page><pageSize>100</pageSize><cityId>" + CityId + "</cityId>",
                "GetSceneryList", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            return rs;
        }

        public string GetSceneryDetail(int SceneryId)
        {
            string PostData = "<sceneryId>" + SceneryId + "</sceneryId><cs>2</cs>";
            string rs = ApiCommon.GetResult(PostData, "GetSceneryDetail", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            return rs;

        }
        /// <summary>
        /// 获取景点交通信息
        /// </summary>
        /// <param name="sceneryId"></param>
        /// <returns></returns>
        public string GetSceneryTrafficInfo(int sceneryId)
        {
            string PostData = "<sceneryId>" + sceneryId + "</sceneryId>";
            string rs = ApiCommon.GetResult(PostData, "GetSceneryTrafficInfo", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
            return rs;
        }

        /// <summary>
        /// 获取景区价格
        /// </summary>
        /// <param name="showDetail">1、简单 2、详细默认为1</param>
        /// <param name="ids">可以传入多个逗号分隔，必填。 示例：3440,1360,79,…，一次最多20个</param>
        /// <returns></returns>
        public SinoTrip.API.LY.Model.SceneryPrice GetSceneryPrice(int showDetail, List<int> ids)
        {
            string sceneryIds = string.Join(",", ids);
            string PostData = "<showDetail>" + showDetail + "</showDetail><sceneryIds>" + sceneryIds + "</sceneryIds><payType>0</payType>";

            string rs = ApiCommon.GetResult(PostData, "GetSceneryPrice", "http://tcopenapi.17usoft.com/handlers/scenery/queryhandler.ashx");
           //rs = rs.Replace("&", "&amp;");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rs);
            return doc.SelectSingleNode("response/body").InnerXml.XmlToEntity<SinoTrip.API.LY.Model.SceneryPrice>();
            //return rs;
        }

       // public string GetPriceCalendar()
    }
}
