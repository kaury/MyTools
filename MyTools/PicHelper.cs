using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace MyTools
{
    class PicHelper
    {
        /// <summary>
        /// 根据指定尺寸得到按比例缩放的尺寸,返回true表示以更改尺寸
        /// </summary>
        /// <param name="picWidth">图片宽度</param>
        /// <param name="picHeight">图片高度</param>
        /// <param name="specifiedWidth">指定宽度</param>
        /// /// <param name="specifiedHeight">指定高度</param>
        /// <returns>返回true表示以更改尺寸</returns>
        private static bool GetPicZoomSize(ref int picWidth, ref int picHeight, int specifiedWidth, int specifiedHeight)
        {
            int sW = 0, sH = 0;
            Boolean isZoomSize = false;
            Size tem_size = new Size(picWidth, picHeight);
            if (tem_size.Width > specifiedWidth || tem_size.Height > specifiedHeight)
            {
                if ((tem_size.Width * specifiedHeight) > (tem_size.Height * specifiedWidth))
                {
                    sW = specifiedWidth;
                    sH = (specifiedWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = specifiedHeight;
                    sW = (tem_size.Width * specifiedHeight) / tem_size.Height;
                }
                isZoomSize = true;
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            picHeight = sH;
            picWidth = sW;
            return isZoomSize;
        }
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag, bool actualSize = true)
        {
            if (!System.IO.File.Exists(sFile)) return false;
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = iSource.Width, sH = iSource.Height;

            GetPicZoomSize(ref sW, ref sH, dWidth, dHeight);

            Bitmap ob = new Bitmap(actualSize ? sW : dWidth, actualSize ? sH : dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.White);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle(actualSize ? 0 : (dWidth - sW) / 2, actualSize ? 0 : (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            ob = CutImageWhitePart(ob, 3);
            ob.MakeTransparent();
            ob.MakeTransparent(Color.White);
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("PNG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        /// <summary>  
        /// 剪去图片空余白边  
        /// </summary>  
        /// <param name="bmp">Bitmap</param>  
        /// <param name="WhiteBarRate">检索白边最大像素</param>  
        public static Bitmap CutImageWhitePart(Bitmap bmp, int MaxWhiteBarPixel = 5)
        {
            int limitPixel = MaxWhiteBarPixel;
            int top = 0, left = 0;
            int right = bmp.Width, bottom = bmp.Height;
            Color white = Color.White;
            //寻找最上面的标线高度,从左(0)到右，从上(0)到下  
            bool find = false;
            for (int i = 0; i < bmp.Height && i < limitPixel; i++)//行  
            {
                for (int j = i; j < bmp.Width - i; j++)//列  
                {
                    Color c = bmp.GetPixel(j, i);
                    if (IsTransparent(c))
                    {
                        top = i + 1;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            //寻找最左边的标线宽度，从上（top位）到下，从左到右  
            find = false;
            for (int i = 0; i < bmp.Width && i < limitPixel; i++)//列  
            {
                for (int j = top; j < bmp.Height; j++)//行  
                {
                    Color c = bmp.GetPixel(i, j);
                    if (IsTransparent(c))
                    {
                        left = i + 1;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            ////寻找最下边标线高度，从下到上，从左到右 
            find = false;
            for (int i = bmp.Height - 1; i >= 0 && i > bmp.Height - limitPixel; i--)//行  
            {
                for (int j = left; j < bmp.Width; j++)//列  
                {
                    Color c = bmp.GetPixel(j, i);
                    if (IsTransparent(c))
                    {
                        bottom = i;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            //寻找最右边的标线宽度，从上到下，从右往左  
            find = false;
            for (int i = bmp.Width - 1; i >= 0 && i > bmp.Width - limitPixel; i--)//列  
            {
                for (int j = 0; j <= bottom; j++)//行  
                {
                    Color c = bmp.GetPixel(i, j);
                    if (IsTransparent(c))
                    {
                        right = i;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            int iWidth = right - left;
            int iHeight = bottom - top;
            return Cut(bmp, left, top, right - left, bottom - top);
        }
        /// <summary>  
        /// 剪去图片空余白边  
        /// </summary>  
        /// <param name="FilePath">源文件</param>  
        /// <param name="WhiteBarRate">保留空白边比例</param>  
        public static void CutImageWhitePart(string FilePath, int WhiteBarRate)
        {
            int limitPixel = 5;
            Bitmap bmp = new Bitmap(FilePath);
            int top = 0, left = 0;
            int right = bmp.Width, bottom = bmp.Height;
            Color white = Color.White;
            //寻找最上面的标线,从左(0)到右，从上(0)到下  
            bool find = false;
            for (int i = 0; i < bmp.Height && i < limitPixel; i++)//行  
            {
                for (int j = i; j < bmp.Width - i; j++)//列  
                {
                    Color c = bmp.GetPixel(j, i);
                    if (IsTransparent(c))
                    {
                        top = i + 1;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            //寻找最左边的标线，从上（top位）到下，从左到右  
            find = false;
            for (int i = 0; i < bmp.Width && i < limitPixel; i++)//列  
            {
                for (int j = top; j < bmp.Height; j++)//行  
                {
                    Color c = bmp.GetPixel(i, j);
                    if (IsTransparent(c))
                    {
                        left = i + 1;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            ////寻找最下边标线，从下到上，从左到右 
            find = false;
            for (int i = bmp.Height - 1; i >= 0 && i > bmp.Height - limitPixel; i--)//行  
            {
                for (int j = left; j < bmp.Width; j++)//列  
                {
                    Color c = bmp.GetPixel(j, i);
                    if (IsTransparent(c))
                    {
                        bottom = i;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            //寻找最右边的标线，从上到下，从右往左  
            find = false;
            for (int i = bmp.Width - 1; i >= 0 && i > bmp.Width - limitPixel; i--)//列  
            {
                for (int j = 0; j <= bottom; j++)//行  
                {
                    Color c = bmp.GetPixel(i, j);
                    if (IsTransparent(c))
                    {
                        right = i;
                        find = true;
                        break;
                    }
                }
                if (!find) break;
                if (find) find = false;
            }
            int iWidth = right - left;
            int iHeight = bottom - top;
            int blockWidth = Convert.ToInt32(iWidth * WhiteBarRate / 100);
            bmp = Cut(bmp, left - blockWidth, top - blockWidth, right - left + 2 * blockWidth, bottom - top + 2 * blockWidth);
            if (bmp != null)
            {
                bmp.Save(System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), bmp.GetHashCode().ToString() + ".png"), ImageFormat.Png);
                bmp.Dispose();
            }
        }

        /// <summary>  
        /// Cut  
        /// </summary>  
        /// <param name="b"></param>  
        /// <param name="StartX"></param>  
        /// <param name="StartY"></param>  
        /// <param name="iWidth"></param>  
        /// <param name="iHeight"></param>  
        /// <returns></returns>  
        private static Bitmap Cut(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>  
        /// 判断透明
        /// </summary>  
        /// <param name="c"></param>  
        /// <returns></returns>  
        private static bool IsTransparent(Color c)
        {
            if (c.A > 0 || c.R > 0 || c.G > 0 || c.B > 0) return true;
            else return false;
        }
    }
}
