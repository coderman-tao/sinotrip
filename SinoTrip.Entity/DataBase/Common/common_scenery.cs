using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoTrip.Entity.DataBase.Common
{
    [Serializable]
    public partial class common_scenery
    {
        public common_scenery()
        { }
        #region Model
        private int _itemid;
        private string _name;
        private string _address;
        private string _summary;
        private string _cover;
        private int? _cityid;
        private string _cityname;
        private int? _countyid;
        private string _countyname;
        private string _grade;
        private string _themename;
        private decimal? _lat;
        private decimal? _lng;
        private string _intro;
        private string _buynotie;
        private string _alias;
        private string _traffic;
        private string _nearid;
        private int _status;
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
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Summary
        {
            set { _summary = value; }
            get { return _summary; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Cover
        {
            set { _cover = value; }
            get { return _cover; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CityId
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
        public int? CountyId
        {
            set { _countyid = value; }
            get { return _countyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CountyName
        {
            set { _countyname = value; }
            get { return _countyname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThemeName
        {
            set { _themename = value; }
            get { return _themename; }
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
        public string Intro
        {
            set { _intro = value; }
            get { return _intro; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyNotie
        {
            set { _buynotie = value; }
            get { return _buynotie; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Alias
        {
            set { _alias = value; }
            get { return _alias; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Traffic
        {
            set { _traffic = value; }
            get { return _traffic; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NearId
        {
            set { _nearid = value; }
            get { return _nearid; }
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
