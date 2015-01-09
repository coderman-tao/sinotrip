using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.DataBase.Hotel
{
    [Serializable]
    public partial class hotel_chain
    {
        public hotel_chain()
        { }
        #region Model
        private int _itemid;
        private string _name;
        private string _liansuo;
        private string _thumbnail;
        private int? _hotelnum;
        private string _ls;
        private int _level;
        private string _levelname;
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
        public string Liansuo
        {
            set { _liansuo = value; }
            get { return _liansuo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Thumbnail
        {
            set { _thumbnail = value; }
            get { return _thumbnail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? HotelNum
        {
            set { _hotelnum = value; }
            get { return _hotelnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ls
        {
            set { _ls = value; }
            get { return _ls; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            set { _level = value; }
            get { return _level; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LevelName
        {
            set { _levelname = value; }
            get { return _levelname; }
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
