using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.DataBase.Common
{
    [Serializable]
    public partial class common_city
    {
        public common_city()
        { }
        #region Model
        private int _itemid;
        private string _country;
        private string _province;
        private string _name;
        private string _abcd;
        private string _suoxie;
        private string _pinyin;
        private decimal? _lat;
        private decimal? _lng;
        private int _isbigcity = 0;
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
        public string Country
        {
            set { _country = value; }
            get { return _country; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Province
        {
            set { _province = value; }
            get { return _province; }
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
        public string ABCD
        {
            set { _abcd = value; }
            get { return _abcd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Suoxie
        {
            set { _suoxie = value; }
            get { return _suoxie; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pinyin
        {
            set { _pinyin = value; }
            get { return _pinyin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Lat
        {
            set { _lat = value; }
            get { return _lat; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Lng
        {
            set { _lng = value; }
            get { return _lng; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsBigCity
        {
            set { _isbigcity = value; }
            get { return _isbigcity; }
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
