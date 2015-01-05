using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.API.Ctrip.Model.Hotel
{
    public abstract class BaseCallEntity
    {
        private string requestType;

        /// <summary>
        /// 请求类型
        /// </summary>
        public string RequestType
        {
            get
            {
                return this.requestType;
            }

        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseCallEntity()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestType"></param>
        public BaseCallEntity(string requestType)
        {
            this.requestType = requestType;
        }



        public string RequestContent
        {
            get;
            set;
        }

        /// <summary>
        /// 请求的URL
        /// </summary>
        public string RequestUrl { set; get; }

    }
}
