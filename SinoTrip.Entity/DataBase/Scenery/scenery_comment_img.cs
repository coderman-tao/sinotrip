using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Entity.DataBase.Scenery
{
    [Serializable]
    public partial class scenery_comment_img
    {
        public scenery_comment_img()
        { }
        #region Model
        private int _itemid;
        private int _commentid;
        private int? _sceneryid;
        private string _imgurl;
        private int? _status;
        private string _tempdata;
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
        public int CommentId
        {
            set { _commentid = value; }
            get { return _commentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SceneryId
        {
            set { _sceneryid = value; }
            get { return _sceneryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImgUrl
        {
            set { _imgurl = value; }
            get { return _imgurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TempData
        {
            set { _tempdata = value; }
            get { return _tempdata; }
        }
        #endregion Model

    }
}
