using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.DataBase.Common
{
    [Serializable]
    public partial class common_city_area_outsign
    {
        public common_city_area_outsign()
        { }
        #region Model
        private int _itemid;
        private int _areaid;
        private int _supply;
        private string _outsign;
        private string _outdata;
        private int? _status;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int ItemId
        {
            set { _itemid = value; }
            get { return _itemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AreaId
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Supply
        {
            set { _supply = value; }
            get { return _supply; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OutSign
        {
            set { _outsign = value; }
            get { return _outsign; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OutData
        {
            set { _outdata = value; }
            get { return _outdata; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        #endregion Model

    }
}
