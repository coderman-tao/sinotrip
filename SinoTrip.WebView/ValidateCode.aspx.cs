using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SinoTrip.FrameWork.Common;
using SinoTrip.FrameWork.Drawing;
using SinoTrip.FrameWork.Security;
using System.Drawing;

namespace SinoTrip.WebView
{
    public partial class ValidateCode : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            var size = Context.Request.QueryString["font"].ToInt32(0);
            var count = Context.Request.QueryString["count"].ToInt32(0);
            ValidationCode validate = new ValidationCode();
            validate.backColor = Color.WhiteSmoke;
            validate.FontSize = size;
            validate.foreColor = Color.White;
            validate.mixedLineColor = Color.LightYellow;
            System.Drawing.Image image = validate.NextImage(count, System.Drawing.Drawing2D.HatchStyle.LightUpwardDiagonal, true);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            Context.Response.ClearContent();
            Context.Response.ContentType = "image/Gif";
            Context.Response.BinaryWrite(ms.ToArray());
            Context.Response.Cookies[SinoTrip.WebBase.HttpBase.CookieKeys.VCode].Value = Md5.GetMD5(validate.validationCode.ToLower());
        }
    }
}