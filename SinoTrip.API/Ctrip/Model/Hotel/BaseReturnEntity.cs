using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Ctrip.Model.Hotel
{
    public class BaseReturnEntity
    {
        /// <summary>
        /// 请求验证结果
        /// </summary>
        public bool ValidateResult { set; get; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string ValidateResultMessage { set; get; }

        /// <summary>
        /// 接口返回信息
        /// </summary>

        public ReturnHeaderInfo HeaderInfo;



        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseReturnEntity()
        {
            ValidateResult = true;
            HeaderInfo = new ReturnHeaderInfo();
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="headerInfo"></param>
        public void GetReturnHeaderInfo(ReturnHeaderInfo headerInfo)
        {
            HeaderInfo = headerInfo;
        }
    }
}
