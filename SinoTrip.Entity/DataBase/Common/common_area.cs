using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTrip.Entity.DataBase.Common
{
    [Serializable]
    public partial class common_area
    {
        public common_area()
        { }
        #region Model
        private int _itemid;
        private int _parent = 0;
        private string _name;
        private string _english;
        private string _abcd;
        private string _suoxie;
        private string _pinyin;
        private string _data;
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
        public int Parent
        {
            set { _parent = value; }
            get { return _parent; }
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
        public string Data
        {
            set { _data = value; }
            get { return _data; }
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
