using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTools
{
    /// <summary>
    /// polar coordinates
    /// </summary>
    public class Polar
    {
        #region polar coordinates

        private double modul = 0.0;
        private double angle = 0.0;

        /// <summary>
        /// 获取或设置极坐标的模
        /// </summary>
        public double Modul {
            get {
                return modul;
            }
            set {
                modul = value;
            }
        }

        /// <summary>
        /// 获取或设置极坐标的角度
        /// </summary>
        public double Angle {
            get {
                return angle;
            }
            set {
                angle = value;
            }
        }

        /// <summary>
        /// 极坐标(polar coordinates)形式的构造函数
        /// </summary>
        /// <param name="dbreal">实部</param>
        /// <param name="dbImage">虚部</param>
        public Polar(double dbreal, double dbImage)
        {
            modul = dbreal;
            angle = dbImage;
        }
        public Complex GetComplex(Polar polar)
        {
            return new Complex(polar.Modul * Math.Cos(polar.Angle / 180 * Math.PI), polar.Modul * Math.Sin(polar.Angle / 180 * Math.PI));
        }
        public static Polar operator +(Polar comp1, Polar comp2)
        {
            return comp1.Add(comp2);
        }
        public Polar Add(Polar comp)
        {
            Complex cpx = GetComplex(this) + GetComplex(comp);

            return new Polar(cpx.GetModul(), cpx.GetAngle() / Math.PI * 180);
        }
        /// <summary>
        /// 旋转angle度
        /// </summary>
        /// <param name="angle">°</param>
        /// <returns></returns>
        public Polar Rotate(double angle)
        {
            return new Polar(modul, this.angle + angle);
        }
        #endregion polar coordinates
    }
}
