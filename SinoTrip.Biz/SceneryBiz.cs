using SinoTrip.DAL.Common;
using SinoTrip.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinoTrip.Biz
{
    public class SceneryBiz
    {
        public List<Entity.DataBase.Common.common_scenery_type> GetTypeList(Entity.DataBase.Common.common_scenery_type model)
        {
            return new DAL.Common.common_scenery_type().GetList(model);
        }
        /// <summary>
        /// 查询所有有效景点
        /// </summary>
        /// <returns></returns>
        public List<ViewScenery> QueryAllSecery()
        {
            return new DAL.Common.common_scenery().QueryAll();
        }
        /// <summary>
        /// 获取图片列表
        /// </summary>
        /// <param name="SceneryId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<SinoTrip.Entity.DataBase.Common.common_scenery_img> GetImage(int SceneryId, int top)
        {
            if (SceneryId <= 0)
                return null;
            return new DAL.Common.common_scenery_img().GetList(SceneryId, top);
        }

        /// <summary>
        /// 获得评论列表
        /// </summary>
        public List<SinoTrip.Entity.DataBase.Scenery.scenery_comment> GetCommentList(int SceneryId, int page, int pageSize, out int total)
        {
            if (SceneryId <= 0 || pageSize > 1000)
            {
                total = 0;
                return null;
            }

            return new DAL.Scenery.scenery_comment().GetList(SceneryId, page, pageSize, out total);
        }

        public string GetPostByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return string.Empty;
            }
            return new DAL.Scenery.QueryDZPost().GetPostByTag(tag);

        }

        /// <summary>
        /// 获取单个景点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="outSign"></param>
        /// <returns></returns>
        public ViewScenery GetItem(int id, string outSign, string nameLike)
        {
            if (id <= 0 & string.IsNullOrEmpty(outSign) && string.IsNullOrEmpty(nameLike))
            {
                return null;
            }
            return new DAL.Common.common_scenery().GetItem(id, outSign, nameLike);
        }
    }
}
