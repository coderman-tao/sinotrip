using SinoTrip.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTrip.Biz.Common
{
    public class AreaBiz
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ViewArea> GetAreaList(ViewArea model)
        {
            return new DAL.Common.common_area().GetList(model);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ViewCity> GetCityList(ViewCity model)
        {
            return new DAL.Common.common_city().GetList(model);
        }
        /// <summary>
        /// 获取城市区域
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ViewCityArea> GetCityAreaList(ViewCityArea model)
        {
            return new DAL.Common.common_city_area().GetList(model);
        }
    }
}
