using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace SinoTrip.FrameWork.Drawing
{
    public class ImageOperation
    {
        /// <summary>
        /// 反色，即底片效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <returns>处理后的图片</returns>
        public static Image Negative(Image imgIn)
        {

            if (null == imgIn)
                throw new Exception();

            int iHeight = imgIn.Height;
            int iWidth = imgIn.Width;

            Bitmap newBitmap = new Bitmap(iWidth, iHeight);
            Bitmap oldBitmap = imgIn as Bitmap;

            try
            {
                Color pixel;//表示一个像素点

                for (int x = 1; x < iWidth; x++)
                {
                    for (int y = 1; y < iHeight; y++)
                    {
                        int r, g, b;//分别表示一个像素点红，绿，蓝的分量。

                        pixel = oldBitmap.GetPixel(x, y);

                        r = 255 - pixel.R;
                        g = 255 - pixel.G;
                        b = 255 - pixel.B;

                        newBitmap.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                    }
                }
            }
            catch (Exception ee)
            {
                throw new Exception();
            }
            return newBitmap;
        }

        /// <summary>
        /// 浮雕效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <returns>处理后的图片</returns>
        public static Image Relievo(Image imgIn)
        {
            if (null == imgIn)
                throw new Exception();

            int iHeight = imgIn.Height;
            int iWidth = imgIn.Width;

            Bitmap newBitmap = new Bitmap(iWidth, iHeight);
            Bitmap oldBitmap = imgIn as Bitmap;

            try
            {

                Color pixel1, pixel2;
                for (int x = 0; x < iWidth - 1; x++)
                {
                    for (int y = 0; y < iHeight - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        pixel1 = oldBitmap.GetPixel(x, y);
                        pixel2 = oldBitmap.GetPixel(x + 1, y + 1);
                        r = Math.Abs(pixel1.R - pixel2.R + 128);
                        g = Math.Abs(pixel1.G - pixel2.G + 128);
                        b = Math.Abs(pixel1.B - pixel2.B + 128);

                        //处理颜色溢出
                        r = CheckRange(r);
                        g = CheckRange(g);
                        b = CheckRange(b);

                        newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return newBitmap;
        }

        /// <summary>
        /// 灰度效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <param name="red">红色值加权比例</param>
        /// <param name="green">绿色值加权比例</param>
        /// <param name="blue">蓝色值加权比例</param>
        /// <param name="bOptAdd">是增加还是降低灰度，true为增加，false为减少</param>
        /// <returns>处理后的图片</returns>
        public static Image Grayscale(Image imgIn, float red, float green, float blue, bool bOptAdd)
        {
            if (null == imgIn)
                throw new Exception();

            int iHeight = imgIn.Height;
            int iWidth = imgIn.Width;

            Bitmap newBitmap = new Bitmap(iWidth, iHeight);
            Bitmap oldBitmap = imgIn as Bitmap;

            try
            {

                Color pixel;
                for (int x = 0; x < iWidth - 1; x++)
                {
                    for (int y = 0; y < iHeight - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        pixel = oldBitmap.GetPixel(x, y);

                        r = CalculatePixel(pixel.R, red, bOptAdd);
                        g = CalculatePixel(pixel.G, green, bOptAdd);
                        b = CalculatePixel(pixel.B, blue, bOptAdd);

                        //处理颜色值溢出
                        r = CheckRange(r);
                        g = CheckRange(g);
                        b = CheckRange(b);

                        newBitmap.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception();
            }
            return newBitmap;
        }

        /// <summary>
        /// 灰度效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <param name="bOptAdd">是增加还是降低灰度，true为增加，false为减少</param>
        /// <returns>处理后的图片</returns>
        public static Image Grayscale(Image imgIn, bool bOptAdd)
        {

            //if (null == imgIn)
            //    throw new Exception();

            //Bitmap bmp2 = (Bitmap)imgIn.Clone();
            //int width = bmp2.Width;
            //int height = bmp2.Height;
            //Rectangle rect = new Rectangle(0, 0, width, height);
            ////用可读写的方式锁定全部位图像素
            //BitmapData bmpData = bmp2.LockBits(rect, ImageLockMode.ReadWrite, bmp2.PixelFormat);
            ////得到首地址
            //IntPtr ptr = bmpData.Scan0;
            ////24位bmp位图字节数
            //int bytes = width * height * 3;

            //byte[] rgbValues = new byte[bytes];

            //Marshal.Copy(ptr, rgbValues, 0, bytes);

            ////灰度化
            //double colorTemp = 0;
            //for (int i = 0; i < bytes; i += 3)
            //{
            //    colorTemp = rgbValues[i + 2] * 0.299 + rgbValues[i + 1] * 0.587 + rgbValues[i] * 0.114;
            //    rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = (byte)colorTemp;
            //}

            return null;
        }


        /// <summary>
        /// 木刻效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <returns>处理后的图片</returns>
        public static Image Woodcut(Image imgIn)
        {
            if (null == imgIn)
                throw new Exception();


            int Height = imgIn.Height;
            int Width = imgIn.Width;
            Bitmap newBitmap = new Bitmap(Width, Height);
            Bitmap oldBitmap = (Bitmap)imgIn;

            try
            {
                Color pixel1, pixel2;
                for (int x = 0; x < Width - 1; x++)
                {
                    for (int y = 0; y < Height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        pixel1 = oldBitmap.GetPixel(x, y);
                        pixel2 = oldBitmap.GetPixel(x + 1, y + 1);
                        r = Math.Abs(pixel1.R - pixel2.R + 128);
                        g = Math.Abs(pixel1.G - pixel2.G + 128);
                        b = Math.Abs(pixel1.B - pixel2.B + 128);

                        //处理颜色值溢出
                        r = CheckRange(r);
                        g = CheckRange(g);
                        b = CheckRange(b);

                        newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            return newBitmap;
        }

        /// <summary>
        /// 锐化效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <param name="factor">锐化系数</param>
        /// <returns>处理后的图片</returns>
        public static Image Sharpen(Image imgIn, int factor)
        {

            if (null == imgIn)
                throw new Exception();

            int Height = imgIn.Height;
            int Width = imgIn.Width;

            Bitmap newBitmap = new Bitmap(Width, Height);
            Bitmap oldBitmap = imgIn as Bitmap;

            //比例
            float sharpen = factor / 100.0f;

            try
            {
                Color pixel;
                //拉普拉斯模板
                int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
                for (int x = 1; x < Width - 1; x++)
                    for (int y = 1; y < Height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        int Index = 0;
                        for (int col = -1; col <= 1; col++)
                            for (int row = -1; row <= 1; row++)
                            {
                                pixel = oldBitmap.GetPixel(x + row, y + col);
                                r += pixel.R * Laplacian[Index];
                                g += pixel.G * Laplacian[Index];
                                b += pixel.B * Laplacian[Index];
                                Index++;
                            }
                        //处理颜色值溢出
                        r = CheckRange(r);
                        g = CheckRange(g);
                        b = CheckRange(b);

                        newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                    }
            }
            catch (Exception ee)
            {
                throw new Exception();
            }

            return newBitmap;
        }

        /// <summary>
        /// 柔化效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <param name="factor">柔化系数</param>
        /// <returns>处理后的图片</returns>
        public static Image Blur(Image imgIn, int factor)
        {
            if (null == imgIn)
                throw new Exception();

            int Height = imgIn.Height;
            int Width = imgIn.Width;
            Bitmap newBitmap = new Bitmap(Width, Height);
            Bitmap oldBitmap = imgIn as Bitmap;

            try
            {
                Color pixel;
                //高斯模板
                int[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
                for (int x = 1; x < Width - 1; x++)
                    for (int y = 1; y < Height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        int Index = 0;
                        for (int col = -1; col <= 1; col++)
                            for (int row = -1; row <= 1; row++)
                            {
                                pixel = oldBitmap.GetPixel(x + row, y + col);
                                r += pixel.R * Gauss[Index];
                                g += pixel.G * Gauss[Index];
                                b += pixel.B * Gauss[Index];
                                Index++;
                            }

                        r /= 16;
                        g /= 16;
                        b /= 16;

                        //处理颜色值溢出
                        r = CheckRange(r);
                        g = CheckRange(g);
                        b = CheckRange(b);

                        newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                    }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            return newBitmap;
        }

        /// <summary>
        /// 黑白效果
        /// </summary>
        /// <param name="imgIn">输入的源图片</param>
        /// <returns>处理后的图片</returns>
        public static Image BlackAndWhite(Image imgIn)
        {

            if (null == imgIn)
                throw new Exception();

            int Height = imgIn.Height;
            int Width = imgIn.Width;
            Bitmap newBitmap = new Bitmap(Width, Height);
            Bitmap oldBitmap = imgIn as Bitmap;

            try
            {
                Color pixel;
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        pixel = oldBitmap.GetPixel(x, y);
                        int r, g, b, Result = 0;
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;
                        //实例程序以加权平均值法产生黑白图像
                        int iType = 2;
                        switch (iType)
                        {
                            case 0://平均值法
                                Result = ((r + g + b) / 3);
                                break;
                            case 1://最大值法
                                Result = r > g ? r : g;
                                Result = Result > b ? Result : b;
                                break;
                            case 2://加权平均值法
                                Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                                break;
                        }
                        newBitmap.SetPixel(x, y, Color.FromArgb(pixel.A, Result, Result, Result));
                    }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            return newBitmap;
        }

        /// <summary>
        /// 图像明暗调整
        /// </summary>
        /// <param name="bitmap">原始图</param>
        /// <param name="degree">亮度[-255, 255]</param>
        /// <returns>处理后的图片</returns>
        public static Image Lighten(Bitmap bitmap, int degree)
        {
            if (bitmap == null)
                throw new Exception();

            //原图片的长和宽
            int width = bitmap.Width;
            int height = bitmap.Height;

            Bitmap newBitmap = new Bitmap(width, height);

            try
            {
                Color pixel;

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        int r, g, b;//代表红，绿，蓝

                        pixel = bitmap.GetPixel(x, y);
                        r = CheckRange(pixel.R + degree);
                        g = CheckRange(pixel.G + degree);
                        b = CheckRange(pixel.B + degree);

                        newBitmap.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                    }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return newBitmap;
        }

        /// <summary>
        /// 图像对比度调整
        /// </summary>
        /// <param name="b">原始图</param>
        /// <param name="degree">对比度[-100, 100]</param>
        /// <returns>处理后的图像</returns>
        public static Image Contrast(Bitmap b, int degree)
        {
            if (b == null)
                throw new Exception();

            if (degree < -100) degree = -100;
            if (degree > 100) degree = 100;

            try
            {

                double pixel = 0;
                double contrast = (100.0 + degree) / 100.0;
                contrast *= contrast;
                int width = b.Width;
                int height = b.Height;
                BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* p = (byte*)data.Scan0;
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // 处理指定位置像素的对比度
                            for (int i = 0; i < 3; i++)
                            {
                                pixel = ((p[i] / 255.0 - 0.5) * contrast + 0.5) * 255;
                                if (pixel < 0) pixel = 0;
                                if (pixel > 255) pixel = 255;
                                p[i] = (byte)pixel;
                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }
                b.UnlockBits(data);
                return b;
            }
            catch
            {
                return null;
            }
        } // end of Contrast

        /// <summary>
        /// 掩值效果
        /// </summary>
        /// <param name="imgIn">原始图</param>
        /// <param name="upLine">上阈值</param>
        /// <param name="downLine">下阈值</param>
        /// <returns></returns>
        public static Image ScanLine(Image imgIn, int upLine, int downLine)
        {


            return null;
        }

        /// <summary>
        /// 垂直翻转
        /// </summary>
        /// <param name="imgIn">原始图像</param>
        /// <returns>处理后的图像</returns>
        public static Image FlipVertical(Image imgIn)
        {
            if (null == imgIn)
                throw new Exception();

            imgIn.RotateFlip(RotateFlipType.Rotate180FlipX);

            return imgIn;
        }

        /// <summary>
        /// 水平翻转
        /// </summary>
        /// <param name="imgIn">原始图像</param>
        /// <returns>处理后的图像</returns>
        public static Image FlipHorizontal(Image imgIn)
        {
            if (null == imgIn)
                throw new Exception();

            imgIn.RotateFlip(RotateFlipType.Rotate180FlipY);

            return imgIn;
        }


        /// <summary>
        /// 以逆时针为方向对图像进行旋转
        /// </summary>
        /// <param name="b">位图流</param>
        /// <param name="angle">旋转角度[0,360](前台给的)</param>
        /// <returns></returns>
        public static Image RotationLeft(Image imgIn, int angle)
        {

            //图像输入异常
            if (null == imgIn)
                throw new Exception();

            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w = imgIn.Width;
            int h = imgIn.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);

            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(imgIn, rect);

            //重至绘图的所有变换
            g.ResetTransform();

            g.Save();
            g.Dispose();
            //dsImage.Save("yuancd.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return dsImage;

        }

        /// <summary>
        /// 边缘检测
        /// </summary>
        /// <param name="imgIn">原始图</param>
        /// <returns>处理后图片</returns>
        public static Image EdgeDetection(Image imgIn)
        {
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };   //  The matrix Gx
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };  //  The matrix Gy
            Bitmap b = (Bitmap)imgIn;
            Bitmap b1 = new Bitmap(imgIn);
            for (int i = 1; i < b.Height - 1; i++)   // loop for the image pixels height
            {
                for (int j = 1; j < b.Width - 1; j++) // loop for image pixels width    
                {
                    float new_x = 0, new_y = 0;
                    float c;
                    for (int hw = -1; hw < 2; hw++)  //loop for cov matrix
                    {
                        for (int wi = -1; wi < 2; wi++)
                        {
                            c = (b.GetPixel(j + wi, i + hw).B + b.GetPixel(j + wi, i + hw).R + b.GetPixel(j + wi, i + hw).G) / 3;
                            new_x += gx[hw + 1, wi + 1] * c;
                            new_y += gy[hw + 1, wi + 1] * c;
                        }
                    }
                    if (new_x * new_x + new_y * new_y > 128 * 128)
                        b1.SetPixel(j, i, Color.Black);
                    else
                        b1.SetPixel(j, i, Color.White);
                }
            }
            return (Image)b1;

        }


        /// <summary>
        /// 按照比例图片缩放
        /// </SUMMARY>
        /// <PARAM name="image">原始图片</PARAM>
        /// <PARAM name="destHeight">缩放后图片高度</PARAM>
        /// <PARAM name="destWidth">缩放后图片宽度</PARAM>
        /// <RETURNS></RETURNS>
        /// </summary>
        public static Bitmap ImageZoom(Image image, int destHeight, int destWidth)
        {
            try
            {
                Image imgSource = image;
                int sW = 0, sH = 0;
                // 按比例缩放
                int sWidth = imgSource.Width;
                int sHeight = imgSource.Height;

                if (sHeight > destHeight || sWidth > destWidth)
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        sW = destWidth;
                        sH = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        sH = destHeight;
                        sW = (sWidth * destHeight) / sHeight;
                    }
                }
                else
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        sW = destWidth;
                        sH = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        sH = destHeight;
                        sW = (sWidth * destHeight) / sHeight;
                    }

                }
                Bitmap outBmp = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage(outBmp);
                g.Clear(Color.Black);

                // 设置画布的描绘质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
                g.Dispose();
                imgSource.Dispose();

                return outBmp;
            }
            catch (Exception e)
            {

                return null;
            }

            #region  保存图片是的操作
            //// 以下代码为保存图片时，设置压缩质量
            //EncoderParameters encoderParams = new EncoderParameters();
            //long[] quality = new long[1];
            //quality[0] = 100;

            //EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //encoderParams.Param[0] = encoderParam;


            //try
            //{
            //    //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            //    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            //    ImageCodecInfo jpegICI = null;
            //    for (int x = 0; x < arrayICI.Length; x++)
            //    {
            //        if (arrayICI[x].FormatDescription.Equals("JPEG"))
            //        {
            //            jpegICI = arrayICI[x];//设置JPEG编码
            //            break;
            //        }
            //    }

            //    if (jpegICI != null)
            //    {
            //        outBmp.Save(destFile, jpegICI, encoderParams);
            //    }
            //    else
            //    {
            //        outBmp.Save(destFile, thisFormat);
            //    }

            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
            //finally
            //{
            //    imgSource.Dispose();
            //    outBmp.Dispose();
            //}
            #endregion
        }

        /// <summary>
        /// 计算基数加或减比例*基数的值
        /// </summary>
        /// <param name="radix">基数</param>
        /// <param name="rate">比例</param>
        /// <param name="bOptAdd">为true：加计算，false：减计算</param>
        /// <returns>计算后的结果</returns>
        private static int CalculatePixel(int radix, float rate, bool bOptAdd)
        {
            int result = 0;

            if (bOptAdd)
            {
                result = (int)(radix + radix * rate);
            }
            else
            {
                result = (int)(radix - radix * rate);
            }

            return result;
        }

        /// <summary>
        /// 检验RGB颜色的值是否在0-255之间
        /// </summary>
        /// <param name="args">RGB颜色的值</param>
        /// <returns>判断后的颜色</returns>
        private static int CheckRange(int args)
        {
            args = args > 0 ? args : 0;
            args = args < 255 ? args : 255;
            return args;
        }

        /// <summary>
        /// base64编码的字符串转IMAGE
        /// </summary>
        /// <param name="base64string">BASE64编码的字串</param>
        /// <returns>位图图片</returns>
        public static Bitmap GetImageFromBase64(string base64string)
        {
            if (base64string == null || base64string.Length == 0)
            {
                return null;
            }
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        /// <summary>
        /// 图片转BASE64编码的字符串
        /// </summary>
        /// <param name="bmpImage">图片对象</param>
        /// <returns>BASE64编码的字符串</returns>
        public static string GetBase64FromImage(Bitmap bmpImage)
        {
            if (bmpImage == null)
            {
                return null;
            }

            string strBaser64 = "";
            try
            {
                MemoryStream ms = new MemoryStream();
                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strBaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("转换失败");
            }
            return strBaser64;
        }

        #region 保存图片为图片文件

        /// <summary>
        /// 保存内存中的图片为文件
        /// </summary>
        /// <param name="bmImage">内存中的bitmap图片</param>
        /// <param name="strFileName">要保存的文件名</param>
        /// <param name="strFileType">文件类型</param>
        /// <returns></returns>
        public static bool SaveImage(Bitmap bmImage, string strFileName, ImageFormat fileFormat)
        {
            try
            {
                bmImage.Save(strFileName, fileFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 保存内存中的图片为文件
        /// </summary>
        /// <param name="bmImage">内存中的bitmap图片</param>
        /// <param name="strFileName">要保存的文件名</param>
        /// <param name="strFileType">文件类型</param>
        /// <returns></returns>
        public static bool SaveImage(Image imgImage, string strFileName, ImageFormat fileFormat)
        {
            try
            {
                imgImage.Save(strFileName, fileFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion


        /// <summary>
        /// 改变图片大小
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="Mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH, int Mode)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }


        #region 压缩图片
        /// <summary>
        /// 得到指定mimeType的ImageCodecInfo
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns>得到指定mimeType的ImageCodecInfo</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// 压缩图片 支持图片质量参数
        /// </summary>
        /// <param name="bmp">图片</param>
        /// <param name="FileName">文件保存路径，需要完成路径包括路径已经文件名如：d:\1.bmp </param>
        /// <param name="Qty">压缩质量参数  100 - 0 图片清晰度递减</param>
        /// <param name="MimeType">MimeType型图片格式 如：image/jpeg</param>
        /// <returns>是否成功保存</returns>
        public static bool SaveAsJPEG(Bitmap bmp, string FileName, int Qty, string MimeType)
        {
            try
            {
                EncoderParameter p;
                EncoderParameters ps;

                ps = new EncoderParameters(1);

                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Qty);
                ps.Param[0] = p;

                bmp.Save(FileName, GetCodecInfo(MimeType), ps);

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 将二进制字节转化为图片
        /// </summary>
        /// <param name="imageBytes">二进制字节</param>
        /// <returns>返回图片</returns>
        public static Image GetImage(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream(imageBytes);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        /// <summary>
        /// 将二进制字节转化为GDI+位图
        /// </summary>
        /// <param name="imageBytes">二进制字节</param>
        /// <returns></returns>
        public static Bitmap GetBitmap(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream(imageBytes);
            Bitmap image = (Bitmap)Image.FromStream(ms, true);
            return image;
        }
        #endregion


        /// <summary>
        /// 图片转化为ico
        /// </summary>
        /// <param name="path">ico路径</param>
        /// <returns>ico</returns>
        public static Icon ImageToIco(string path)
        {
            System.Drawing.Image appIcon = null;
            appIcon = System.Drawing.Bitmap.FromFile(path);
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            appIcon.Save(mStream, System.Drawing.Imaging.ImageFormat.Png);
            Icon icon1 = Icon.FromHandle(new Bitmap(mStream).GetHicon());
            mStream.Close();
            return icon1;

        }

    }
}
