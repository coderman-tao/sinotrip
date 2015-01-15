using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity
{
    [Serializable]
    public class BaseEntity
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex;
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize;
        /// <summary>
        /// 每页开始索引
        /// </summary>
        public int PageStart;
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int Total;
        /// <summary>
        /// 创建者姓名
        /// </summary>
        public string creatorname;
        /// <summary>
        /// 操作人ID
        /// </summary>
        public int OperatorId;
        /// <summary>
        /// 有效时间
        /// </summary>
        public int ValidTime;


    }
}
