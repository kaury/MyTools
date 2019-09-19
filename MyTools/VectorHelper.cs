using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Platform.Utility
{
    public class VectorHelper
    {
        public int BitmapWidth = 200;
        private int BitmapHeight { get { return BitmapWidth; } }
        public int CircleWidth = 190;
        private int CircleHeight { get { return CircleWidth; } }
        public string VectorPath = "";
        public string VectorName = "VectorTemp.jpg";
        public string VectorFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(VectorPath))
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), VectorName);
                }
                return Path.Combine(VectorPath, VectorName);
            }
        }
        public Color L1Color = Color.Red;
        public Color L2Color = Color.Yellow;
        public Color L3Color = Color.Blue;
        public Color L1FontColor = Color.Red;
        public Color L2FontColor = Color.Orange;
        public Color L3FontColor = Color.Blue;
        public string UL1Name = "U1";
        public string UL2Name = "U2";
        public string UL3Name = "U3";
        public string IL1Name = "I1";
        public string IL2Name = "I2";
        public string IL3Name = "I3";
        /// <summary>
        /// 向量模式：0分相，1合成
        /// </summary>
        public int Mode = 0;
        public float UL1Angle = 0;
        public float UL2Angle = 240;
        public float UL3Angle = 120;
        public float IL1Angle = 0;
        public float IL2Angle = 240;
        public float IL3Angle = 120;
        public float UL1Value = 220;
        public float UL2Value = 220;
        public float UL3Value = 220;
        public float IL1Value = 1;
        public float IL2Value = 1;
        public float IL3Value = 1;

        private int offsetText = 10;

        public bool DrawVector()
        {
            using (Bitmap bmp = new Bitmap(BitmapWidth, BitmapHeight))
            {
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = new Pen(Color.Black);
                g.DrawEllipse(pen, (BitmapWidth - CircleWidth) / 2, (BitmapWidth - CircleWidth) / 2, CircleWidth, CircleWidth);

                pen = new Pen(Color.Silver);
                g.DrawLine(pen, BitmapWidth / 2f, (BitmapWidth - CircleWidth) / 2, BitmapWidth / 2f, (BitmapWidth - CircleWidth) / 2 + CircleWidth);
                g.DrawLine(pen, (BitmapWidth - CircleWidth) / 2, BitmapWidth / 2f, (BitmapWidth - CircleWidth) / 2 + CircleWidth, BitmapWidth / 2f);

                Point CirclePoint = new Point(BitmapWidth / 2, BitmapWidth / 2);

                float Uradio = CircleWidth / 2f;
                if (UL1Value > 0)
                    DrawVecotr(g, UL1Angle, Uradio, UL1Name, L1Color, CirclePoint, L1FontColor);
                if (UL2Value > 0)
                    DrawVecotr(g, UL2Angle, Uradio, UL2Name, L2Color, CirclePoint, L2FontColor);
                if (UL3Value > 0)
                    DrawVecotr(g, UL3Angle, Uradio, UL3Name, L3Color, CirclePoint, L3FontColor);
                float Iradio = (CircleWidth / 2f - 30f);
                if (IL1Value > 0)
                    DrawVecotr(g, IL1Angle, Iradio, IL1Name, L1Color, CirclePoint, L1FontColor);
                if (IL2Value > 0)
                    DrawVecotr(g, IL2Angle, Iradio, IL2Name, L2Color, CirclePoint, L2FontColor);
                if (IL3Value > 0)
                    DrawVecotr(g, IL3Angle, Iradio, IL3Name, L3Color, CirclePoint, L3FontColor);

                bmp.Save(VectorFullPath);
            }
            return true;
        }
        private void DrawVecotr(Graphics g, float Angle, float radius, string text, Color color, Point CircleCenterPoint, Color FontColor)
        {
            float angle = -1 * Angle % 360F;
            double x_offset = 0;
            double y_offset = 0;
            double Text_X_offset = 0;
            double Text_Y_offset = 0;
            double CenterCircle_X = CircleCenterPoint.X;
            double CenterCircle_Y = CircleCenterPoint.Y;
            var font = new Font("宋体", 9.5f);
            var textSize = g.MeasureString(text, font);

            x_offset = radius * Math.Sin(angle * Math.PI / 180) + CenterCircle_X;
            y_offset = -radius * Math.Cos(angle * Math.PI / 180) + CenterCircle_Y;

            if (Math.Abs(angle) <= 90.0)
            {
                Text_X_offset = (radius - offsetText) * Math.Sin((angle + 1) * Math.PI / 180) + CenterCircle_X;
                Text_Y_offset = -(radius - offsetText) * Math.Cos((angle + 1) * Math.PI / 180) + CenterCircle_Y - textSize.Height * Math.Sin(Math.Abs(angle) * Math.PI / 180);
            }
            else if (Math.Abs(angle) <= 180)
            {
                Text_X_offset = (radius - offsetText) * Math.Sin((angle + 1) * Math.PI / 180) + CenterCircle_X - textSize.Width * Math.Sin(Math.Abs(angle + 90) * Math.PI / 180);
                Text_Y_offset = -(radius - offsetText) * Math.Cos((angle + 1) * Math.PI / 180) + CenterCircle_Y - textSize.Height;
            }
            else if (Math.Abs(angle) <= 270)
            {
                Text_X_offset = (radius - offsetText) * Math.Sin((angle + 1) * Math.PI / 180) + CenterCircle_X - textSize.Width * Math.Sin(Math.Abs(angle + 90) * Math.PI / 180);
                Text_Y_offset = -(radius - offsetText) * Math.Cos((angle + 1) * Math.PI / 180) + CenterCircle_Y + textSize.Height * Math.Sin(Math.Abs(angle + 180) * Math.PI / 180);
            }
            else if (Math.Abs(angle) <= 360)
            {
                Text_X_offset = (radius - offsetText) * Math.Sin((angle - 1) * Math.PI / 180) + CenterCircle_X - textSize.Width * Math.Sin(Math.Abs(angle + 90) * Math.PI / 180);
                Text_Y_offset = -(radius - offsetText) * Math.Cos((angle - 1) * Math.PI / 180) + CenterCircle_Y + textSize.Height * Math.Sin(Math.Abs(angle + 180) * Math.PI / 180);
            }

            Pen pn = new Pen(color, 1);
            pn.CustomEndCap = new AdjustableArrowCap(3, 3, true);
            //画线
            g.DrawLine(pn, (float)CenterCircle_X, (float)CenterCircle_Y, (float)x_offset, (float)y_offset);
            //字字体
            g.DrawString(text, font, new SolidBrush(FontColor), (float)Text_X_offset, (float)Text_Y_offset);
        }
    }
}
