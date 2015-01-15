using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.DataBase.Common
{
    [Serializable]
    public partial class common_scenery_type
    {
        public common_scenery_type()
        { }
        #region Model
        private int _itemid;
        private string _name;
        private int? _orderno;
        private int _status = 0;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        #endregion Model

    }
}
