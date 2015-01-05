using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;

namespace SinoTrip.FrameWork.Drawing
{
    /// <summary>
    /// 本类用于产生随机的验证码
    /// </summary>
    public class ValidationCode
    {
        //用户存取验证码字符串
        public string validationCode = String.Empty;

        Graphics g = null;

        int bgWidth = 0;
        int bgHeight = 0;

        public string FontFace = "Comic Sans MS";
        public int FontSize = 9;
        public Color foreColor = Color.FromArgb(220, 220, 220);
        public Color backColor = Color.FromArgb(190, 190, 190);
        public Color mixedLineColor = Color.FromArgb(220, 220, 220);
        public int mixedLineWidth = 1;
        public int mixedLineCount = 5;


        #region 根据指定长度，返回随机验证码
        /// <summary>
        /// 根据指定长度，返回随机验证码
        /// </summary>
        /// <param >制定长度</param>
        /// <returns>随即验证码</returns>
        public string Next(int length)
        {
            this.validationCode = GetRandomCode(length);
            return this.validationCode;
        }
        #endregion


        #region 根据指定长度及背景图片样式，返回带有随机验证码的图片对象
        /// <summary>
        /// 根据指定长度及背景图片样式，返回带有随机验证码的图片对象
        /// </summary>
        /// <param >指定长度</param>
        /// <param >背景图片样式</param>
        /// <returns>Image对象</returns>
        public Image NextImage(int length, HatchStyle hatchStyle, bool allowMixedLines)
        {
            this.validationCode = GetRandomCode(length);

            //校验码字体
            Font myFont = new Font(FontFace, FontSize);

            //根据校验码字体大小算出背景大小
            bgWidth = (int)myFont.Size * length + 4;
            bgHeight = (int)myFont.Size * 2;
            //生成背景图片
            Bitmap myBitmap = new Bitmap(bgWidth, bgHeight);

            g = Graphics.FromImage(myBitmap);


            this.DrawBackground(hatchStyle);
            this.DrawValidationCode(this.validationCode, myFont);
            if (allowMixedLines)
                this.DrawMixedLine();

            return (Image)myBitmap;
        }
        #endregion


        #region 内部方法：绘制验证码背景
        private void DrawBackground(HatchStyle hatchStyle)
        {
            //设置填充背景时用的笔刷
            HatchBrush hBrush = new HatchBrush(hatchStyle, backColor);

            //填充背景图片
            g.FillRectangle(hBrush, 0, 0, this.bgWidth, this.bgHeight);
        }
        #endregion


        #region 内部方法：绘制验证码
        private void DrawValidationCode(string vCode, Font font)
        {
            g.DrawString(vCode, font, new SolidBrush(this.foreColor), 2, 2);
        }
        #endregion


        #region 内部方法：绘制干扰线条
        /// <summary>
        /// 绘制干扰线条
        /// </summary>
        private void DrawMixedLine()
        {
            for (int i = 0; i < mixedLineCount; i++)
            {
                g.DrawBezier(new Pen(new SolidBrush(mixedLineColor), mixedLineWidth), RandomPoint(), RandomPoint(), RandomPoint(), RandomPoint());
            }
        }
        #endregion

        #region 内部方法：返回指定长度的随机验证码字符串
        /// <summary>
        /// 根据指定大小返回随机验证码
        /// </summary>
        /// <param >字符串长度</param>
        /// <returns>随机字符串</returns>
        private string GetRandomCode(int length)
        {
            ArrayList MyArray = new ArrayList();
            Random random = new Random();
            string str = null;
            //循环的次数   
            int Nums = length;
            while (Nums > 0)
            {
                int i = random.Next(1, 9);
                MyArray.Add(i);
                Nums -= 1;
            }

            for (int j = 0; j <= MyArray.Count - 1; j++)
            {
                str += MyArray[j].ToString();
            }
            return str;
            //StringBuilder sb = new StringBuilder(6);

            //for (int i = 0; i < length; i++)
            //{
            //    sb.Append(Char.ConvertFromUtf32(RandomAZ09()));
            //}

            //return sb.ToString();
        }
        #endregion


        #region 内部方法：产生随机数和随机点

        /// <summary>
        /// 产生0-9A-Z的随机字符代码
        /// </summary>
        /// <returns>字符代码</returns>
        private int RandomAZ09()
        {
            Thread.Sleep(15);
            int result = 48;
            Random ram = new Random();
            int i = ram.Next(2);

            switch (i)
            {
                case 0:
                    result = ram.Next(48, 58);
                    break;
                case 1:
                    result = ram.Next(65, 91);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 返回一个随机点，该随机点范围在验证码背景大小范围内
        /// </summary>
        /// <returns>Point对象</returns>
        private Point RandomPoint()
        {
            Thread.Sleep(15);
            Random ram = new Random();
            Point point = new Point(ram.Next(this.bgWidth), ram.Next(this.bgHeight));
            return point;
        }
        #endregion
    }
}
