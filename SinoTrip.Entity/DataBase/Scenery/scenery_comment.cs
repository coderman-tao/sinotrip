using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.DataBase.Scenery
{
    [Serializable]
    public partial class scenery_comment
    {
        public scenery_comment()
        { }
        #region Model
        private int _itemid;
        private int _sceneryid;
        private int? _outsign;
        private int? _rank = 1;
        private string _dptitle;
        private string _comment;
        private int? _uid;
        private string _username;
        private string _dpservice;
        private string _dpshigouyu;
        private string _dptraffic;
        private int? _servicescore;
        private string _servicegrade;
        private int? _convenientscore;
        private string _convenientgrade;
        private int? _discountscore;
        private string _discountgrade;
        private string _dpganwu;
        private int? _dptime;
        private string _tempdata;
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
        public int SceneryId
        {
            set { _sceneryid = value; }
            get { return _sceneryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OutSign
        {
            set { _outsign = value; }
            get { return _outsign; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Rank
        {
            set { _rank = value; }
            get { return _rank; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DPTitle
        {
            set { _dptitle = value; }
            get { return _dptitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Comment
        {
            set { _comment = value; }
            get { return _comment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Uid
        {
            set { _uid = value; }
            get { return _uid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DPService
        {
            set { _dpservice = value; }
            get { return _dpservice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DPShiGouYu
        {
            set { _dpshigouyu = value; }
            get { return _dpshigouyu; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DPTraffic
        {
            set { _dptraffic = value; }
            get { return _dptraffic; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ServiceScore
        {
            set { _servicescore = value; }
            get { return _servicescore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceGrade
        {
            set { _servicegrade = value; }
            get { return _servicegrade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ConvenientScore
        {
            set { _convenientscore = value; }
            get { return _convenientscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ConvenientGrade
        {
            set { _convenientgrade = value; }
            get { return _convenientgrade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DiscountScore
        {
            set { _discountscore = value; }
            get { return _discountscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DiscountGrade
        {
            set { _discountgrade = value; }
            get { return _discountgrade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DPGanwu
        {
            set { _dpganwu = value; }
            get { return _dpganwu; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DPTime
        {
            set { _dptime = value; }
            get { return _dptime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TempData
        {
            set { _tempdata = value; }
            get { return _tempdata; }
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
