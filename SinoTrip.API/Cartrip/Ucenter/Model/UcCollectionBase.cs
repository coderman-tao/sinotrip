﻿using System.Collections;

namespace SinoTrip.API.Cartrip.Ucenter
{
    /// <summary>
    /// 集合基类
    /// </summary>
    public abstract class UcCollectionBase
    {
        private IDictionary _data;
        /// <summary>
        /// 数据
        /// </summary>
        protected IDictionary Data
        {
            get { return _data ?? (_data = new Hashtable()); }
            set { _data = value; }
        }
    }
}
