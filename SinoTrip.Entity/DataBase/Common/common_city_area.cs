using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.DataBase.Common
{
    [Serializable]
    public partial class common_city_area
    {
        public common_city_area()
        { }
        #region Model
        private int _itemid;
        private string _name;
        private string _english;
        private string _abcd;
        private int? _cityid;
        private string _cityname;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string English
        {
            set { _english = value; }
            get { return _english; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ABCD
        {
            set { _abcd = value; }
            get { return _abcd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CityName
        {
            set { _cityname = value; }
            get { return _cityname; }
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
